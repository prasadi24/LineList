using LineList.Cenovus.Com.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class ReserveLinesViewModel : Entity
    {
        // Dropdown collections
        public List<Specification> Specifications { get; set; } = new();
        public List<Location> Locations { get; set; } = new();
        public List<Commodity> Commodities { get; set; } = new();

        // Line List Revision metadata
        public Guid? LineListRevisionId { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentRevision { get; set; }
        public string LineListStatusName { get; set; }

        // Form inputs
        public Guid SpecificationId { get; set; }
        public string SpecificationName { get; set; }
        //[Required(ErrorMessage = "Location is required.")]
        public Guid? LocationId { get; set; }
        
        public Guid? CommodityId { get; set; }

        public Guid SelectedLocationId { get; set; }
        public Guid SelectedCommodityId { get; set; }


        [Range(1, 9999, ErrorMessage = "Starting Line Sequence must be between 1 and 9999.")]
        public int StartingLineSequence { get; set; }
        [Required(ErrorMessage = "Number of Lines is required.")]
        [Range(1, 9999, ErrorMessage = "Number of Lines must be between 1 and 9999.")]
        public int NumberOfLines { get; set; }
        public bool Contiguous { get; set; }
        public bool OverrideSequence { get; set; }

    }
}