using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.RulesEngine
{
    public class LineDesignationReport
    {
        public LineDesignationReport(Guid lineListRevisionId, bool showAllFields)
        {
            var db = new LineListDbContext();
            this.LineListRevisionId = LineListRevisionId;
            this.Revision = db.LineListRevisions.Single(m => m.Id == lineListRevisionId).DocumentRevision;
            this.Header = db.LineDesignationTableViewHeaders.Single(m => m.Id == lineListRevisionId);

            var db2 = new LineListDbContext();
           // db2.Configuration.LazyLoadingEnabled = false;

            var results = from m in db2.LineRevisions
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
                          where m.LineListRevisionId == lineListRevisionId
                            && m.IsReferenceLine == false
                          select m;
            var input = results.ToList();
            this.Lines = FlatFactory.ToFlatLines(input).ToList(); //.OrderBy(m => m.Location).ThenBy(m => m.Commodity).ThenBy(m => m.SequenceNumber).ThenBy(m => m.ChildNumber).ThenBy(m => m.AltOpMode).ToList();
            int maxNumberOfRevisions = 6;
            var list = db.LineDesignationTableViewRevisions.Where(m => m.LineListId == this.Header.LineListId).OrderBy(m => m.DocumentRevisionSort).ToList();
            list = list.Skip(Math.Max(0, list.Count() - maxNumberOfRevisions)).Take(maxNumberOfRevisions).ToList();
            if (list.Count() < 6)
                for (int i = list.Count(); i < 6; i++)
                    list.Add(new LineDesignationTableViewRevision() { DocumentRevisionSort = 99 });
            this.Revisions = list;
            this.ShowAllFields = showAllFields;
        }

        public LineDesignationTableViewHeader Header { get; set; }
        public bool IsDraft { get; }
        public Guid LineListRevisionId { get; set; }
        public List<FlatLine> Lines { get; set; }
        [NotMapped]
        public string Mode { get; set; }
        public string Revision { get; set; }
        public List<LineDesignationTableViewRevision> Revisions { get; set; }
        public bool ShowAllFields { get; set; }
        public bool ShowCoverpage { get; }
        public bool ShowDraftWatermark { get; }
        public bool ShowPrintedDate { get; }
        public string[] SpecsInReport { get; }
    }
}
