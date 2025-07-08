using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.LineListRevision
{
    public class LineListRevisionAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public bool IsLocked { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime IssuedOn { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public bool IsSimpleRevisionBlock { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int DocumentRevisionSort { get; set; } = 0;

        [StringLength(60, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? Description { get; set; }

        [StringLength(5, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? DocumentRevision { get; set; }

        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? LockedBy { get; set; }

        public DateTime? LockedOn { get; set; }

        [StringLength(3)]
        public string? PreparedBy { get; set; }

        [StringLength(3)]
        public string? PreparedByProcess { get; set; }

        [StringLength(3)]
        public string? PreparedByMechanical { get; set; }

        [StringLength(3)]
        public string? ReviewedBy { get; set; }

        [StringLength(3)]
        public string? ReviewByProcess { get; set; }

        [StringLength(3)]
        public string? ReviewedByMechanical { get; set; }

        [StringLength(3)]
        public string? ApprovedByLead { get; set; }

        [StringLength(3)]
        public string? ApprovedByProject { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid EpCompanyId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid LineListId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid LineListStatusId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid SpecificationId { get; set; }

        public Guid? AreaId { get; set; }

        public Guid? EpProjectId { get; set; }

        public Guid? LocationId { get; set; }
    }
}