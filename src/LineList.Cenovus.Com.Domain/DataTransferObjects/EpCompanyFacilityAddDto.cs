using LineList.Cenovus.Com.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class EpCompanyFacilityAddViewModel
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid EpCompanyId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid FacilityId { get; set; }

        public List<Facility> Facilities { get; set; }

        public List<EpCompany> EpCompanies { get; set; }
    }
}