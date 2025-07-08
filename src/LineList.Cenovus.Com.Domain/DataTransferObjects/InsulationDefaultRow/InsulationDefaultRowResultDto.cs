namespace LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultRow
{
    public class InsulationDefaultRowResultDto
    {
        public Guid Id { get; set; }

        public Guid InsulationDefaultId { get; set; }

        public Guid? SizeNpsId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
        public string SizeNpsName { get; set; }
    }
}