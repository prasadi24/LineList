using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.ImportRowException
{
    public class ImportRowExceptionAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid ImportRowId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Message { get; set; }

        public int? SegmentNumber { get; set; }
    }
}