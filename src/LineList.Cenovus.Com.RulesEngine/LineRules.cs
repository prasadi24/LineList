using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;

namespace LineList.Cenovus.Com.RulesEngine
{
    public static class LineRules
    {
        public static string GetNextRevision(LineRevision lineRevision, ILineRevisionService lineRevisionService)
        {
            //6.4.14 LI10 - compare the attribute values on this LineRevision with those
            //              from the line revision last issued for this line list.

            LineRevision lastIssuedLineRev = GetLastIssuedLineRevision(lineRevision.Line.LocationId, lineRevision.Line.CommodityId, lineRevision.Line.SequenceNumber, lineRevision.Line.ChildNumber, lineRevision.LineListRevision.LineListId, lineRevisionService);

            if (HasChanged(lineRevision, lastIssuedLineRev))
            {
                //if there are any diffs, this LineRevision is assigned its Line List revision...
                return lineRevision.LineListRevision.DocumentRevision;
            }
            else
            {
                //otherwise, the line revision is left unchanged...
                if (lastIssuedLineRev != null)
                    return lastIssuedLineRev.Revision;
                else
                    return lineRevision.Revision;
            }
        }

        public static async Task<bool> CanBeDiscarded(Guid lineRevisionId, string userName, ILineRevisionService lineRevisionService)
        {
            var items = await CanBeDiscarded(new Guid[] { lineRevisionId }, userName, lineRevisionService);
            return items.Length == 1 && items[0] == lineRevisionId;
        }

        public static async Task<Guid[]> CanBeDiscarded(Guid[] possibles, string userName, ILineRevisionService lineRevisionService)
        {
            //var allLines = await lineRevisionService.GetAll();
            ////var linesToDiscard = from m in lineRevisionService.GetAll().Result
            ////                     where
            ////                            // selected
            ////                            possibles.Contains(m.Id)
            ////                            // checked out to current user
            ////                            && m.CheckedOutBy == userName
            ////                            // not previously issued on this line list
            ////                            && !lineRevisionService.GetAll().Result.Where(z => z.LineListRevision.LineListStatus.IsIssuedOfId != null && z.LineId == m.LineId && z.LineListRevision.LineListId == m.LineListRevision.LineListId).Any() // no issued version
            ////                                                                                                                                                                                                                                       // has no children on this line list
            ////                            && !lineRevisionService.GetAll().Result.Where(z => z.Line.ChildNumber == 0 && z.LineId == m.LineId && lineRevisionService.GetAll().Result.Where(x => x.LineListRevision.LineListId == m.LineListRevision.LineListId && x.Line.LocationId == z.Line.LocationId && x.Line.CommodityId == z.Line.CommodityId && x.Line.SequenceNumber == z.Line.SequenceNumber && x.Line.ChildNumber != 0).Any()).Any()
            ////                     select m.Id;
            ////                     
            ////return linesToDiscard.ToArray<Guid>();
            //var linesToDiscard = allLines
            //    .Where(m =>
            //        possibles.Contains(m.Id) &&
            //        m.CheckedOutBy == userName &&
            //        !allLines.Any(z =>
            //            z.LineListRevision != null &&
            //            z.LineListRevision.LineListStatus != null &&
            //            z.LineListRevision.LineListStatus.IsIssuedOfId != null &&
            //            z.LineId == m.LineId &&
            //            z.LineListRevision.LineListId == m.LineListRevision?.LineListId) &&
            //        !allLines.Any(z =>
            //            z.Line.ChildNumber != 0 &&
            //            z.Line.LocationId == m.Line.LocationId &&
            //            z.Line.CommodityId == m.Line.CommodityId &&
            //            z.Line.SequenceNumber == m.Line.SequenceNumber &&
            //            z.LineListRevision.LineListId == m.LineListRevision.LineListId))
            //    .Select(m => m.Id)
            //    .ToArray();

            var linesToDiscard= await lineRevisionService.GetDiscardableIds(possibles, userName);

            return linesToDiscard;
        }

        public static void Discard(Guid lineRevisionId)
        {
            //db.ObjectContext.ExecuteStoreCommand("DELETE FROM LineRevisionSegment WHERE LineRevisionId = {0}", lineRevisionId);
            //db.ObjectContext.ExecuteStoreCommand("DELETE FROM LineRevisionOperatingMode WHERE LineRevisionId = {0}", lineRevisionId);
            //db.ObjectContext.ExecuteStoreCommand("DELETE FROM LineRevision WHERE Id={0}", lineRevisionId);
            //db.ObjectContext.ExecuteStoreCommand("DELETE FROM Line WHERE Id NOT IN (SELECT LineId FROM LineRevision)");
            //db.SaveChanges();
        }

        public static async Task<LineRevision> DeleteLine(LineRevision line, string userName,
            ILineRevisionService lineRevisionService,
            ILineRevisionSegmentService lineRevisionSegmentService,
            ILineRevisionOperatingModeService lineRevisionOperatingModeService,
            ILineStatusService lineStatusService
            )
        {
            bool canBeDiscarded = await CanBeDiscarded(line.Id, userName, lineRevisionService);

            if (line.LineStatus.Name.ToUpper() == "RESERVED" || canBeDiscarded)
            {
                //remove from DB
                var segments = await lineRevisionSegmentService.GetSegmentsByLineRevisionId(line.Id);
                foreach (var segment in segments)
                    await lineRevisionSegmentService.Remove(segment);

                var opModes = await lineRevisionOperatingModeService.GetOperatingModesByLineRevisionId(line.Id);
                foreach (var opMode in opModes)
                    await lineRevisionOperatingModeService.Remove(opMode);

                await lineRevisionService.Remove(line);
            }
            else
            {
                line = await lineRevisionService.GetById(line.Id);

                //reset the line status to "DELETED"
                //line.LineStatusId = (await lineStatusService.GetAll()).First(x => x.Name.ToUpper() == "DELETED").Id;
                line.LineStatusId = await lineStatusService.GetStatusIdByName("DELETED");


                //clear out all engineering data except for SpecificationId, LocationId, CommodityId, and Line Sequence #.
                await ClearLineAttributes(line, lineRevisionOperatingModeService, lineRevisionSegmentService);

                line.LineRevisionOperatingModes.First().Notes = "DELETED"; // enhancement 133b;
                line.ModifiedBy = userName;
                line.ModifiedOn = DateTime.Now;

                //line = (await lineRevisionService.GetAll()).Single(m => m.Id == line.Id);
                line.Revision = GetNextRevision(line, lineRevisionService);
                line.ValidationState = (int)LineRevisionHardValidationState.Pass;
            }

            return line;
        }

        public static LineRevision GetLastIssuedLineRevision(Guid LocationId,
            Guid CommodityId,
            string SequenceNo,
            int childNumber,
            Guid lineListId,
            ILineRevisionService lineRevisionService
            )
        {
            //return the matching line revision for the LAST issued line list

            var lines = from x in lineRevisionService.GetAll().Result
                        where x.Line.LocationId == LocationId &&
                                x.Line.CommodityId == CommodityId &&
                                x.Line.SequenceNumber == SequenceNo &&
                                x.Line.ChildNumber == childNumber &&
                                x.LineListRevision.LineListStatus.IsIssuedOfId != null &&
                                x.LineListRevision.LineListId == lineListId

                        orderby x.LineListRevision.IssuedOn descending,
                            x.ModifiedOn descending,
                            x.IsActive descending
                        select x;

            LineRevision line = null;

            if (lines.Any())
                line = lines.First();
            else
                line = null;

            return line;
        }

