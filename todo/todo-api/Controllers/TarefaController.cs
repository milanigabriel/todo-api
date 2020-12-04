using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_api.Data;
using todo_api.Models;

namespace todo_api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/task")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public TarefaController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<Tarefa>> CreateTask(TarefaDto model)
        {
            var category = _context.Categories.Find(model.CategoryId);
            if(category == null)
            {
                return BadRequest();
            }
            var tarefa = new Tarefa()
            {
                Name = model.Name,
                Description = model.Description,
                Category = category
            };
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();

            return Ok(tarefa);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarefa>> GetTask(int id)
        {
            var tarefa = await _context.Tarefas.Where(x => x.Id == id).Include(x => x.Category).ToListAsync();
            if (tarefa == null)
            {
                return BadRequest();
            }
            return Ok(tarefa);
        }
        [HttpGet]
        public async Task<ActionResult<Tarefa>> GetAllTarefa()
        {
            var tarefa = await _context.Tarefas.Include(x => x.Category).ToListAsync();

            return Ok(tarefa);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Tarefa>> DeleteTask(int id)
        {
            var tarefa = _context.Tarefas.Find(id);
            if(tarefa == null)
            {
                return BadRequest();
            }
            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();

            return Ok(tarefa);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Tarefa>> UpdateTask(int id, TarefaDto model)
        {
            var exists = _context.Tarefas.Any(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }
            var category = _context.Categories.Find(model.CategoryId);
            if(category == null)
            {
                return BadRequest();
            }
            var tarefa = new Tarefa()
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                Category = category
            };
            _context.Entry(tarefa).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(tarefa);
        }
        [HttpPut("done/{id}")]
        public async Task<ActionResult<Tarefa>> DoneTask(int id)
        {
            var tarefa = _context.Tarefas.Find(id);
            if(tarefa == null)
            {
                return NotFound();
            }
            tarefa.Done = true;
            _context.Entry(tarefa).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Tarefa concluida com sucesso.");
        }
    }
}
