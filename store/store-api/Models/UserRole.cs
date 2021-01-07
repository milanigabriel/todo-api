using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace store_api.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string NameUsers { get; set; }
        public string NameRoles { get; set; }
    }
}