        public static async Task<int> PasteLineAttributes(Guid fromLineRevisionId, Guid[] toLineRevisionId,
            string userName, ILineRevisionService lineRevisionService,
            ILineRevisionSegmentService lineRevisionSegmentService,
            ILineRevisionOperatingModeService lineRevisionOperatingModeService,
            ILocationService locationService,
            ICommodityService commodityService,
            IPipeSpecificationService pipeSpecificationService,
            ISizeNpsService sizeNpsService,
            IInsulationThicknessService insulationThicknessService,
            ITracingTypeService tracingTypeService,
            IInsulationTypeService insulationTypeService
            )
        {
            //get the LineListRev to copy
            LineRevision sourceLine = await lineRevisionService.GetById(fromLineRevisionId);

            int count = 0;

            foreach (var id in toLineRevisionId)
            {
                LineRevision destLineRev = await lineRevisionService.GetById(fromLineRevisionId);
                if (destLineRev != null && destLineRev.IsCheckedOut &&
                    destLineRev.CheckedOutBy == userName &&
                    sourceLine.SpecificationId == destLineRev.SpecificationId)
                //lines being pasted to must be checked out to the current user
                //Bug 409: lines being pasted must share the same Spec as the source.
                {
                    //copy the attributes of source line to dest lines
                    //except Specification, Area, Location, Commodity, Line Sequence number, and Line Revision
                    destLineRev.CorrosionAllowancePipeId = sourceLine.CorrosionAllowancePipeId;
                    destLineRev.DesignPressurePipe = sourceLine.DesignPressurePipe;
                    destLineRev.DesignTemperatureMaximumPipe = sourceLine.DesignTemperatureMaximumPipe;
                    destLineRev.DesignTemperatureMinimumPipe = sourceLine.DesignTemperatureMinimumPipe;
                    destLineRev.ExpansionTemperature = sourceLine.ExpansionTemperature;
                    destLineRev.InternalCoatingLinerId = sourceLine.InternalCoatingLinerId;
                    destLineRev.LineNumber = sourceLine.LineNumber;
                    destLineRev.MinimumDesignMetalTemperature = sourceLine.MinimumDesignMetalTemperature;
                    destLineRev.NdeCategoryPipeId = sourceLine.NdeCategoryPipeId;
                    destLineRev.OriginatingPID = sourceLine.OriginatingPID;
                    destLineRev.OtherInspectionId = sourceLine.OtherInspectionId;
                    destLineRev.PipeSpecificationId = sourceLine.PipeSpecificationId;
                    destLineRev.PostWeldHeatTreatmentId = sourceLine.PostWeldHeatTreatmentId;
                    //destLineRev.PressureProtectionId = sourceLine.PressureProtectionId; //SER 435.  Changes by Armando Chaves.
                    destLineRev.SchedulePipeId = sourceLine.SchedulePipeId;
                    destLineRev.SizeNpsPipeId = sourceLine.SizeNpsPipeId;
                    destLineRev.StressAnalysisId = sourceLine.StressAnalysisId;
                    destLineRev.TestMediumPipeId = sourceLine.TestMediumPipeId;
                    destLineRev.TestPressurePipe = sourceLine.TestPressurePipe;
                    destLineRev.UpsetPressurePipe = sourceLine.UpsetPressurePipe;
                    destLineRev.UpsetTemperaturePipe = sourceLine.UpsetTemperaturePipe;
                    destLineRev.WallThicknessPipe = sourceLine.WallThicknessPipe;
                    destLineRev.XrayPipeId = sourceLine.XrayPipeId;

                    // destLineRev.ModularId  = sourceLine.ModularId;
                    //annulus values
                    destLineRev.IsJacketed = sourceLine.IsJacketed;
                    destLineRev.CorrosionAllowanceAnnulusId = sourceLine.CorrosionAllowanceAnnulusId;
                    destLineRev.DesignPressureAnnulus = sourceLine.DesignPressureAnnulus;
                    destLineRev.DesignTemperatureMaximumAnnulus = sourceLine.DesignTemperatureMaximumAnnulus;
                    destLineRev.DesignTemperatureMinimumAnnulus = sourceLine.DesignTemperatureMinimumAnnulus;
                    destLineRev.NdeCategoryAnnulusId = sourceLine.NdeCategoryAnnulusId;
                    destLineRev.ScheduleAnnulusId = sourceLine.ScheduleAnnulusId;
                    destLineRev.SizeNpsAnnulusId = sourceLine.SizeNpsAnnulusId;
                    destLineRev.TestMediumAnnulusId = sourceLine.TestMediumAnnulusId;
                    destLineRev.TestPressureAnnulus = sourceLine.TestPressureAnnulus;
                    destLineRev.UpsetPressureAnnulus = sourceLine.UpsetPressureAnnulus;
                    destLineRev.UpsetTemperatureAnnulus = sourceLine.UpsetTemperatureAnnulus;
                    destLineRev.WallThicknessAnnulus = sourceLine.WallThicknessAnnulus;
                    destLineRev.XrayAnnulusId = sourceLine.XrayAnnulusId;

                    destLineRev.ModifiedBy = userName;
                    destLineRev.ModifiedOn = DateTime.Now;

                    //bug 581 - clear the old segs + opmodes and add copies
                    //clear the old segments
                    var segmentsToRemove = (await lineRevisionSegmentService.GetAll()).Where(m => m.LineRevisionId == id).ToList();
                    foreach (var segment in segmentsToRemove)
                        await lineRevisionSegmentService.Remove(segment);  

                    var opModesToRemove = (await lineRevisionOperatingModeService.GetAll()).Where(m => m.LineRevisionId == id).ToList();
                    foreach (var opMode in opModesToRemove)
                        await lineRevisionOperatingModeService.Remove(opMode);  


                    //copy segments
                    var segmentsToCopy = await lineRevisionSegmentService.GetAll();
                    foreach (var segment in segmentsToCopy.Where(m => m.LineRevisionId == fromLineRevisionId))
                    {
                        //db.Detach(segment);
                        segment.Id = Guid.NewGuid();
                        segment.LineRevisionId = id;
                        segment.ModifiedBy = userName;
                        segment.ModifiedOn = DateTime.Now;
                        await lineRevisionSegmentService.Add(segment);
                    }

                    //copy operating modes, exclude Notes, Line From, and Line To.
                    var opModesToCopy = await lineRevisionOperatingModeService.GetAll();
                    foreach (var opMode in opModesToCopy.Where(m => m.LineRevisionId == fromLineRevisionId))
                    {
                        //db.Detach(opMode);
                        opMode.Id = Guid.NewGuid();
                        opMode.LineRevisionId = id;
                        opMode.Notes = string.Empty;
                        opMode.LineRoutingFrom = string.Empty;
                        opMode.LineRoutingTo = string.Empty;
                        opMode.ModifiedBy = userName;
                        opMode.ModifiedOn = DateTime.Now;
                        opMode.IsAbsaRegistration = string.Empty; //SER 435.  Changes by Armando Chaves.
                        opMode.PressureProtectionId = (Guid?)null; //Armand. SER 435.Revisado
                        await lineRevisionOperatingModeService.Add(opMode);
                    }

                    //update the revision
                    destLineRev.Revision = GetNextRevision(destLineRev, lineRevisionService);

                    //if the revision changed, and it's reserved, bump it up to a non-reserved status
                    if (destLineRev.LineStatus != null && destLineRev.LineStatus.IsDefaultForReservation)
                        if (destLineRev.LineListRevision != null && destLineRev.LineListRevision.LineListStatus != null)
                            destLineRev.LineStatusId = destLineRev.LineListRevision.LineListStatus.CorrespondingLineStatusId;

                    //update the line number now.
                    destLineRev.LineNumber = LineNumberGenerator.Evaluate(destLineRev, locationService, commodityService,
                        pipeSpecificationService, sizeNpsService, insulationThicknessService, tracingTypeService, insulationTypeService);

                    //db.SaveChanges();

                    destLineRev.ValidationState = (int)LineRevisionValidation.GetHardRevisionValidationState(destLineRev);

                    //db.SaveChanges();

                    count++;
                }
            }
            return count;
        }

