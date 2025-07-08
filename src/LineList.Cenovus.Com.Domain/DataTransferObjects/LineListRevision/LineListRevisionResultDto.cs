namespace LineList.Cenovus.Com.API.DataTransferObjects.LineListRevision
{
    public class LineListRevisionResultDto
    {
        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        public bool IsLocked { get; set; }

        public DateTime IssuedOn { get; set; }

        public bool IsSimpleRevisionBlock { get; set; }

        public int DocumentRevisionSort { get; set; }

        public string? Description { get; set; }

        public string? DocumentRevision { get; set; }

        public string? LockedBy { get; set; }

        public DateTime? LockedOn { get; set; }

        public string? PreparedBy { get; set; }

        public string? PreparedByProcess { get; set; }

        public string? PreparedByMechanical { get; set; }

        public string? ReviewedBy { get; set; }

        public string? ReviewByProcess { get; set; }

        public string? ReviewedByMechanical { get; set; }

        public string? ApprovedByLead { get; set; }

        public string? ApprovedByProject { get; set; }

        public Guid EpCompanyId { get; set; }

        public Guid LineListId { get; set; }

        public Guid LineListStatusId { get; set; }

        public Guid SpecificationId { get; set; }

        public Guid? AreaId { get; set; }

        public Guid? EpProjectId { get; set; }

        public Guid? LocationId { get; set; }
    }
}