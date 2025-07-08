namespace LineList.Cenovus.Com.API.DataTransferObjects.EpCompanyAlpha
{
    public class EpCompanyAlphaResultDto
    {
        public Guid Id { get; set; }

        public string Alpha { get; set; }

        public Guid EpCompanyId { get; set; }

        public Guid FacilityId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}