using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.RulesEngine
{
    public static class LineNumberGenerator
    {
        //Line Numbers are of the form ABC-DE-FGH-12-3456-78I-JKL, e.g. CL1-SO-AC-16-0301-2½H-GT2. The following Line Revision attributes (and a “-“) are concatenated together to create the full Line Number:

        //Form String	Attribute	Example
        //ABC	Location Identifier	CL1
        //DE	Commodity Code	SO
        //FGH	Pipe Specification Code or Class Service Material Code	AC
        //12	Size NPS	16
        //3456	Line Sequence Number	0301
        //78	Insulation Thickness	2½
        //I	Insulation Type Code	H
        //JKL	Tracing Type Code	GT2

        //Business Rules

        //The following business rules are applied to create the full Line Number:
        //•	Line Number to be automatically assigned when a new Line is reserved
        //•	Line Number to be automatically updated (changed) when corresponding attribute values are updated, e.g. Size NPS
        //•	Line List Revision attributes (and a “-“) are concatenated together to create the full Line Number
        //•	Location Identifier is concatenated for all 3 Specifications, i.e. TR, CL and FC Spec
        //o	For FC Spec, Location Identifier FCX was intentionally created, in place of a blank originally used for FC1 Location Identifier, so that every Line Number would have a Location Identifier
        //o	Lookup Tables for Area and Location has been rationalized to resolve Areas that were originally used for FC Spec Offsite Locations
        //•	Values for some attributes may be blank or “None”, depending on the Line Revision Status
        //o	If Pipe Specification Code (for CL Spec) and Class Service Material Code are both blank, then substitute one asterisk “*”
        //o	If Size NPS is blank, then substitute one asterisk “*”
        //o	If Insulation Thickness and Insulation Type Code is “None”, then do not concatenate
        //o	If Tracing Type Code is blank, then do not concatenate
        //o	Size NPS and Insulation Thickness to concatenate the fractional representation of attribute value, e.g. 2½ not 2.5

        public static string Evaluate(LineRevision line, ILocationService locationService,
            ICommodityService commodityService,
            IPipeSpecificationService pipeSpecificationService,
            ISizeNpsService sizeNpsService,
            IInsulationThicknessService insulationThicknessService,
            ITracingTypeService tracingTypeService,
            IInsulationTypeService insulationTypeService
            )
        {
            var value = string.Empty;
            const string asterisk = "*";
            const string seperator = "-";
            const string NONE = "NONE";

            if (line.Line.Location != null)
                value += line.Line.Location.Name + seperator;
            else
                value += locationService.GetAll().Result.Single(m => m.Id == line.Line.LocationId).Name + seperator;

            if (line.Line.Commodity != null)
                value += line.Line.Commodity.Name + seperator;
            else
                value += commodityService.GetAll().Result.Single(m => m.Id == line.Line.CommodityId).Name + seperator;

            if (line.PipeSpecification != null)
                value += line.PipeSpecification.Name + seperator;
            else if (line.PipeSpecificationId.HasValue)
            {
                var id = line.PipeSpecificationId;
                var pipeSpec = pipeSpecificationService.GetAll().Result.Where(m => m.Id == id && m.Name.ToUpper() != NONE);
                if (pipeSpec.Any())
                    value += pipeSpec.First().Name + seperator;
                else
                    value += asterisk + seperator;
            }
            else
                value += asterisk + seperator;

            if (line.SizeNpsPipe != null && line.SizeNpsPipe.Name.ToUpper() != NONE)
                value += line.SizeNpsPipe.Name + seperator;
            else if (line.SizeNpsPipeId.HasValue)
            {
                var id = line.SizeNpsPipeId;
                var sizeNps = sizeNpsService.GetAll().Result.Where(m => m.Id == id && m.Name.ToUpper() != NONE);
                if (sizeNps.Any())
                    value += sizeNps.First().Name + seperator;
                else
                    value += asterisk + seperator;
            }
            else
                value += asterisk + seperator;

            value += line.Line.SequenceNumber + seperator;

            if (line.LineRevisionSegments != null && line.LineRevisionSegments.Any())
            {
                var insulationThickness = string.Empty;
                var insulationType = string.Empty;

                if (line.LineRevisionSegments.First().InsulationThickness != null)
                {
                    if (line.LineRevisionSegments.First().InsulationThickness.Name.ToUpperInvariant() != NONE)
                        value += line.LineRevisionSegments.First().InsulationThickness.Name;
                }
                else if (line.LineRevisionSegments.First().InsulationThicknessId.HasValue)
                {
                    var id = line.LineRevisionSegments.First().InsulationThicknessId;
                    var thk = insulationThicknessService.GetAll().Result.Where(m => m.Id == id && m.Name.ToUpper() != NONE);
                    if (thk.Any())
                        insulationThickness = thk.First().Name;
                }

                if (line.LineRevisionSegments.First().InsulationType != null)
                {
                    if (line.LineRevisionSegments.First().InsulationType.Name.ToUpperInvariant() != NONE)
                        insulationType = line.LineRevisionSegments.First().InsulationType.Name;
                }
                else if (line.LineRevisionSegments.First().InsulationTypeId.HasValue)
                {
                    var id = line.LineRevisionSegments.First().InsulationTypeId;
                    var thk = insulationTypeService.GetAll().Result.Where(m => m.Id == id && m.Name.ToUpper() != NONE);
                    if (thk.Any())
                        insulationType = thk.First().Name;
                }

                if (insulationThickness.Length > 0 || insulationType.Length > 0)
                    value += (insulationThickness + insulationType + seperator);

                if (line.LineRevisionSegments.First().TracingType != null)
                {
                    if (line.LineRevisionSegments.First().TracingType.Name.ToUpperInvariant() != NONE)
                        value += line.LineRevisionSegments.First().TracingType.Name + seperator;
                }
                else if (line.LineRevisionSegments.First().TracingTypeId.HasValue)
                {
                    var id = line.LineRevisionSegments.First().TracingTypeId;
                    var tracingType = tracingTypeService.GetAll().Result.Where(m => m.Id == id && m.Name.ToUpper() != NONE);
                    if (tracingType.Any())
                        value += tracingType.First().Name + seperator;
                }
            }

            if (value.Substring(value.Length - 1, 1) == "-")
                value = value.Substring(0, value.Length - 1);

            return value;
        }
    }
}