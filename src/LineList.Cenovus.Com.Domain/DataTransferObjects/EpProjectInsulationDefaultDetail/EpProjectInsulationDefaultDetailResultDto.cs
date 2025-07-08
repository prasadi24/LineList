namespace LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefaultDetail
{
    public class EpProjectInsulationDefaultDetailResultDto
    {
        public Guid Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Guid EpProjectInsulationDefaultColumnId { get; set; }

        public Guid EpProjectInsulationDefaultRowId { get; set; }

        public Guid InsulationThicknessId { get; set; }

        public Guid TracingDesignNumberOfTracersId { get; set; }
    }
}