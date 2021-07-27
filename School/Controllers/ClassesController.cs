using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Models;

namespace School.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/classes")]
    public class ClassesController : ControllerBase
    {
        private readonly DataContext _context;

        public ClassesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Classes>>> GetClass()
        {
            return await _context.Classes.ToListAsync();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Classes>> GetClassByName(string name)
        {
            return await _context.Classes.FirstOrDefaultAsync(n => n.Name.Contains(name));
        }

        [HttpPost]
        public async Task<ActionResult<Classes>> SaveClass(Classes classes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!await _context.Classes.AnyAsync(c => c.Name == classes.Name))
                    {
                        _context.Classes.Add(classes);
                        int codReturn = await _context.SaveChangesAsync();

                        if (codReturn > 0)
                        {
                            return Ok("Classe cadastrada com sucesso");
                        }
                        else
                        {
                            return BadRequest("Não foi possível cadastrar a classe");
                        }
                    }
                    else
                    {
                        return BadRequest("Classe já existe");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut]
        public async Task<ActionResult> UpdateClasses(Classes classes)
        {
            try
            {
                _context.Classes.Update(classes);
                int codReturn = await _context.SaveChangesAsync();

                if (codReturn > 0)
                {
                    return Ok("Classe alterada com sucesso");
                }
                else
                {
                    return BadRequest("Não foi possível alterar a classe");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{classId}")]
        public async Task<ActionResult> RemoveClass(int classId)
        {
            try
            {
                Classes classes = await _context.Classes.FindAsync(classId);

                if (classes == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Remove(classes);
                    int codReturn = await _context.SaveChangesAsync();

                    if (codReturn > 0)
                    {
                        return Ok("Classe removida com sucesso");
                    }
                    else
                    {
                        return BadRequest("Não foi possível remover a classe");
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}