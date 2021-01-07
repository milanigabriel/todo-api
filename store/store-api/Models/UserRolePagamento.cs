using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace store_api.Models
{
    public class UserRolePagamento
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string NameUsers { get; set; }
        public string NameRoles { get; set; }
        public decimal Valor { get; set; }
        public decimal Salario { get; set; }
        public decimal Result { get; set; }
        public decimal J { get; set; }
        public decimal P { get; set; }
        public decimal S { get; set; }
    }
}
