namespace LineList.Cenovus.Com.API.DataTransferObjects.LineListModel
{
    public class LineListModelResultDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid FacilityId { get; set; } // Foreign Key to Facility

        public Guid ProjectTypeId { get; set; } // Foreign Key to ProjectType
    }
}