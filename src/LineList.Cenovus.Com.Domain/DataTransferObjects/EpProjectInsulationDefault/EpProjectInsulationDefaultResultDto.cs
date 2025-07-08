namespace LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefault
{
    public class EpProjectInsulationDefaultResultDto
    {
        public Guid Id { get; set; }

        public string? Description { get; set; }

        public string? SpecificationRevision { get; set; }

        public DateTime? SpecificationRevisionDate { get; set; }

        public string? Name { get; set; }

        public string? Notes { get; set; }

        public string? LinkToDocument { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int SortOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public Guid EpProjectId { get; set; }

        public Guid InsulationMaterialId { get; set; }

        public Guid InsulationTypeId { get; set; }

        public Guid TracingTypeId { get; set; }

        public string InsulationMaterialName { get; set; }
        public string InsulationMaterialDescription { get; set; }
        public string InsulationTypeName { get; set; }
        public string InsulationTypeDescription { get; set; }
        public string TracingTypeName { get; set; }
        public string TracingTypeDescription { get; set; }
    }
}