using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
	[Serializable]
	[Table("Line")]
	public class Line : LLTable
	{
		[Required]
		[ForeignKey("Location")]
		public Guid LocationId { get; set; }

		public virtual Location Location { get; set; }

		[Required]
		[ForeignKey("Commodity")]
		public Guid CommodityId { get; set; }

		public virtual Commodity Commodity { get; set; }

		[MaxLength(4)]
		[Required]
		public string SequenceNumber { get; set; }

		[Required]
		public int ChildNumber { get; set; }

		public string? ModularId { get; set; }

		public virtual ICollection<LineRevision> LineRevisions { get; set; }

		public int ActiveLineRevisionCount
		{
			get
			{
				return LineRevisions.Where(x => x.IsActive).Count();
			}
		}

		public int AsBuiltRevisionCount
		{
			get
			{
				return LineRevisions.Where(x => x.LineStatus.Name.Contains("As Built")).Count();
			}
		}
	}
}