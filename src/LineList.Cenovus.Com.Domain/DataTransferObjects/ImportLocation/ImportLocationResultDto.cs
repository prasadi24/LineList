namespace LineList.Cenovus.Com.API.DataTransferObjects.ImportLocation
{
    public class ImportLocationResultDto
    {
        public Guid Id { get; set; }

        public Guid ImportId { get; set; }

        public Guid LocationId { get; set; }

        public int BeforeCount { get; set; }

        public int AfterCount { get; set; }

        public LocationResultDto Location { get; set; }
    }
}