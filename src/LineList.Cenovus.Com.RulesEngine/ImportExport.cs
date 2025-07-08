using LineList.Cenovus.Com.Domain.Models;
using Newtonsoft.Json;
using System.Data;

namespace LineList.Cenovus.Com.RulesEngine
{
    public class ImportExport
    {
        private FlatLineList lineLists = null;

        //var requestText = new StreamReader(context.Request.InputStream).ReadToEnd();
        //var requestJson = JsonConvert.DeserializeObject<RequestJson>(requestText);

        public FlatLineList GetLineListRevision(Guid lineListRevisionId)
        {
            var db = new LLDatabase();
            db.Configuration.LazyLoadingEnabled = false;

            var results = db.LineListRevisions
                .Include("LineList")
                .Include("EpProject.EpCompany")
                .Include("Specification")
                .Include("LineListStatus")
                .Single(m => m.Id == lineListRevisionId);
            var list = FlatFactory.ToFlatLineList(results);
            return list;
        }

        public FlatLine[] GetLines(Guid lineListRevisionId, string checkedOutBy)
        {
            var db = new LLDatabase();
            db.Configuration.AutoDetectChangesEnabled = false;
            //Entity Framework doesn't always handle lazy-loading properly, so here's some explicit includes
            //more info: http://weblogs.asp.net/dotnetstories/archive/2011/03/10/lazy-loading-eager-loading-explicit-loading-in-entity-framework-4.aspx
            db.Configuration.LazyLoadingEnabled = false;

            var results = from m in db.LineRevisions
                              .Include("LineStatus")
                              .Include("LineListRevision.LineList")
                              .Include("LineListRevision.EpProject.EpCompany")
                              .Include("LineListRevision.LineListStatus")
                              .Include("Line.Commodity")
                              .Include("Line.Location").Include("InternalCoatingLiner")
                              .Include("Specification").Include("PostWeldHeatTreatment")
                              .Include("Area").Include("LineStatus")
                              .Include("LineRevisionSegments.SegmentType")
                              .Include("LineRevisionOperatingModes.Code")
                              .Include("CorrosionAllowancePipe").Include("CorrosionAllowanceAnnulus")
                              .Include("XrayPipe").Include("XrayAnnulus")
                              .Include("NdeCategoryPipe").Include("NdeCategoryAnnulus")
                                .Include("LineRevisionOperatingModes.CsaHvpLvp")
                                .Include("LineRevisionOperatingModes.CsaClassLocation")
                                .Include("LineRevisionOperatingModes.Fluid")
                                .Include("LineRevisionOperatingModes.Code")
                                .Include("LineRevisionOperatingModes.FluidPhase")
                                .Include("LineRevisionOperatingModes.PressureProtection") //SER 435.  Changes by Armando Chaves.
                                .Include("StressAnalysis")
                                .Include("SchedulePipe")
                                .Include("ScheduleAnnulus")
                                .Include("SizeNpsPipe")
                                .Include("SizeNpsAnnulus")
                                .Include("PipeSpecification")
                                .Include("TestMediumPipe")
                                .Include("TestMediumAnnulus")
                                .Include("LineRevisionSegments.InsulationMaterial")
                                .Include("LineRevisionSegments.TracingDesignNumberOfTracers")
                                .Include("LineRevisionSegments.TracingType")
                                .Include("LineRevisionSegments.InsulationType")
                                .Include("LineRevisionSegments.InsulationThickness")
                                .Include("LineRevisionSegments.PaintSystem")
                                .Include("LineRevisionSegments.SegmentType")
                              // .Include("Line.Modular")
                          where m.LineListRevisionId == lineListRevisionId
                            && m.CheckedOutBy == checkedOutBy
                            && m.IsReferenceLine == false
                            && m.LineStatus.Name.ToUpper() != "DELETED"
                          select m;
            var input = results.ToList();
            // lineLists = JsonConvert.DeserializeObject<FlatLineList>(results);
            var list = FlatFactory.ToFlatLines(input).OrderBy(m => m.Location).ThenBy(m => m.Commodity).ThenBy(m => m.SequenceNumber).ThenBy(m => m.ChildNumber).ThenBy(m => m.AltOpMode);
            return list.ToArray();
        }

