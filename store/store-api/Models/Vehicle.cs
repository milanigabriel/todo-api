using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace store_api.Models
{
    public class Vehicle
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string Fuel { get; set; }
        [Required]
        public string Motor { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public bool StatusValue { get; set; }
        [Required]
        public int Year { get; set; }
    }
}