        public static bool HasChanged(LineRevision lineRevision, ILineRevisionService lineRevisionService)
        {
            var results = lineRevisionService.GetAll().Result.Where(m => m.Id == lineRevision.Id);
            if (results.Any())
            {
                LineRevision previousVersion = results.First();
                return HasChanged(lineRevision, previousVersion);
            }
            else
                return true;
        }

        public static bool HasChanged(LineRevision lineRevision, ILineRevisionService lineRevisionService, bool checkService)
        {
            LineRevision lastIssuedLineRev = GetLastIssuedLineRevision(lineRevision.Line.LocationId, lineRevision.Line.CommodityId, lineRevision.Line.SequenceNumber, lineRevision.Line.ChildNumber, lineRevision.LineListRevision.LineListId, lineRevisionService);
            return HasChanged(lineRevision, lastIssuedLineRev);
        }

        public static bool HasChanged(LineRevision lineRevision, LineRevision lastIssuedLineRev)
        {
            bool diffs = false;
            if (lastIssuedLineRev != null && lineRevision != null)
            {
                diffs = diffs || (lineRevision.AreaId != lastIssuedLineRev.AreaId);
                diffs = diffs || (lineRevision.Line.CommodityId != lastIssuedLineRev.Line.CommodityId);
                diffs = diffs || (lineRevision.CorrosionAllowanceAnnulusId != lastIssuedLineRev.CorrosionAllowanceAnnulusId);
                diffs = diffs || (lineRevision.CorrosionAllowancePipeId != lastIssuedLineRev.CorrosionAllowancePipeId);
                diffs = diffs || ((lineRevision.DesignPressureAnnulus ?? string.Empty) != (lastIssuedLineRev.DesignPressureAnnulus ?? string.Empty));
                diffs = diffs || ((lineRevision.DesignPressurePipe ?? string.Empty) != (lastIssuedLineRev.DesignPressurePipe ?? string.Empty));
                diffs = diffs || ((lineRevision.DesignTemperatureMaximumAnnulus ?? string.Empty) != (lastIssuedLineRev.DesignTemperatureMaximumAnnulus ?? string.Empty));
                diffs = diffs || ((lineRevision.DesignTemperatureMaximumPipe ?? string.Empty) != (lastIssuedLineRev.DesignTemperatureMaximumPipe ?? string.Empty));
                diffs = diffs || ((lineRevision.DesignTemperatureMinimumAnnulus ?? string.Empty) != (lastIssuedLineRev.DesignTemperatureMinimumAnnulus ?? string.Empty));
                diffs = diffs || ((lineRevision.DesignTemperatureMinimumPipe ?? string.Empty) != (lastIssuedLineRev.DesignTemperatureMinimumPipe ?? string.Empty));
                diffs = diffs || ((lineRevision.ExpansionTemperature ?? string.Empty) != (lastIssuedLineRev.ExpansionTemperature ?? string.Empty));
                diffs = diffs || (lineRevision.InternalCoatingLinerId != lastIssuedLineRev.InternalCoatingLinerId);
                diffs = diffs || (lineRevision.Line.ChildNumber != lastIssuedLineRev.Line.ChildNumber);
                diffs = diffs || (lineRevision.IsJacketed != lastIssuedLineRev.IsJacketed);

                //diffs = diffs || ((lineRevision.Line.ModularId ?? string.Empty) !=  (lastIssuedLineRev.Line.ModularId?? string.Empty));
                if (lastIssuedLineRev.ModularId == null || lastIssuedLineRev.ModularId == "")
                {
                    lastIssuedLineRev.ModularId = "";
                    diffs = diffs || (lineRevision.ModularId != lastIssuedLineRev.ModularId); //to make uprevving and backrevving work
                }
                else
                    diffs = diffs || (lineRevision.ModularId != lastIssuedLineRev.ModularId); //to make uprevving and backrevving work
                //don't compare them when they're both soft revisions
                if ((lineRevision.LineListRevision.LineListStatus.IsHardRevision != lastIssuedLineRev.LineListRevision.LineListStatus.IsHardRevision)
                    || (lineRevision.LineListRevision.LineListStatus.IsHardRevision && lastIssuedLineRev.LineListRevision.LineListStatus.IsHardRevision))
                    diffs = diffs || (lineRevision.LineStatusId != lastIssuedLineRev.LineStatusId);

                diffs = diffs || (lineRevision.Line.LocationId != lastIssuedLineRev.Line.LocationId);
                diffs = diffs || ((lineRevision.MinimumDesignMetalTemperature ?? string.Empty) != (lastIssuedLineRev.MinimumDesignMetalTemperature ?? string.Empty));
                diffs = diffs || (lineRevision.NdeCategoryAnnulusId != lastIssuedLineRev.NdeCategoryAnnulusId);
                diffs = diffs || (lineRevision.NdeCategoryPipeId != lastIssuedLineRev.NdeCategoryPipeId);
                diffs = diffs || ((lineRevision.OriginatingPID ?? string.Empty) != (lastIssuedLineRev.OriginatingPID ?? string.Empty));
                diffs = diffs || (lineRevision.OtherInspectionId != lastIssuedLineRev.OtherInspectionId);
                diffs = diffs || (lineRevision.PipeSpecificationId != lastIssuedLineRev.PipeSpecificationId);
                diffs = diffs || (lineRevision.PostWeldHeatTreatmentId != lastIssuedLineRev.PostWeldHeatTreatmentId);
                //diffs = diffs || (lineRevision.PressureProtectionId != lastIssuedLineRev.PressureProtectionId); //SER 435.  Changes by Armando Chaves.
                diffs = diffs || (lineRevision.ScheduleAnnulusId != lastIssuedLineRev.ScheduleAnnulusId);
                diffs = diffs || (lineRevision.SchedulePipeId != lastIssuedLineRev.SchedulePipeId);
                diffs = diffs || ((lineRevision.Line.SequenceNumber ?? string.Empty) != (lastIssuedLineRev.Line.SequenceNumber ?? string.Empty));
                // diffs = diffs || ((lineRevision.Line.ModularId ?? string.Empty) != (lastIssuedLineRev.Line.ModularId ?? string.Empty));

                diffs = diffs || (lineRevision.SizeNpsAnnulusId != lastIssuedLineRev.SizeNpsAnnulusId);
                diffs = diffs || (lineRevision.SizeNpsPipeId != lastIssuedLineRev.SizeNpsPipeId);
                diffs = diffs || (lineRevision.SpecificationId != lastIssuedLineRev.SpecificationId);
                diffs = diffs || (lineRevision.StressAnalysisId != lastIssuedLineRev.StressAnalysisId);
                diffs = diffs || (lineRevision.TestMediumAnnulusId != lastIssuedLineRev.TestMediumAnnulusId);
                diffs = diffs || (lineRevision.TestMediumPipeId != lastIssuedLineRev.TestMediumPipeId);
                diffs = diffs || ((lineRevision.TestPressureAnnulus ?? string.Empty) != (lastIssuedLineRev.TestPressureAnnulus ?? string.Empty));
                diffs = diffs || ((lineRevision.TestPressurePipe ?? string.Empty) != (lastIssuedLineRev.TestPressurePipe ?? string.Empty));
                diffs = diffs || ((lineRevision.UpsetPressureAnnulus ?? string.Empty) != (lastIssuedLineRev.UpsetPressureAnnulus ?? string.Empty));
                diffs = diffs || ((lineRevision.UpsetPressurePipe ?? string.Empty) != (lastIssuedLineRev.UpsetPressurePipe ?? string.Empty));
                diffs = diffs || ((lineRevision.UpsetTemperatureAnnulus ?? string.Empty) != (lastIssuedLineRev.UpsetTemperatureAnnulus ?? string.Empty));
                diffs = diffs || ((lineRevision.UpsetTemperaturePipe ?? string.Empty) != (lastIssuedLineRev.UpsetTemperaturePipe ?? string.Empty));
                diffs = diffs || ((lineRevision.WallThicknessAnnulus ?? 0) != (lastIssuedLineRev.WallThicknessAnnulus ?? 0));
                diffs = diffs || ((lineRevision.WallThicknessPipe ?? 0) != (lastIssuedLineRev.WallThicknessPipe ?? 0));
                diffs = diffs || (lineRevision.XrayAnnulusId != lastIssuedLineRev.XrayAnnulusId);
                diffs = diffs || (lineRevision.XrayPipeId != lastIssuedLineRev.XrayPipeId);

                if (!diffs)
                {
                    diffs = diffs || (lineRevision.LineRevisionOperatingModes.Count() != lastIssuedLineRev.LineRevisionOperatingModes.Count());
                    diffs = diffs || (lineRevision.LineRevisionSegments.Count() != lastIssuedLineRev.LineRevisionSegments.Count());
                }

                if (!diffs)
                {
                    foreach (var issuedSegment in lastIssuedLineRev.LineRevisionSegments)
                    {
                        var segments = lineRevision.LineRevisionSegments.Where(m => m.SegmentNumber == issuedSegment.SegmentNumber);
                        if (segments.Any())
                        {
                            var segment = segments.First();

                            diffs = diffs || (segment.InsulationMaterialId != issuedSegment.InsulationMaterialId);
                            diffs = diffs || (segment.InsulationThicknessId != issuedSegment.InsulationThicknessId);
                            diffs = diffs || (segment.InsulationTypeId != issuedSegment.InsulationTypeId);
                            diffs = diffs || (segment.PaintSystemId != issuedSegment.PaintSystemId);
                            diffs = diffs || (segment.SegmentTypeId != issuedSegment.SegmentTypeId);
                            diffs = diffs || ((segment.TracingDesignHoldTemperature ?? string.Empty) != (issuedSegment.TracingDesignHoldTemperature ?? string.Empty));
                            diffs = diffs || (segment.TracingDesignNumberOfTracersId != issuedSegment.TracingDesignNumberOfTracersId);
                            diffs = diffs || (segment.TracingTypeId != issuedSegment.TracingTypeId);
                        }
                        else
                            diffs = true;
                    }
                }

                if (!diffs)
                {
                    foreach (var issuedOpMode in lastIssuedLineRev.LineRevisionOperatingModes)
                    {
                        var opModes = lineRevision.LineRevisionOperatingModes.Where(m => m.OperatingModeNumber == issuedOpMode.OperatingModeNumber);
                        if (opModes.Any())
                        {
                            var opMode = opModes.First();

                            diffs = diffs || (opMode.CodeId != issuedOpMode.CodeId);
                            diffs = diffs || (opMode.CsaClassLocationId != issuedOpMode.CsaClassLocationId);
                            diffs = diffs || (opMode.CsaHvpLvpId != issuedOpMode.CsaHvpLvpId);
                            diffs = diffs || (opMode.FluidId != issuedOpMode.FluidId);
                            diffs = diffs || (opMode.FluidPhaseId != issuedOpMode.FluidPhaseId);
                            diffs = diffs || ((opMode.HoopStressLevel ?? 0) != (issuedOpMode.HoopStressLevel ?? 0));
                            diffs = diffs || (opMode.IsAbsaRegistration != issuedOpMode.IsAbsaRegistration);
                            diffs = diffs || (opMode.IsSourService != issuedOpMode.IsSourService);
                            diffs = diffs || ((opMode.LineRoutingFrom ?? string.Empty).Trim().ToUpper() != (issuedOpMode.LineRoutingFrom ?? string.Empty).Trim().ToUpper());
                            diffs = diffs || ((opMode.LineRoutingTo ?? string.Empty).Trim().ToUpper() != (issuedOpMode.LineRoutingTo ?? string.Empty).Trim().ToUpper());
                            diffs = diffs || ((opMode.Notes ?? string.Empty).Trim().ToUpper() != (issuedOpMode.Notes ?? string.Empty).Trim().ToUpper());
                            diffs = diffs || (opMode.OperatingModeId != issuedOpMode.OperatingModeId);
                            diffs = diffs || ((opMode.OperatingPressureAnnulus ?? string.Empty) != (issuedOpMode.OperatingPressureAnnulus ?? string.Empty));
                            diffs = diffs || ((opMode.OperatingPressurePipe ?? string.Empty) != (issuedOpMode.OperatingPressurePipe ?? string.Empty));
                            diffs = diffs || ((opMode.OperatingTemperatureAnnulus ?? string.Empty) != (issuedOpMode.OperatingTemperatureAnnulus ?? string.Empty));
                            diffs = diffs || ((opMode.OperatingTemperaturePipe ?? string.Empty) != (issuedOpMode.OperatingTemperaturePipe ?? string.Empty));
                            diffs = diffs || ((opMode.PipeMaterialSpecification ?? string.Empty).Trim().ToUpper() != (issuedOpMode.PipeMaterialSpecification ?? string.Empty).Trim().ToUpper());
                            diffs = diffs || (opMode.PressureProtectionId != issuedOpMode.PressureProtectionId); //SER 435.  Changes by Armando Chaves.

                            // diffs = diffs || (opMode.ModularId != issuedOpMode.ModularId);
                        }
                        else
                            diffs = true;
                    }
                }

                // diffs = diffs || ((lineRevision.Line.ModularId ?? string.Empty) != (lastIssuedLineRev.Line.ModularId ?? string.Empty));
            }
            return diffs;
        }

