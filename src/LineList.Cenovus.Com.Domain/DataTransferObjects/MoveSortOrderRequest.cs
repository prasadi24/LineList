namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class MoveSortOrderRequest
    {
        public Guid Id { get; set; }
        public string Direction { get; set; }
    }

    public class NameUniqueRequest
    {
        public string Name { get; set; }
        public string TableName { get; set; }
    }
}