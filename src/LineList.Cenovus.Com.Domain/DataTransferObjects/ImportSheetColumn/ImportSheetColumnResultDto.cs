namespace LineList.Cenovus.Com.API.DataTransferObjects.ImportSheetColumn
{
    public class ImportSheetColumnResultDto
    {
        public Guid Id { get; set; }

        public Guid ImportSheetId { get; set; }

        public string NameInExcel { get; set; }

        public string NameInDatabase { get; set; }

        public int SortOrder { get; set; }
    }
}