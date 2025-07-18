﻿using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.SizeNps
{
    public class SizeNpsResultDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Notes { get; set; }

        [Display(Name = "Sort")]
        public int SortOrder { get; set; }

        [Display(Name = "Decimal Value")]
        public string DecimalValue { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}