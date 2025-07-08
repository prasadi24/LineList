namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class IncludeExistingLinesModel
    {
        public List<Guid> ExistingLineIds { get; set; }
        public Guid LineListRevisionId { get; set; }
        public string UserName { get; set; }
        public bool IsReferenceLine { get; set; }
    }
}
