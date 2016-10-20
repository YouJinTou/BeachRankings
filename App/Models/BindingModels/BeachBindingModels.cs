﻿namespace BeachRankings.App.Models.BindingModels
{
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AddBeachBindingModel
    {
        [Required(ErrorMessage = "The name field is required.")]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required]
        public string CountryName { get; set; }

        [Required(ErrorMessage = "The location field is required.")]
        [MinLength(2, ErrorMessage = "The location name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The location name cannot be longer than 100 characters.")]
        public string LocationName { get; set; }

        [MaxLength(350, ErrorMessage = "The description cannot be longer than 350 characters.")]
        public string Description { get; set; }

        [Required]
        public int WaterBodyId { get; set; }

        [Required(ErrorMessage = "A body of water is required.")]
        [MaxLength(100, ErrorMessage = "There is no body of water with that long of a name.")]
        public string WaterBodyName { get; set; }

        public string ApproximateAddress { get; set; }

        public string Coordinates { get; set; }

        public IEnumerable<BeachPhoto> Photos { get; set; }
    }
}