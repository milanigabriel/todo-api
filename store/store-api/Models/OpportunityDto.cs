using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace store_api.Models
{
    public class OpportunityDto
    {
        public string Status { get; set; }
        public int VehicleId { get; set; }
        public DateTime DateInitial { get; set; }
        public DateTime DateExpiration { get; set; }
        public decimal Price { get; set; }
        public string EmployeeName { get; set; }
    }
}
