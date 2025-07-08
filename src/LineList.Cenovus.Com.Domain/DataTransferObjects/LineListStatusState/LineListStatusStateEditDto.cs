using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.LineListStatusState
{
    public class LineListStatusStateEditDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public bool IsForUpRev { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid CurrentStatusId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid FutureStatusId { get; set; }

        public Guid? RequiredIssuedStatus1Id { get; set; }

        public Guid? RequiredIssuedStatus2Id { get; set; }

        public Guid? RequiredIssuedStatus3Id { get; set; }

        public Guid? ExcludeIssuedStatus1Id { get; set; }

        public Guid? ExcludeIssuedStatus2Id { get; set; }

        public Guid? ExcludeIssuedStatus3Id { get; set; }

        public Guid? ExcludeIssuedStatus4Id { get; set; }
    }
}