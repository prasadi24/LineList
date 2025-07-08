namespace LineList.Cenovus.Com.API.DataTransferObjects.ImportCommodity
{
    public class ImportCommodityResultDto
    {
        public Guid Id { get; set; }

        public Guid ImportId { get; set; }

        public Guid CommodityId { get; set; }

        public int BeforeCount { get; set; }

        public int AfterCount { get; set; }

        public CommodityResultDto Commodity { get; set; }
    }
}