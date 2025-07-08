namespace LineList.Cenovus.Com.API.DataTransferObjects.ReferenceLine
{
    public class ReferenceLineResultDto
    {
        public Guid Id { get; set; }

        public Guid LineListRevisionId { get; set; }

        public Guid LineRevisionId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}