using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefault;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultColumn;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultDetail;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultRow;

namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class InsulationDefaultDetailsViewModel
    {
        public InsulationDefaultResultDto InsulationDefault { get; set; }
        public IEnumerable<InsulationDefaultRowResultDto> InsulationDefaultRows { get; set; }
        public IEnumerable<InsulationDefaultColumnResultDto> InsulationDefaultColumns { get; set; }

        public IEnumerable<InsulationDefaultDetailResultDto> InsulationDefaultDetails { get; set; }

        public List<InsulationDefaultGridViewModel> GridData { get; set; }
    }
}