using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
	[Serializable]
	[Table("LineListRevision")]
	public class LineListRevision : LLTable
	{
		public bool IsActive { get; set; }

		public string Description { get; set; }
		public Guid? AreaId { get; set; }

		public Guid LineListId { get; set; }
		public virtual LineListModel LineList { get; set; }

		public string? DocumentRevision { get; set; }
		public int DocumentRevisionSort { get; set; }

		public Guid EpProjectId { get; set; }
		public Guid EpCompanyId { get; set; }
		public Guid? LocationId { get; set; }

		public string? LockedBy { get; set; }
		public DateTime? LockedOn { get; set; }
		public bool IsLocked { get; set; }
		public DateTime? IssuedOn { get; set; }

		public bool IsSimpleRevisionBlock { get; set; }

		public Guid SpecificationId { get; set; }
		public Guid LineListStatusId { get; set; }

		public string? PreparedBy { get; set; }
		public string? PreparedByProcess { get; set; }
		public string? PreparedByMechanical { get; set; }

		public string? ReviewedBy { get; set; }
		public string? ReviewByProcess { get; set; }
		public string? ReviewedByMechanical { get; set; }

		public string? ApprovedByLead { get; set; }
		public string? ApprovedByProject { get; set; }

		public virtual Area Area { get; set; }
		public virtual EpProject EpProject { get; set; }
		public virtual EpCompany EpCompany { get; set; }
		public virtual Location Location { get; set; }
		public virtual Specification Specification { get; set; }
		public virtual LineListStatus LineListStatus { get; set; }

		public virtual ICollection<LineRevision> LineRevisions { get; set; }

		[NotMapped]
		public string LineListDocumentNumber
		{
			get
			{
				return LineList != null ? LineList.DocumentNumber : string.Empty;
			}
		}
	}
}