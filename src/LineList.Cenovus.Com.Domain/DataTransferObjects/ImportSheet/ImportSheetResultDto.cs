using LineList.Cenovus.Com.API.DataTransferObjects.ImportRow;

namespace LineList.Cenovus.Com.API.DataTransferObjects.ImportSheet
{
    public class ImportSheetResultDto
    {
        public Guid Id { get; set; }

        public Guid ImportId { get; set; }

        public string Name { get; set; }

        public string SheetType { get; set; }

        public int NumberOfRows { get; set; }

        public int NumberOfExceptions { get; set; }

        public int NumberOfColumns { get; set; }

        public int NumberOfAccepted { get; set; }

        public int NumberOfImported { get; set; }

        public string? IgnoredFields { get; set; }
        public List<ImportRowResultDto> ImportRows { get; set; } 
        public List<ImportSheetColumnDto> Columns { get; set; } 
    }
    public class ImportSheetColumnDto
    {
        public string NameInDatabase { get; set; }
        public string NameInExcel { get; set; }
        public int SortOrder { get; set; }
    }
}