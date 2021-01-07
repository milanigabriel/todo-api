using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_api.Data;
using store_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace store_api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/opportunity")]
    [ApiController]
    public class OpportunityController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public OpportunityController(StoreDbContext context)
        {
            _context = context;
        }


        [HttpPost("create")]
        public async Task<ActionResult<Opportunity>> CreateOpportunity(OpportunityDto model)
        {
            var vehicle = _context.Vehicles.Find(model.VehicleId);
            if (vehicle == null)
            {
                return NotFound();
            }
            if (vehicle.StatusValue == true)
            {
                return NotFound("Carro não esta disponivel.");
            }
            var Data = DateTime.Now;
            vehicle.StatusValue = true;

            var opportunity = new Opportunity()
            {
                Status = "Criada",
                DateInitial = Data,
                DateExpiration = Data.AddDays(2),
                Vehicle = vehicle,
                Employee = User.Identity.Name,
                Price = model.Price
            };
            _context.Opportunities.Add(opportunity);
            await _context.SaveChangesAsync();

            return Ok(opportunity);
        }
        [HttpGet("search/{id}")]
        public async Task<ActionResult<Opportunity>> GetOpportunity(int id)
        {
            var model = await _context.Opportunities.Include(x => x.Vehicle).Where(x => x.Employee == User.Identity.Name).Where(x => x.Id == id).ToListAsync();
            if (model == null)
            {
                return NotFound("Esta oportunidade não existe.");
            }
            return Ok(model);
        }
        [HttpGet("search")]
        public async Task<ActionResult<Opportunity>> GetAllOpportunity()
        {
            var model = await _context.Opportunities.Include(x => x.Vehicle).Where(x => x.Employee == User.Identity.Name).ToListAsync();
            if (model == null)
            {
                return NotFound("Esta lista de oportunidade não existe.");
            }
            return Ok(model);
        }
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Opportunity>> DeleteOpportunity(int id)
        {
            var model = await _context.Opportunities.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            if (model.Employee != User.Identity.Name)
            {
                return NotFound();
            }
            _context.Opportunities.Remove(model);
            await _context.SaveChangesAsync();

            return model;
        }
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Opportunity>> UpdateOpportunity(int id, OpportunityUpdate model)
        {
            var mod = _context.Opportunities.Find(id);
            if (mod == null)
            {
                return NotFound("Opportunity nao existe.");
            }
            if (mod.Employee != User.Identity.Name)
            {
                return NotFound();
            }
            if (mod.Status == "Aceito" || mod.Status == "Cancelado" || mod.Status == "Expirado")
            {
                return NotFound("Contrato Inacessivel.");
            }
            var vehicleOld = await _context.Vehicles.FindAsync(mod.VehicleId);
            vehicleOld.StatusValue = false;

            var vehicle = await _context.Vehicles.FindAsync(model.VehicleId);
            if (vehicle == null)
            {
                return NotFound();
            }
            if (vehicle.StatusValue == true)
            {
                return NotFound("Veiculo nao esta disponivel.");
            }

            vehicle.StatusValue = true;
            mod.Vehicle = vehicle;
            mod.Price = model.Price;

            _context.Entry(mod).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return mod;
        }

        [HttpPut("accept/{id}")]
        public async Task<ActionResult<Opportunity>> AceptOpportunity(int id)
        {
            var mod = _context.Opportunities.Find(id);
            if (mod == null)
            {
                return NotFound();
            }
            if (mod.Employee != User.Identity.Name)
            {
                return NotFound();
            }
            if (mod.Status == "Aceito" || mod.Status == "Cancelado" || mod.Status == "Expirado")
            {
                return NotFound("Contrato Inacessivel.");
            }
            mod.Status = "Aceito";

            _context.Entry(mod).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok($"Contrato {new { mod.Id }} aceito.");
        }


        [HttpPut("cancel/{id}")]
        public async Task<ActionResult<Opportunity>> CancelOpportunity(int id)
        {
            var mod = _context.Opportunities.Find(id);
            if (mod == null)
            {
                return NotFound();
            }
            if (mod.Employee != User.Identity.Name)
            {
                return NotFound();
            }
            if (mod.Status == "Aceito" || mod.Status == "Cancelado" || mod.Status == "Expirado")
            {
                return NotFound("Contrato Inacessivel.");
            }
            mod.Status = "Cancelado";

            _context.Entry(mod).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok($"Contrato {new { mod.Id }} cancelado.");
        }
        [HttpPut("expiration/{id}")]
        public async Task<ActionResult<Opportunity>> ExpirationOpportunity(int id)
        {
            var mod = _context.Opportunities.Find(id);
            if (mod == null)
            {
                return NotFound();
            }
            if (mod.Employee != User.Identity.Name)
            {
                return NotFound();
            }
            if (mod.Status == "Aceito" || mod.Status == "Cancelado" || mod.Status == "Expirado")
            {
                return NotFound("Contrato Inacessivel.");
            }
            if (mod.DateExpiration < DateTime.Now)
            {
                mod.Status = "Expirado";

                _context.Entry(mod).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok($"Contrato {new { mod.Id }} Expirado.");
            }
            return Ok("contrato em aberto");
        }
    }
}