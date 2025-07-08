using LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefault;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefaultColumn;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefaultRow;

namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class EpProjectInsulationDefaultDetailsViewModel
    {
        public EpProjectInsulationDefaultResultDto InsulationDefault { get; set; }
        public IEnumerable<EpProjectInsulationDefaultRowResultDto> InsulationDefaultRows { get; set; }
        public IEnumerable<EpProjectInsulationDefaultColumnResultDto> InsulationDefaultColumns { get; set; }
        public List<EpProjectInsulationDefaultGridViewModel> GridData { get; set; }
    }
}