        public static async Task<int> CheckOutLines(Guid[] ids, string userName, ILineRevisionService lineRevisionService)
        {
            int numRowsCheckedOut = 0;
            //var revs = lineRevisionService.GetAll().Result.Where(m => ids.Contains(m.Id) && (m.IsCheckedOut == false || m.CheckedOutBy == userName));
            var revs = (await lineRevisionService.GetCheckOutLines(ids, userName));
                    //.Where(m => ids.Contains(m.Id) && (!m.IsCheckedOut || m.CheckedOutBy == userName));
            //checkout lines
            foreach (var lineRev in revs)
            {
                lineRev.IsCheckedOut = true;
                lineRev.CheckedOutBy = userName;
                lineRev.CheckedOutOn = DateTime.Now;
                await lineRevisionService.Update(lineRev);

                numRowsCheckedOut++;
            }
            //await lineRevisionService.SaveChanges();
            return numRowsCheckedOut;
        }

        public static async Task<int> CheckInLines(Guid[] ids, Guid epCompanyId, CurrentUser user, ILineRevisionService lineRevisionService)
        {
            int numRowsCheckedIn = 0;
            var admin = user.IsCenovusAdmin || user.EpAdmin.Contains(epCompanyId);
            var revs = await lineRevisionService.GetCheckInLines(ids, user.Username, admin);

            foreach (var lineRev in revs)
            {
                lineRev.IsCheckedOut = false;
                lineRev.CheckedOutBy = string.Empty;
                lineRev.CheckedOutOn = null;
                numRowsCheckedIn++;
                await lineRevisionService.Update(lineRev);
            }
            return numRowsCheckedIn;
        }

