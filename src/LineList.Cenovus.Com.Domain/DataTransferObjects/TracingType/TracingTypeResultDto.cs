﻿using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.TracingType
{
    public class TracingTypeResultDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Notes { get; set; }

        [Display(Name = "Sort")]
        public int SortOrder { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public Guid SpecificationId { get; set; }

        [Display(Name = "Specification")]
        public string SpecificationName { get; set; }

        public string? Temperature { get; set; }

        [Display(Name = "Is Jacketed")]
        public bool IsJacketed { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}