namespace LineList.Cenovus.Com.API.DataTransferObjects.RoleUser
{
    public class RoleUserResultDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public Guid RoleId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}