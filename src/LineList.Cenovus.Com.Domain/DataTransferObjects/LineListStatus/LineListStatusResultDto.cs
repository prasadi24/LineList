using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.LineListStatus
{
    public class LineListStatusResultDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Notes { get; set; }

        [Display(Name = "Sort")]
        public int SortOrder { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public bool IsHardRevision { get; set; }

        public Guid? IsIssuedOfId { get; set; }

        public Guid? IsDraftOfId { get; set; }

        public Guid? CorrespondingLineStatusId { get; set; }

        public Guid? DefaultUpRevStatusId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}