        public static async Task<LineRevision> CreateChild(Guid id, string userName,
            ILineRevisionService lineRevisionService,
            ILineListRevisionService lineListRevisionService,
            ILineService lineService,
            ILineRevisionSegmentService lineRevisionSegmentService,
            ILineRevisionOperatingModeService lineRevisionOperatingModeService
            )
        {
            LineRevision childLine = null;
            //childLine = lineRevisionService.GetAll().Result.Single(m => m.Id == id);
            childLine = await lineRevisionService.GetById(id);

            var lineListRevisions = await lineListRevisionService.GetAllLineListRevisions();//.Result.Single(m => m.Id == childLine.LineListRevisionId);
            var lineListRevision = lineListRevisions.Single(m => m.Id == childLine.LineListRevisionId);
            if (lineListRevision == null)
            {
                throw new ArgumentException("LineListRevision not found");
            }
            var revision = lineListRevision.DocumentRevision;
            var lineStatusId = lineListRevision.LineListStatus.CorrespondingLineStatusId;

            //var childNumber = lineService.GetAll().Result.Where(m => m.LocationId == childLine.Line.LocationId && m.CommodityId == childLine.Line.CommodityId && m.SequenceNumber == childLine.Line.SequenceNumber).Max(m => m.ChildNumber) + 1;
            //var childNumber = (await lineService.GetAll())
            //            .Where(m => m.LocationId == childLine.Line.LocationId
            //                        && m.CommodityId == childLine.Line.CommodityId
            //                        && m.SequenceNumber == childLine.Line.SequenceNumber)
            //            .Max(m => m.ChildNumber) + 1;
            var childNumber = await lineService.GetNextChildNumber(
                                                childLine.Line.LocationId,
                                                childLine.Line.CommodityId,
                                                childLine.Line.SequenceNumber);
            var line = new Line()
            {
                Id = Guid.NewGuid(),
                LocationId = childLine.Line.LocationId,
                CommodityId = childLine.Line.CommodityId,
                SequenceNumber = childLine.Line.SequenceNumber,
                ModularId = childLine.Line.ModularId,
                ChildNumber = childNumber,
                CreatedOn = DateTime.Now,
                CreatedBy = userName,
                ModifiedBy = userName,
                ModifiedOn = DateTime.Now
            };

            await lineService.Add(line);

            // Duplicate LineRevision (child)
            var oldLineId = childLine.Id;
            //childLine.Id = Guid.NewGuid();
            //childLine.LineId = line.Id;
            //childLine.LineStatusId = lineStatusId;

            ////child line should have its own modified and created info
            //childLine.CreatedBy = userName;
            //childLine.CreatedOn = DateTime.Now;
            //childLine.ModifiedBy = userName;
            //childLine.ModifiedOn = DateTime.Now;
            //childLine.Revision = revision;
            //childLine.SizeNpsPipeId = null;

            var newChildLine = new LineRevision
            {
                Id = Guid.NewGuid(),
                LineId = line.Id,
                LineStatusId = lineStatusId,
                CreatedBy = userName,
                CreatedOn = DateTime.Now,
                ModifiedBy = userName,
                ModifiedOn = DateTime.Now,
                Revision = revision,
                SizeNpsPipeId = null,
                LineListRevisionId = childLine.LineListRevisionId,
                LineNumber = childLine.LineNumber ?? string.Empty,
                IsJacketed = childLine.IsJacketed,
                RevisionSort = childLine.RevisionSort
            };
            var segments = await lineRevisionSegmentService.GetSegmentsByLineRevisionId(oldLineId);

            // Create a list to hold the updated segments to batch insert.            
            foreach (var segment in segments)
            {
                //db.Detach(segment);
                segment.Id = Guid.NewGuid();
                segment.LineRevisionId = newChildLine.Id;
                await lineRevisionSegmentService.AddWithoutSave(segment);
            }

            var opModes = await lineRevisionOperatingModeService.GetOperatingModesByLineRevisionId(oldLineId);
            foreach (var opMode in opModes)
            {
                //db.Detach(opMode);
                opMode.Id = Guid.NewGuid();
                opMode.LineRevisionId = newChildLine.Id;

                //don't copy the notes if they are "deleted", since that means the parent line is deleted.
                if ((opMode.Notes ?? string.Empty).Trim() == "DELETED")
                    opMode.Notes = string.Empty;

                await lineRevisionOperatingModeService.AddWithoutSave(opMode);
            }

            await lineRevisionService.AddWithoutSave(newChildLine);
            await lineRevisionService.SaveChanges();

           //db.SaveChanges();

            //childLine = lineRevisionService.GetAll().Result.Single(m => m.Id == childLine.Id);
           childLine = await lineRevisionService.GetById(newChildLine.Id);
            childLine.ValidationState = (int)LineRevisionValidation.GetHardRevisionValidationState(childLine);

            return childLine;
        }

