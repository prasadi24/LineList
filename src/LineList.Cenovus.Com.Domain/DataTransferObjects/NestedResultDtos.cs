namespace LineList.Cenovus.Com.API.DataTransferObjects
{
    public class CommodityResultDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SpecificationResultDto Specification { get; set; }
    }

    public class SpecificationResultDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class FacilityResultDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class LocationResultDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}