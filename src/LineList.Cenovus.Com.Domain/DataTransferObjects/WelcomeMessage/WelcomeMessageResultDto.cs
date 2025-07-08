namespace LineList.Cenovus.Com.API.DataTransferObjects.WelcomeMessage
{
    public class WelcomeMessageResultDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string? Notes { get; set; }

        public string? Message2 { get; set; }

        public string? Message3 { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}