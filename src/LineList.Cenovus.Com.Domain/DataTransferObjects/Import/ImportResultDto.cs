using LineList.Cenovus.Com.API.DataTransferObjects.ImportCommodity;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportFacility;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportLocation;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportSheet;

namespace LineList.Cenovus.Com.API.DataTransferObjects.Import
{
    public class ImportResultDto
    {
        public Guid Id { get; set; }

        public string OriginalFileName { get; set; }

        public string FacilityName { get; set; }

        public string Status { get; set; }

        public string Path { get; set; }

        public int? ValidationCount { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Added navigation properties for related collections
        public List<ImportSheetResultDto> ImportSheets { get; set; }
        public List<ImportCommodityResultDto> ImportCommodities { get; set; }
        public List<ImportFacilityResultDto> ImportFacilities { get; set; }
        public List<ImportLocationResultDto> ImportLocations { get; set; }
    }
}