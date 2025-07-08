using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class MoveLinesViewModel
    {
        public List<EpProject> EPProjects { get; set; }

        public List<LineListModel> LineLists { get; set; }
        public Guid SelectedEPProjectId { get; set; }
        public Guid SelectedLineListId { get; set; }
        public Guid LineListRevisionId { get; set; }

        public string ValidationMessage { get; set; }

    }
    public class MoveLinesValidationRequest
    {
        public Guid LineListRevisionId { get; set; }
        public List<Guid> SelectedLineIds { get; set; }
    }
}