namespace LineList.Cenovus.Com.API.DataTransferObjects.NotesConfiguration
{
    public class NotesConfigurationResultDto
    {
        public Guid Id { get; set; }

        public Guid FacilityId { get; set; }

        public Guid SpecificationId { get; set; }

        public string FacilityName { get; set; }
        public string SpecificationName { get; set; }

        public string FileName { get; set; }

        public byte[]? FileData { get; set; }

        public string UploadedBy { get; set; }

        public DateTime UploadedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
