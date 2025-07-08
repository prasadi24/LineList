namespace LineList.Cenovus.Com.API.DataTransferObjects.LineRevisionOperatingMode
{
    public class LineRevisionOperatingModeResultDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid LineRevisionId { get; set; } // Foreign Key to LineRevision
    }
}