        public static async Task ClearLineAttributes(LineRevision lineRevision,
            ILineRevisionOperatingModeService lineRevisionOperatingModes,
            ILineRevisionSegmentService lineRevisionSegmentService
            )
        {
            lineRevision.AreaId = null;
            lineRevision.CheckedOutBy = "";
            lineRevision.CheckedOutOn = null;
            lineRevision.CorrosionAllowanceAnnulusId = null;
            lineRevision.CorrosionAllowancePipeId = null;
            lineRevision.DesignPressureAnnulus = "";
            lineRevision.DesignPressurePipe = "";
            lineRevision.DesignTemperatureMaximumAnnulus = "";
            lineRevision.DesignTemperatureMaximumPipe = "";
            lineRevision.DesignTemperatureMinimumAnnulus = "";
            lineRevision.DesignTemperatureMinimumPipe = "";
            lineRevision.ExpansionTemperature = "";
            lineRevision.InternalCoatingLinerId = null;
            lineRevision.IsCheckedOut = false;
            lineRevision.MinimumDesignMetalTemperature = "";
            lineRevision.NdeCategoryAnnulusId = null;
            lineRevision.NdeCategoryPipeId = null;
            lineRevision.OriginatingPID = "";
            lineRevision.OtherInspectionId = null;
            lineRevision.PipeSpecificationId = null;
            lineRevision.PostWeldHeatTreatmentId = null;
            //lineRevision.PressureProtectionId = null; //SER 435.  Changes by Armando Chaves.
            lineRevision.ScheduleAnnulusId = null;
            lineRevision.SchedulePipeId = null;
            lineRevision.SizeNpsAnnulusId = null;
            lineRevision.SizeNpsPipeId = null;
            lineRevision.StressAnalysisId = null;
            lineRevision.TestMediumAnnulusId = null;
            lineRevision.TestMediumPipeId = null;
            lineRevision.TestPressureAnnulus = "";
            lineRevision.TestPressurePipe = "";
            lineRevision.UpsetPressureAnnulus = "";
            lineRevision.UpsetPressurePipe = "";
            lineRevision.UpsetTemperatureAnnulus = "";
            lineRevision.UpsetTemperaturePipe = "";
            lineRevision.WallThicknessAnnulus = null;
            lineRevision.WallThicknessPipe = null;
            lineRevision.XrayAnnulusId = null;
            lineRevision.XrayPipeId = null;

            var segments = await lineRevisionSegmentService.GetSegmentsByLineRevisionId(lineRevision.Id);
            foreach (var segment in segments.Where(m => m.SegmentNumber != "1"))
                await lineRevisionSegmentService.Remove(segment);

            var opModes = await lineRevisionOperatingModes.GetOperatingModesByLineRevisionId(lineRevision.Id);
            foreach (var opMode in opModes.Where(m => !new[] { "1", "", null }.Contains(m.OperatingModeNumber)))
                await lineRevisionOperatingModes.Remove(opMode);

            var firstSegment = await lineRevisionSegmentService.GetFirstSegment(lineRevision.Id);
            if (firstSegment != null)
            {
                firstSegment.InsulationMaterialId = null;
                firstSegment.InsulationThicknessId = null;
                firstSegment.InsulationTypeId = null;
                firstSegment.PaintSystemId = null;
                firstSegment.SegmentTypeId = null;
                firstSegment.TracingDesignHoldTemperature = string.Empty;
                firstSegment.TracingDesignNumberOfTracersId = null;
                firstSegment.TracingTypeId = null;
            }

            var primaryOpMode = await lineRevisionOperatingModes.GetPrimaryOperatingMode(lineRevision.Id);
            if (primaryOpMode != null)
            {
                primaryOpMode.CodeId = null;
                primaryOpMode.CsaClassLocationId = null;
                primaryOpMode.CsaHvpLvpId = null;
                primaryOpMode.FluidId = null;
                primaryOpMode.FluidPhaseId = null;
                primaryOpMode.HoopStressLevel = null;
                primaryOpMode.IsAbsaRegistration = string.Empty; //SER 435.  Changes by Armando Chaves.
                primaryOpMode.IsSourService = false;
                primaryOpMode.LineRoutingFrom = string.Empty;
                primaryOpMode.LineRoutingTo = string.Empty;
                primaryOpMode.Notes = string.Empty;
                primaryOpMode.OperatingModeId = null;
                primaryOpMode.OperatingModeNumber = "1";
                primaryOpMode.OperatingPressureAnnulus = string.Empty;
                primaryOpMode.OperatingPressurePipe = string.Empty;
                primaryOpMode.OperatingTemperatureAnnulus = string.Empty;
                primaryOpMode.OperatingTemperaturePipe = string.Empty;
                primaryOpMode.PipeMaterialSpecification = string.Empty;
                primaryOpMode.PressureProtectionId = null; //SER 435.  Changes by Armando Chaves.

                //primaryOpMode.ModularId  = null;
            }
        }

        public static void MoveLines(List<LineRevision> lines, LineListRevision toLineList, string userName,
            ILineRevisionOperatingModeService lineRevisionOperatingModes,
            ILineRevisionSegmentService lineRevisionSegmentService)
        {
            foreach (var lineToMove in lines)
            {
                //LM4: If the LLR is reserved or Cancelled, clear the attributes on the LineRevision
                if (toLineList.LineListStatus.Name.ToUpper() == "RESERVED")
                {
                    ClearLineAttributes(lineToMove, lineRevisionOperatingModes, lineRevisionSegmentService);
                }
                else if (toLineList.LineListStatus.Name.ToUpper().Contains("CANCELLED"))
                {
                    ClearLineAttributes(lineToMove, lineRevisionOperatingModes, lineRevisionSegmentService);
                    lineToMove.LineRevisionOperatingModes.First().Notes = "DELETED"; // enhancement 143;
                    lineToMove.ModifiedBy = userName;
                    lineToMove.ModifiedOn = DateTime.Now;
                }

                //change the current line's LineListRevision to the selected one...
                if (lineToMove != null && toLineList != null)
                {
                    lineToMove.ModifiedBy = userName;
                    lineToMove.ModifiedOn = DateTime.Now;
                    lineToMove.IsActive = true;
                    lineToMove.LineListRevisionId = toLineList.Id;

                    //Enh 416 - If moving a reserved (or deleted) line to another list, don't update the status (keep it reserved/deleted)
                    if (!lineToMove.LineStatus.IsDefaultForReservation && lineToMove.LineStatus.Name != "DELETED")
                        lineToMove.LineStatusId = toLineList.LineListStatus.CorrespondingLineStatusId;

                    lineToMove.Revision = toLineList.DocumentRevision;
                }

                lineToMove.ValidationState = (int)LineRevisionValidation.GetHardRevisionValidationState(lineToMove);
            }

            //db.SaveChanges();
        }

