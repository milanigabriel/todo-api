using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace store_api.Models
{
    public class Opportunity
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public Vehicle Vehicle { get; set; }
        [Required]
        public int VehicleId { get; set; }
        [Required]
        public DateTime DateInitial { get; set; }
        [Required]
        public DateTime DateExpiration { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required]
        public string Employee { get; set; }
    }
}
