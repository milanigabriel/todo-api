using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using store_api.Data;
using store_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace store_api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/role")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RolesController(StoreDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost("create")]
        public async Task<ActionResult<RoleInfo>> CreateRole([FromBody] RoleInfo model)
        {
            var role = new IdentityRole { Name = model.Name };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return NotFound();
            }
            return Ok(new { role.Name });

        }
        [HttpPost("cadastro")]
        public async Task<ActionResult<UserRole>> CadastrarRoleUser([FromBody] UserRoleDto model)
        {
            var role = await _roleManager.FindByNameAsync(model.NameRoles);
            if (role == null)
            {
                return NotFound("Problemas com as Roles.");
            }
            var user = await _userManager.FindByNameAsync(model.NameUsers);
            if (user == null)
            {
                return NotFound("Problemas com os Usuarios.");
            }
            var userRole = new UserRole()
            {
                UserId = user.Id,
                RoleId = role.Id,
                NameRoles = role.Name,
                NameUsers = user.Email
            };
            _context.UsersRoles.Add(userRole);
            await _context.SaveChangesAsync();
            
            return Ok(userRole);

        }
        [HttpPost("pay")]
        public async Task<ActionResult<UserRole>> PagamentoRoleUser([FromBody] UserRolePagamento model)
        {
            model.J = 5m;
            model.P = 7.5m;
            model.S = 11m;
            decimal value = model.Valor;
            if (model.Salario == 0)
            {
                return NotFound();
            }
            var user = await _context.UsersRoles.FindAsync(model.Id);
            if (user.NameRoles == "Júnior")
            {
                if(value != 0)
                {
                    model.Result = model.Salario + (model.Salario * (value / 100));
                }
                else
                {
                    model.Result = model.Salario + (model.Salario * (model.J / 100));
                }
            }
            if (user.NameRoles == "Pleno")
            {
                if (model.Valor == 0m)
                {
                    model.Result = model.Salario + (model.Salario * (model.P / 100));
                }
                else
                {
                    model.Result = model.Salario + (model.Salario * (model.Valor / 100));
                }
            }
            if (user.NameRoles == "Sênior")
            {
                if (model.Valor == 0m)
                {
                    model.Result = model.Salario + (model.Salario * (model.S / 100));
                }
                else
                {
                    model.Result = model.Salario + (model.Salario * (model.Valor / 100));
                }
            }
            return Ok(model.Result);
        }
    }
}
