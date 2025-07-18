﻿using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.Category
{
    public class SubCategoryAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(150, ErrorMessage = "This field length must be between {2} and {1} characters.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int CategoryId { get; set; }
    }
}