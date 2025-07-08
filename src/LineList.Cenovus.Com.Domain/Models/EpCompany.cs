using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("EpCompany")]
    public partial class EpCompany : LLLookupTable
    {
        public string ActiveDirectoryGroup { get; set; }
        public string? Description { get; set; }

        public string? Notes { get; set; }

        public virtual ICollection<EpCompanyAlpha> EpCompanyAlphas { get; set; }

        public byte[]? Logo { get; set; }

        [NotMapped] //can be in partial class really used by UI but could be implemented here as well
        public byte[] LogoThumbnail { get; set; }
    }
}