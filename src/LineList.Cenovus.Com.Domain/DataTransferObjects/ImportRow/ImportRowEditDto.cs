using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.ImportRow
{
    public class ImportRowEditDto : ImportRowAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid Id { get; set; }
    }
}