        //public static async Task IncludeExistingLines(List<Guid> existingLineIds,
        //    Guid toLineListRevisionId, string userName, bool isReferenceLine,
        //    ILineRevisionService lineRevisionService,
        //    ILineListRevisionService lineListRevisionService,
        //    ILineStatusService lineStatusService,
        //    ILineListStatusService lineListStatusService,
        //    ILineRevisionSegmentService lineRevisionSegmentService,
        //    ILineRevisionOperatingModeService lineRevisionOperatingModeService
        //    )
        //{
        //    List<Guid> newIds = new List<Guid>();
        //    bool isCancelledDraft = false;

        //    var rev = await lineListRevisionService.GetById(toLineListRevisionId);
        //    var deletedId = await lineStatusService.GetDeletedStatusId();
        //    var cancelledDraftId = await lineListStatusService.GetCancelledDraftId();
        //    var existingLines = await lineRevisionService.GetByIds(existingLineIds);

        //    isCancelledDraft = (rev.LineListStatusId == cancelledDraftId);

        //    foreach (LineRevision selectedLine in existingLines)
        //    {
        //        var linerev = await lineRevisionService.ExistLineRevisions(
        //                                                        rev.Id,
        //                                                        selectedLine.Line.LocationId,
        //                                                        selectedLine.Line.CommodityId,
        //                                                        selectedLine.Line.SequenceNumber,
        //                                                        selectedLine.Line.ChildNumber);
        //        if (!linerev)
        //        {
        //            //db.Detach(selectedLine);
        //            var oldLineId = selectedLine.Id;
        //            selectedLine.Id = Guid.NewGuid();
        //            selectedLine.LineListRevisionId = rev.Id;
        //            selectedLine.Revision = rev.DocumentRevision;
        //            selectedLine.IsActive = true;
        //            if (selectedLine.LineStatusId != deletedId && !isReferenceLine)
        //                selectedLine.LineStatusId = rev.LineListStatus.CorrespondingLineStatusId;
        //            selectedLine.CheckedOutBy = string.Empty;
        //            selectedLine.IsCheckedOut = false;
        //            selectedLine.CheckedOutOn = null;

        //            if (isReferenceLine)
        //            {
        //                selectedLine.IsReferenceLine = true;
        //                selectedLine.IsActive = false; //(the design spec does not necessarily indicate if active should be set for reference lines, but it does indicate it should be 'read-only')
        //            }
        //            //await lineRevisionService.Add(selectedLine);
        //            //await lineRevisionService.SaveChanges();

        //            var segments = await lineRevisionSegmentService.GetSegmentsByLineRevisionId(oldLineId);

        //            foreach (var segment in segments)
        //            {
        //                //db.Detach(segment);
        //                segment.Id = Guid.NewGuid();
        //                segment.LineRevisionId = selectedLine.Id;
        //                await lineRevisionSegmentService.AddWithoutSave(segment);
        //            }

        //            var opModes = await lineRevisionOperatingModeService.GetOperatingModesByLineRevisionId(oldLineId);
        //            foreach (var opMode in opModes)
        //            {
        //                //db.Detach(opMode);
        //                opMode.Id = Guid.NewGuid();
        //                opMode.LineRevisionId = selectedLine.Id;
        //                await lineRevisionOperatingModeService.AddWithoutSave(opMode);
        //            }

        //            newIds.Add(selectedLine.Id);
        //            await lineRevisionService.AddWithoutSave(selectedLine);

        //        }
        //    }
        //    await lineRevisionService.SaveChanges();

        //    if (!isReferenceLine)
        //    {
        //        var lines = await lineRevisionService.GetByIds(newIds);
        //        foreach (var line in lines)
        //        {
        //            if (isCancelledDraft)
        //            {
        //                await ClearLineAttributes(line, lineRevisionOperatingModeService, lineRevisionSegmentService);
        //                line.LineRevisionOperatingModes.First().Notes = "DELETED"; // enhancement 143;
        //                line.ModifiedBy = userName;
        //                line.ModifiedOn = DateTime.Now;
        //            }
        //            line.ValidationState = (int)LineRevisionValidation.GetHardRevisionValidationState(line);
        //        }
        //    }
        //}

        public static async Task IncludeExistingLines(List<Guid> existingLineIds,
            Guid toLineListRevisionId, string userName, bool isReferenceLine,
            ILineRevisionService lineRevisionService,
            ILineListRevisionService lineListRevisionService,
            ILineStatusService lineStatusService,
            ILineListStatusService lineListStatusService,
            ILineRevisionSegmentService lineRevisionSegmentService,
            ILineRevisionOperatingModeService lineRevisionOperatingModeService,
            ILineService lineService 
            )
        {
            List<Guid> newIds = new();
            bool isCancelledDraft = false;

            var rev = await lineListRevisionService.GetById(toLineListRevisionId);
            var deletedId = await lineStatusService.GetDeletedStatusId();
            var cancelledDraftId = await lineListStatusService.GetCancelledDraftId();
            var existingLines = await lineRevisionService.GetByIds(existingLineIds); // includes Line

            isCancelledDraft = (rev.LineListStatusId == cancelledDraftId);

            foreach (LineRevision selectedLine in existingLines)
            {
                var linerev = await lineRevisionService.ExistLineRevisions(
                    rev.Id,
                    selectedLine.Line.LocationId,
                    selectedLine.Line.CommodityId,
                    selectedLine.Line.SequenceNumber,
                    selectedLine.Line.ChildNumber);

                if (!linerev)
                {
                    var oldLineId = selectedLine.Id;

                    var clonedLine = new LineRevision
                    {
                        Id = Guid.NewGuid(),
                        LineListRevisionId = rev.Id,
                        Revision = rev.DocumentRevision,
                        IsActive = true,
                        IsReferenceLine = isReferenceLine,
                        LineStatusId = (!isReferenceLine && selectedLine.LineStatusId != deletedId)
                                            ? rev.LineListStatus.CorrespondingLineStatusId
                                            : selectedLine.LineStatusId,
                        CheckedOutBy = string.Empty,
                        IsCheckedOut = false,
                        CheckedOutOn = null,
                        LineId = selectedLine.LineId,
                        SpecificationId = selectedLine.SpecificationId,
                        PipeSpecificationId = selectedLine.PipeSpecificationId,
                        SizeNpsPipeId = selectedLine.SizeNpsPipeId,
                        AreaId = selectedLine.AreaId,
                        CreatedBy = userName,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedBy = userName,
                        ModifiedOn = DateTime.UtcNow,
                        LineNumber = selectedLine.LineNumber ?? string.Empty,
                        IsJacketed = selectedLine.IsJacketed,
                        RevisionSort = selectedLine.RevisionSort
                    };
                    if (isReferenceLine)
                    {
                        clonedLine.IsReferenceLine = true;
                        clonedLine.IsActive = false; //(the design spec does not necessarily indicate if active should be set for reference lines, but it does indicate it should be 'read-only')
                    }

                    var segments = await lineRevisionSegmentService.GetSegmentsByLineRevisionId(oldLineId);
                    foreach (var segment in segments)
                    {
                        segment.Id = Guid.NewGuid();
                        segment.LineRevisionId = clonedLine.Id;
                        await lineRevisionSegmentService.AddWithoutSave(segment);
                    }

                    var opModes = await lineRevisionOperatingModeService.GetOperatingModesByLineRevisionId(oldLineId);
                    foreach (var opMode in opModes)
                    {
                        opMode.Id = Guid.NewGuid();
                        opMode.LineRevisionId = clonedLine.Id;
                        await lineRevisionOperatingModeService.AddWithoutSave(opMode);
                    }

                    newIds.Add(clonedLine.Id);
                    await lineRevisionService.AddWithoutSave(clonedLine);
                }
            }

            await lineRevisionService.SaveChanges();

            if (!isReferenceLine)
            {
                var lines = await lineRevisionService.GetByIds(newIds); // Includes Line
                foreach (var line in lines)
                {
                    if (line.Line == null)
                        line.Line = await lineService.GetById(line.LineId);

                    if (isCancelledDraft)
                    {
                        await ClearLineAttributes(line, lineRevisionOperatingModeService, lineRevisionSegmentService);
                        line.LineRevisionOperatingModes.First().Notes = "DELETED";
                        line.ModifiedBy = userName;
                        line.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
                    }

                    line.ValidationState = (int)LineRevisionValidation.GetHardRevisionValidationState(line);
                }

                await lineRevisionService.SaveChanges();
            }
        }
        public static IList<LineRevision> SetDefaults(IList<LineRevision> lines, bool scheduleDefaults,
            bool insulationDefaults, bool pipSpecDefaults,
            IPipeSpecificationService pipeSpecificationService,
             IEpProjectInsulationDefaultService epProjectInsulationDefaultService,
            IEpProjectInsulationDefaultDetailService epProjectInsulationDefaultDetailService,
            ITracingTypeService tracingTypeService,
            IScheduleDefaultService scheduleDefaultService
            )
        {
            //only TR lines
            foreach (var line in lines.Where(m => m.Specification != null && m.Specification.Name == "TR"))
            {
                if (scheduleDefaults)
                    SetScheduleDefaults(line, scheduleDefaultService);
                if (insulationDefaults)
                    SetInsulationDefaults(line, epProjectInsulationDefaultService, epProjectInsulationDefaultDetailService, tracingTypeService);
                if (pipSpecDefaults)
                    SetPipeSpecDefaults(line, pipeSpecificationService);
            }
            return lines;
        }

