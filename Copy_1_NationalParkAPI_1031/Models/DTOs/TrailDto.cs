﻿using System.ComponentModel.DataAnnotations;
using static Copy_1_NationalParkAPI_1031.Models.Trail;

namespace Copy_1_NationalParkAPI_1031.Models.DTOs
{
    public class TrailDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Distance { get; set; }
        [Required]
        public string Elevation { get; set; }
        public DifficultyType Difficulty { get; set; }
        public int NationalParkId { get; set; }
        public NationalPark NationalPark { get; set; }  
    }
}
