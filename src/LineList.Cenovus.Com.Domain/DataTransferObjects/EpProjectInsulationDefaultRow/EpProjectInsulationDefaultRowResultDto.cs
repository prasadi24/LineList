namespace LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefaultRow
{
    public class EpProjectInsulationDefaultRowResultDto
    {
        public Guid Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Guid EpProjectInsulationDefaultId { get; set; }

        public Guid SizeNpsId { get; set; }
    }
}