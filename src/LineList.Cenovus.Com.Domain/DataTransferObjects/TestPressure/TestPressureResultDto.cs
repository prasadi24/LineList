namespace LineList.Cenovus.Com.API.DataTransferObjects.TestPressure
{
    public class TestPressureResultDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Notes { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}