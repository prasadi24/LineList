namespace LineList.Cenovus.Com.API.DataTransferObjects.ImportRowException
{
    public class ImportRowExceptionResultDto
    {
        public Guid Id { get; set; }

        public Guid ImportRowId { get; set; }

        public string Message { get; set; }

        public int? SegmentNumber { get; set; }
    }
}