        public void Update(string jsonLines, string userName)
        {
            var lines = JsonConvert.DeserializeObject<FlatLine[]>(jsonLines);
            lines.ToList().ForEach(m => m.Source = FlatSourceEnum.Revision);
            var db = new LLDatabase();
            Validator.VerifyJacket(db, lines);
            //var jacketedTypes = db.TracingTypes.Where(m => m.IsJacketed == true).Select(m => "I/S:" + m.Name).ToArray();
            var jacketed = FlatFactory.ToLineRevisions(db, lines.Where(m => m.IsJacketed).ToList(), userName, true, false);
            var nonJacketed = FlatFactory.ToLineRevisions(db, lines.Where(m => !m.IsJacketed).ToList(), userName, false, false);
            var list = jacketed.Union(nonJacketed);
            var reservedStatusId = db.LineStatuses.Where(m => m.IsDefaultForReservation).First().Id;
            var changed = new List<Guid>();

            //close the context and renew it, otherwise some info might be cached.
            db.Dispose();
            db = null;
            db = new LLDatabase();

            foreach (var item in list)
            {
                //pass in a new data context, so it doesn't read any of the existing changes
                if (LineRules.HasChanged(db, item))
                {
                    var newItem = db.LineRevisions.Single(m => m.Id == item.Id);
                    CopyValues(newItem, item);
                    CopyCollection(db.LineRevisionSegments.Where(m => m.LineRevisionId == item.Id).ToList(), item.LineRevisionSegments.ToList(), db);
                    CopyCollection(db.LineRevisionOperatingModes.Where(m => m.LineRevisionId == item.Id).ToList(), item.LineRevisionOperatingModes.ToList(), db);
                    if (item.LineStatusId == reservedStatusId)
                        newItem.LineStatusId = item.LineListRevision.LineListStatus.CorrespondingLineStatusId;
                    newItem.IsJacketed = newItem.LineRevisionSegments.Where(m => m.TracingType != null && m.TracingType.IsJacketed).Any();
                    newItem.ModifiedOn = DateTime.Now;
                    newItem.ModifiedBy = userName;

                    changed.Add(newItem.Id);

                    db.SaveChanges();
                }
            }

            //close the context and renew it, otherwise some info might be cached.
            db.Dispose();
            db = null;
            db = new LLDatabase();

            //refresh all the dependant objects so it can be properly validated
            foreach (var newItem in db.LineRevisions.Where(m => changed.Contains(m.Id)))
            {
                newItem.LineNumber = LineNumberGenerator.Evaluate(db, newItem);
                newItem.ValidationState = (int)LineRevisionValidation.GetHardRevisionValidationState(newItem);
                newItem.Revision = LineRules.GetNextRevision(newItem);
            }
            db.SaveChanges();
        }

        private void CopyValues(LineRevision obj, LineRevision newValues)
        {
            obj.AreaId = newValues.AreaId;
            obj.SizeNpsAnnulusId = newValues.SizeNpsAnnulusId;
            obj.SizeNpsPipeId = newValues.SizeNpsPipeId;
            obj.OriginatingPID = newValues.OriginatingPID;
            obj.ScheduleAnnulusId = newValues.ScheduleAnnulusId;
            obj.SchedulePipeId = newValues.SchedulePipeId;
            obj.WallThicknessAnnulus = newValues.WallThicknessAnnulus;
            obj.WallThicknessPipe = newValues.WallThicknessPipe;
            obj.DesignPressureAnnulus = newValues.DesignPressureAnnulus;
            obj.DesignPressurePipe = newValues.DesignPressurePipe;
            obj.DesignTemperatureMaximumAnnulus = newValues.DesignTemperatureMaximumAnnulus;
            obj.DesignTemperatureMaximumPipe = newValues.DesignTemperatureMaximumPipe;
            obj.DesignTemperatureMinimumAnnulus = newValues.DesignTemperatureMinimumAnnulus;
            obj.DesignTemperatureMinimumPipe = newValues.DesignTemperatureMinimumPipe;
            obj.TestMediumAnnulusId = newValues.TestMediumAnnulusId;
            obj.TestMediumPipeId = newValues.TestMediumPipeId;
            obj.TestPressurePipe = newValues.TestPressurePipe;
            obj.TestPressureAnnulus = newValues.TestPressureAnnulus;
            obj.ExpansionTemperature = newValues.ExpansionTemperature;
            obj.UpsetPressureAnnulus = newValues.UpsetPressureAnnulus;
            obj.UpsetPressurePipe = newValues.UpsetPressurePipe;
            obj.UpsetTemperatureAnnulus = newValues.UpsetTemperatureAnnulus;
            obj.UpsetTemperaturePipe = newValues.UpsetTemperaturePipe;
            obj.MinimumDesignMetalTemperature = newValues.MinimumDesignMetalTemperature;
            obj.CorrosionAllowanceAnnulusId = newValues.CorrosionAllowanceAnnulusId;
            obj.CorrosionAllowancePipeId = newValues.CorrosionAllowancePipeId;
            obj.XrayAnnulusId = newValues.XrayAnnulusId;
            obj.XrayPipeId = newValues.XrayPipeId;
            obj.NdeCategoryAnnulusId = newValues.NdeCategoryAnnulusId;
            obj.NdeCategoryPipeId = newValues.NdeCategoryPipeId;
            obj.PostWeldHeatTreatmentId = newValues.PostWeldHeatTreatmentId;
            obj.StressAnalysisId = newValues.StressAnalysisId;
            obj.InternalCoatingLinerId = newValues.InternalCoatingLinerId;
            //obj.PressureProtectionId = newValues.PressureProtectionId;//SER 435.  Changes by Armando Chaves.
            obj.PipeSpecificationId = newValues.PipeSpecificationId;
            obj.LineStatusId = newValues.LineStatusId;
            obj.Line.ModularId = newValues.Line.ModularId;
            obj.ModularId = newValues.Line.ModularId;
        }

