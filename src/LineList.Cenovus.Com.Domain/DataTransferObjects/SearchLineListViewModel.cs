using LineList.Cenovus.Com.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class SearchLineListViewModel
    {
        public List<Facility> Facilities { get; set; } = new();
        public List<CenovusProject> CenovusProjects { get; set; } = new();
        public List<ProjectType> ProjectTypes { get; set; } = new();
        public List<Specification> Specifications { get; set; } = new();
        public List<EpCompany> EPs { get; set; } = new();
        public List<LineListStatus> LineListStatuses { get; set; } = new();
        public List<Location> Locations { get; set; } = new();
        public List<EpProject> EPProjects { get; set; } = new();
        public List<Commodity> Commodities { get; set; } = new();
        public List<Area> Areas { get; set; } = new();
        public List<LineListModel> LineLists { get; set; } = new();
        public List<LineListModel> DocNumbers { get; set; } = new();
        public List<ModularDropDownModel> ModularDetails { get; set; } = new();
        public List<PipeSpecification> PipeSpecifications { get; set; } = new();
        public List<LineStatus> LineStatuses { get; set; } = new();

        public bool ShowOnlyActive { get; set; } = true;
        public bool ShowDrafts { get; set; } = true;
        // Property to trigger auto-search when coming from EpProject
        public bool AutoSearch { get; set; } = false;

        // Selected values
        public Guid LineListId { get; set; }

        public Guid SelectedFacilityId { get; set; }

        public Guid SelectedProjectId { get; set; }
        public Guid SelectedProjectTypeId { get; set; }
        public Guid SelectedSpecificationId { get; set; }
        public Guid SelectedEPId { get; set; }
        public Guid SelectedLineListStatusId { get; set; }
        public Guid SelectedLocationId { get; set; }
        public Guid SelectedEPProjectId { get; set; }
        public Guid SelectedAreaId { get; set; }
        public string? SelectedDocumentNumberId { get; set; }
        public string? SelectedModularID { get; set; }
        public Guid SelectedCommodityId { get; set; }

        public Guid SelectedPipeSpecificationId { get; set; }
        public Guid SelectedLineStatusId { get; set; }
        public Guid SelectedcenovusProjectId { get; set; }
        public Guid LineListRevisionId { get; set; }

        public string? SelectedSequenceNumber { get; set; }


        // Added LineType property to store the type of lines being included ("existing" or "reference")
        public string? LineType { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? CreatedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime CreatedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        public Guid Id { get; set; }
        public string? DocumentNumber { get; set; }
        public string Description { get; set; }
        public string? DocumentRevision { get; set; }
        public int DocumentRevisionSort { get; set; }

        public string SelectedBlock { get; set; }
        public string? PreparedBy { get; set; }
        public string? ReviewedBy { get; set; }
        public string? ApprovedByLead { get; set; }
        public string? ApprovedByProject { get; set; }
        //Complex Object
        public string? PreparedByProcess { get; set; }
        public string? PreparedByMechanical { get; set; }
        public string? ReviewByProcess { get; set; }
        public string? ReviewedByMechanical { get; set; }
        public string? ApprovedByLeadComplex { get; set; }
        public string? ApprovedByProjectComplex { get; set; }

        // Added properties for button state management
        public bool IsDraft { get; set; }
        public bool IsMostRecentRevision { get; set; }
        public bool IsReserved { get; set; }
        public bool IsIssued { get; set; }
        public bool IsLocked { get; set; }

        public string? NextRevision { get; set; }
        public string? LineListStatusName { get; set; }

        public bool IsEpUser { get; set; }
        public bool IsEpAdmin { get; set; }
        public bool IsCenovusAdmin { get; set; }

        public bool IsEpLeadEngineer { get; set; }
        public bool IsEpDataEntry { get; set; }

        public bool CanAdd { get; set; }
        public bool IsReadOnly { get; set; }
        public bool CanRevertToDraft { get; set; }
        public bool CanPrintForIssue { get; set; }
        public bool HasBeenIssued { get; set; }
        public bool IsSimpleRevisionBlock { get; set; }
        public string PrintLabel { get; set; }
        public string SelectedLineListStatusName { get; set; }
        public string LockedBy { get; set; }
        public string LockedOn { get; set; }
        public bool IsEditable { get; set; }
        public string DateValidationMessage { get; set; }

        // Global
        public bool DisableActive { get; set; }
        public bool DisableDescription { get; set; }
        public bool DisableDocumentNumber { get; set; }

        // Dropdowns
        public bool DisableCenovusProject { get; set; }
        public bool DisableEP { get; set; }
        public bool DisableEPProject { get; set; }
        public bool DisableFacility { get; set; }
        public bool DisableLocation { get; set; }
        public bool DisableArea { get; set; }
        public bool DisableProjectType { get; set; }
        public bool DisableSpecification { get; set; }

        // Status and Block
        public bool DisableRevisionStatus { get; set; }
        public bool DisableBlockSelection { get; set; }

        // Simple Block
        public bool DisableSimpleBlock { get; set; }
        public bool DisablePreparedBy { get; set; }
        public bool DisableReviewedBy { get; set; }
        public bool DisableApprovedByLead { get; set; }
        public bool DisableApprovedByProject { get; set; }

        // Complex Block
        public bool DisableComplexBlock { get; set; }
        public bool DisablePreparedByProcess { get; set; }
        public bool DisablePreparedByMechanical { get; set; }
        public bool DisableReviewByProcess { get; set; }
        public bool DisableReviewedByMechanical { get; set; }
        public bool DisableApprovedByLeadComplex { get; set; }
        public bool DisableApprovedByProjectComplex { get; set; }
        public bool SaveVisible { get; set; }
        public bool PrintForIssueVisible { get; set; }
        public bool PrintForIssueEnabled { get; set; }
        public bool RevertToDraftVisible { get; set; }
        public bool DiscardDraftVisible { get; set; }
        public bool LockedEnabled { get; set; }
    }
}