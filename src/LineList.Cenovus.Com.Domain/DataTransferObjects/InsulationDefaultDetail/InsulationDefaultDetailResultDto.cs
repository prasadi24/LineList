namespace LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultDetail
{
    public class InsulationDefaultDetailResultDto
    {
        public Guid Id { get; set; }

        public Guid InsulationDefaultColumnId { get; set; }

        public Guid InsulationDefaultRowId { get; set; }

        public Guid? InsulationThicknessId { get; set; }

        public Guid? TracingDesignNumberOfTracersId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}