        private static void SetPipeSpecDefaults(LineRevision line, IPipeSpecificationService pipeSpecificationService)
        {
            var items = from p in pipeSpecificationService.GetAll().Result
                        where p.Id == line.PipeSpecificationId
                        select p;

            if (items.Any())
            {
                var item = items.First();
                if (!line.CorrosionAllowancePipeId.HasValue)
                {
                    line.CorrosionAllowancePipeId = item.CorrosionAllowanceId;
                    line.CorrosionAllowancePipe = item.CorrosionAllowance;
                }
                if (!line.XrayPipeId.HasValue)
                {
                    line.XrayPipeId = item.XrayId;
                    line.XrayPipe = item.Xray;
                }
                if (!line.NdeCategoryPipeId.HasValue)
                {
                    line.NdeCategoryPipeId = item.NdeCategoryId;
                    line.NdeCategoryPipe = item.NdeCategory;
                }
                if (!line.PostWeldHeatTreatmentId.HasValue)
                {
                    line.PostWeldHeatTreatmentId = item.PostWeldHeatTreatmentId;
                    line.PostWeldHeatTreatment = item.PostWeldHeatTreatment;
                }
            }
        }

        private static void SetInsulationDefaults(LineRevision line,
            IEpProjectInsulationDefaultService epProjectInsulationDefaultService,
            IEpProjectInsulationDefaultDetailService epProjectInsulationDefaultDetailService,
            ITracingTypeService tracingTypeService
            )
        {
            var opModes = line.LineRevisionOperatingModes.Where(m => m.OperatingModeNumber == "1");
            if (opModes.Any())
            {
                var opMode = opModes.First();

                foreach (var segment in line.LineRevisionSegments)
                {
                    int temp = 0;
                    if (int.TryParse(opMode.OperatingTemperaturePipe, out temp))
                    {
                        var table = epProjectInsulationDefaultService.GetAll().Result.FirstOrDefault(m => m.EpProjectId == line.LineListRevision.EpProjectId
                            && m.InsulationTypeId == segment.InsulationTypeId
                            && m.TracingTypeId == segment.TracingTypeId
                            && m.InsulationMaterialId == segment.InsulationMaterialId
                            );
                        if (table != null)
                        {
                            var rows = table.Rows.Where(m => m.SizeNpsId == line.SizeNpsPipeId).Select(m => m.Id);
                            var cols = table.Columns.Where(m => (m.MinOperatingTemperature ?? -999) < temp && m.MaxOperatingTemperature > temp).Select(m => m.Id);
                            var dtls = epProjectInsulationDefaultDetailService.GetAll().Result.Where(m => rows.Contains(m.EpProjectInsulationDefaultRowId) && cols.Contains(m.EpProjectInsulationDefaultColumnId));

                            if (dtls.Any())
                            {
                                var dtl = dtls.First();

                                if (!segment.InsulationThicknessId.HasValue)
                                {
                                    segment.InsulationThicknessId = dtl.InsulationThicknessId;
                                    segment.InsulationThickness = dtl.InsulationThickness;
                                }

                                if (!segment.TracingDesignNumberOfTracersId.HasValue)
                                {
                                    segment.TracingDesignNumberOfTracersId = dtl.TracingDesignNumberOfTracersId;
                                    segment.TracingDesignNumberOfTracers = dtl.TracingDesignNumberOfTracers;
                                }

                                var tt = tracingTypeService.GetAll().Result.Where(m => m.Id == segment.TracingTypeId);
                                if (tt.Any() && string.IsNullOrWhiteSpace(segment.TracingDesignHoldTemperature))
                                    segment.TracingDesignHoldTemperature = tt.First().Temperature ?? string.Empty;
                            }
                        }
                    }
                }
            }
        }

        private static void SetScheduleDefaults(LineRevision line, IScheduleDefaultService scheduleDefaultService)
        {
            if (!line.SchedulePipeId.HasValue)
            {
                var schedules = scheduleDefaultService.GetAll().Result.Where(m => m.PipeSpecificationId == line.PipeSpecificationId && m.SizeNpsId == line.SizeNpsPipeId);
                if (schedules.Any())
                {
                    line.SchedulePipeId = schedules.First().ScheduleId;
                    line.SchedulePipe = schedules.First().Schedule;
                }
            }
        }

        public static void UpdateRevisions(Guid lineListRevisionId, ILineRevisionService lineRevisionService)
        {
            foreach (var item in lineRevisionService.GetAll().Result.Where(m => m.LineListRevisionId == lineListRevisionId))
            {
                var last = GetLastIssuedLineRevision(item.Line.LocationId, item.Line.CommodityId, item.Line.SequenceNumber, item.Line.ChildNumber, item.LineListRevision.LineListId, lineRevisionService);
                if (last == null)
                    item.Revision = item.LineListRevision.DocumentRevision;
                else if (HasChanged(item, last))
                    item.Revision = item.LineListRevision.DocumentRevision;
                else
                    item.Revision = last.Revision;
            }
        }
    }
}