        private void CopyCollection(List<LineRevisionSegment> list, List<LineRevisionSegment> newValues, LLDatabase db)
        {
            foreach (LineRevisionSegment item in newValues)
            {
                var results = list.Where(m => m.Id == item.Id);
                var result = new LineRevisionSegment() { Id = Guid.NewGuid() };
                if (results.Any())
                    result = results.First();
                else
                    db.LineRevisionSegments.Add(result);
                result.CreatedBy = item.CreatedBy;
                result.CreatedOn = item.CreatedOn;
                result.InsulationMaterialId = item.InsulationMaterialId;
                result.InsulationThicknessId = item.InsulationThicknessId;
                result.InsulationTypeId = item.InsulationTypeId;
                result.LineRevisionId = item.LineRevisionId;
                result.ModifiedBy = item.ModifiedBy;
                result.ModifiedOn = item.ModifiedOn;
                result.PaintSystemId = item.PaintSystemId;
                result.SegmentNumber = item.SegmentNumber;
                result.SegmentTypeId = item.SegmentTypeId;
                result.TracingDesignHoldTemperature = item.TracingDesignHoldTemperature;
                result.TracingDesignNumberOfTracersId = item.TracingDesignNumberOfTracersId;
                result.TracingTypeId = item.TracingTypeId;
            }
            foreach (var item in list)
                if (!newValues.Where(m => m.Id == item.Id).Any())
                    db.LineRevisionSegments.Remove(item);
        }

        public List<FlatValidation> GetValidationErrors(Guid lineListRevisionId, string jsonLines)
        {
            var lines = JsonConvert.DeserializeObject<FlatLine[]>(jsonLines);
            lines.ToList().ForEach(m => m.Source = FlatSourceEnum.Revision);
            //6.4.19 LG5 - Lines with 'DELETED' status are not validated
            lines = lines.Where(m => m.LineStatus == null || (m.LineStatus != null && m.LineStatus.ToUpper() != "DELETED")).ToArray();
            var validator = new Validator(lineListRevisionId, true);
            var errors = validator.Validate(lines);
            var transitionalErrors = validator.AuditInactiveTransitions(lines, string.Empty);
            errors = errors.Union(transitionalErrors).ToList();
            return FlatFactory.ToFlatValidation(errors).ToList();
        }

        private void CopyCollection(List<LineRevisionOperatingMode> list, List<LineRevisionOperatingMode> newValues, LLDatabase db)
        {
            foreach (LineRevisionOperatingMode item in newValues)
            {
                var results = list.Where(m => m.Id == item.Id);
                var result = new LineRevisionOperatingMode() { Id = Guid.NewGuid() };
                if (results.Any())
                    result = results.First();
                result.CodeId = item.CodeId;
                result.CreatedBy = item.CreatedBy;
                result.CreatedOn = item.CreatedOn;
                result.CsaClassLocationId = item.CsaClassLocationId;
                result.CsaHvpLvpId = item.CsaHvpLvpId;
                result.FluidId = item.FluidId;
                result.FluidPhaseId = item.FluidPhaseId;
                result.HoopStressLevel = item.HoopStressLevel;
                result.IsAbsaRegistration = item.IsAbsaRegistration;
                result.PressureProtectionId = item.PressureProtectionId; //SER 435.  Changes by Armando Chaves.

                result.IsSourService = item.IsSourService;
                result.LineRevisionId = item.LineRevisionId;
                result.LineRoutingFrom = item.LineRoutingFrom;
                result.LineRoutingTo = item.LineRoutingTo;
                result.ModifiedBy = item.ModifiedBy;
                result.ModifiedOn = item.ModifiedOn;
                result.OperatingModeId = item.OperatingModeId;
                result.OperatingModeNumber = item.OperatingModeNumber;
                result.OperatingPressureAnnulus = item.OperatingPressureAnnulus;
                result.OperatingPressurePipe = item.OperatingPressurePipe;
                result.OperatingTemperatureAnnulus = item.OperatingTemperatureAnnulus;
                result.OperatingTemperaturePipe = item.OperatingTemperaturePipe;
                result.PipeMaterialSpecification = item.PipeMaterialSpecification;
                result.Notes = item.Notes;
                result.ModularId = item.ModularId;
            }
        }
    }
}