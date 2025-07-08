namespace LineList.Cenovus.Com.API.DataTransferObjects.EpProjectUserRole
{
    public class EpProjectUserRoleResultDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public Guid EpProjectId { get; set; }

        public Guid EpProjectRoleId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}