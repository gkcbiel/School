using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentController
{
    [Authorize]
    [ApiController]
    [Route("v1/student")]
    public class StudentController : ControllerBase
    {
        private readonly DataContext _context;

        public StudentController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetStudent()
        {
            return await _context.Student.ToListAsync();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<List<Student>>> GetStudentByName(string name)
        {
            return await _context.Student.Where(n => n.Name.Contains(name)).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Student.Add(student);
                    int codReturn = await _context.SaveChangesAsync();

                    if (codReturn > 0)
                    {
                        return Ok("Aluno cadastrado com sucesso");
                    }
                    else
                    {
                        return BadRequest("Não foi possível cadastrar aluno");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"erro tratado: {e.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateStudent(Student student)
        {
            try
            {
                _context.Student.Update(student);
                int codReturn = await _context.SaveChangesAsync();

                if (codReturn > 0)
                {
                    return Ok("Aluno alterado com sucesso");
                }
                else
                {
                    return BadRequest("Não foi possível alterar aluno");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"erro tratado: {e.Message}");
            }
        }

        [HttpDelete("{StudentId}")]
        public async Task<ActionResult> RemoveStudent(int StudentId)
        {
            try
            {
                Student student = await _context.Student.FindAsync(StudentId);

                if (student == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Remove(student);
                    int codReturn = await _context.SaveChangesAsync();

                    if (codReturn > 0)
                    {
                        return Ok("Aluno removido com sucesso");
                    }
                    else
                    {
                        return BadRequest("Não foi possível remover aluno");
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"erro tratado: {e.Message}");
            }
        }
    }
}