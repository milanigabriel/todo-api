using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace store_api.Models
{
    public class UserRoleDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string NameUsers { get; set; }
        public string NameRoles { get; set; }
    }
}
