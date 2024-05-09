using lab2.DTO;
using lab2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Day2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ITIContext db;
        public StudentController(ITIContext _db)
        {
            db = _db;
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetAllStds()
        {
            List<Student> students = db.Students.Include(d => d.Dept).Include(a => a.St_superNavigation).ToList();
            List<StudentDTO> studentsDTO = new List<StudentDTO>();
            foreach (var student in students)
            {
                StudentDTO studentDTO = new StudentDTO()
                {
                    St_Id = student.St_Id,
                    Name = $"{student.St_Fname} {student.St_Lname}",
                    Address = student.St_Address,
                    Age = student.St_Age,
                    DeptName = student.Dept?.Dept_Name ?? "No Department", 
                    superName = student.St_superNavigation?.St_Fname ?? "No Supervisor" 
                };
                studentsDTO.Add(studentDTO);
            }
            return Ok(studentsDTO) ;
        }
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            Student student = db.Students.Include(d=>d.Dept).Include(a=>a.St_superNavigation).FirstOrDefault(s => s.St_Id == id);
            if (student == null)
            {
                return NotFound();
            }
            else
            {
                StudentDTO studentDTO = new StudentDTO()
                {
                    St_Id = student.St_Id,
                    Name = $"{student.St_Fname} {student.St_Lname}",
                    Address = student.St_Address,
                    Age = student.St_Age,
                    DeptName = student.Dept.Dept_Name,
                    superName = student.St_superNavigation.St_Fname
                    
                };
                return Ok(studentDTO);
            }
          
        }
        [HttpPost("addStd")]
        public IActionResult AddStudent(StudentDTO student)
        {
            Student newStudent = new Student()
            {
                St_Id = student.St_Id,
                St_Fname = student.Name,
                St_Lname = student.Name,
                St_Age = student.Age,
               
            };
            //var dept = db.Departments.FirstOrDefault(a => a.Dept_Name.Contains(student.DeptName));
            //if (dept != null)
            //{
            //    dept.Students.Add(newStudent);
            //}

            db.Students.Add(newStudent);
            db.SaveChanges();
            return Created($"api/Student/{newStudent.St_Id}", newStudent);
           
        }
        [HttpPut("updateStd/{id}")]
        public IActionResult UpdateStudent(int id, StudentDTO student)
        {
            Student studentToUpdate = db.Students.FirstOrDefault(s => s.St_Id == id);
            if (studentToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                studentToUpdate.St_Fname = student.Name.Split(" ")[0];
                studentToUpdate.St_Lname = (student.Name.Split(" ").Count()>1)? student.Name?.Split(" ")[1]:" ";
                studentToUpdate.St_Age = student.Age;
                db.SaveChanges();
                return Ok(studentToUpdate);
            }
        }
        [HttpDelete("deleteStd/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            Student studentToDelete = db.Students.FirstOrDefault(s => s.St_Id == id);
            if (studentToDelete == null)
            {
                return NotFound();
            }
            else
            {
                db.Students.Remove(studentToDelete);
                db.SaveChanges();
                return Ok();
            }
        }
    }

}
