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
    [Route("v1/studentclassgrade")]
    [ApiController]
    public class StudentClassGradeController : ControllerBase
    {
        private readonly DataContext _context;

        public StudentClassGradeController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<StudentClassGrade>>> GetStudentClassGrade()
        {
            return await _context.StudentClassGrade
            .Include(c => c.Classes)
            .Include(s => s.Student)
            .ToListAsync();
        }

        [HttpGet("Teste")]
        public async Task<ActionResult<List<StudentClassGrade>>> GetStudentAndClassByName(string studentName = null, string className = null)
        {
            var getNameAndClass = await _context.StudentClassGrade
            .Include(x => x.Classes)
            .Include(s => s.Student)
            .Where(l => l.Student.Name.Contains(studentName) || l.Classes.Name.Contains(className))
            .ToListAsync();

            return getNameAndClass;
        }

        [HttpPost]
        public async Task<ActionResult<StudentClassGrade>> CreateStudentClassGrade(StudentClassGrade studentClassGrade)
        {
            try
            {
                _context.StudentClassGrade.Add(studentClassGrade);
                int codReturn = await _context.SaveChangesAsync();

                if (codReturn > 0)
                {
                    return Ok("Nota cadastrada com sucesso");
                }
                else
                {
                    return BadRequest("Não foi possível cadastrar a nota");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"erro tratado: {e.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudentClassGrade(StudentClassGrade studentClassGrade)
        {
            try
            {
                _context.StudentClassGrade.Update(studentClassGrade);
                int codReturn = await _context.SaveChangesAsync();

                if (codReturn > 0)
                {
                    return Ok("Nota alterada com sucesso");
                }
                else
                {
                    return BadRequest("Não foi possível alterar a nota");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"erro tratado: {e.Message}");
            }
        }

        [HttpDelete("{studentClassGradeId}")]
        public async Task<ActionResult> RemoveStudentClassGrade(int studentClassGradeId)
        {
            try
            {
                StudentClassGrade studentClassGrade = await _context.StudentClassGrade.FindAsync(studentClassGradeId);

                if (studentClassGrade == null)
                {
                    return BadRequest("Nota não encontrada");
                }
                else
                {
                    _context.Remove(studentClassGrade);
                    int codReturn = await _context.SaveChangesAsync();

                    if (codReturn > 0)
                    {
                        return Ok("Nota removida com sucesso");
                    }
                    else
                    {
                        return BadRequest("Não foi possível remover a nota");
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