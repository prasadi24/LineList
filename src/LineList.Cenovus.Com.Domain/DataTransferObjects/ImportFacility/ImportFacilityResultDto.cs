namespace LineList.Cenovus.Com.API.DataTransferObjects.ImportFacility
{
    public class ImportFacilityResultDto
    {
        public Guid Id { get; set; }

        public Guid ImportId { get; set; }

        public Guid FacilityId { get; set; }

        public int BeforeCount { get; set; }

        public int AfterCount { get; set; }

        public FacilityResultDto Facility { get; set; }
    }
}