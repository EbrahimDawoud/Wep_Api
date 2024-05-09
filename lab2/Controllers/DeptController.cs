using lab2.DTO;
using lab2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeptController : ControllerBase
    {
        public ITIContext db;
        public DeptController( ITIContext _db)
        {
            db = _db;
        }
        [HttpGet]
        public IActionResult GetAllDepts()
        {
            var depts = db.Departments.Include(a=>a.Students).ToList();
            var deptsDTO = new List<DeptDTO>();
            foreach (var dept in depts)
            {
                DeptDTO deptDTO = new DeptDTO()
                {
                    Dept_Id = dept.Dept_Id,
                    Dept_Name = dept.Dept_Name,
                    NoofStudents = dept.Students.Count,
                    Dept_Location = dept.Dept_Location,
                    studentNames = new List<string>()
                };
                foreach (var student in dept.Students)
                {
                    deptDTO.studentNames.Add($"{student.St_Fname} {student.St_Lname}");
                }
                deptsDTO.Add(deptDTO);
            }
            return Ok(deptsDTO);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Department dept = db.Departments.Include(a => a.Students).FirstOrDefault(d => d.Dept_Id == id);
            if (dept == null)
            {
                return NotFound();
            }
            else
            {
                DeptDTO deptDTO = new DeptDTO()
                {
                    Dept_Id = dept.Dept_Id,
                    Dept_Name = dept.Dept_Name,
                    NoofStudents = dept.Students.Count,
                    Dept_Location = dept.Dept_Location,
                    studentNames = new List<string>()                    
                };
                foreach (var student in dept.Students)
                {
                    deptDTO.studentNames.Add($"{student.St_Fname} {student.St_Lname}");
                }
                return Ok(deptDTO);
            }

            
        }
        [HttpGet("pagination")]
        public IActionResult GetByPagination(int page, int pageSize)
        {
            var depts = db.Departments.Include(a => a.Students).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var deptsDTO = new List<DeptDTO>();
            foreach (var dept in depts)
            {
                DeptDTO deptDTO = new DeptDTO()
                {
                    Dept_Id = dept.Dept_Id,
                    Dept_Name = dept.Dept_Name,
                    NoofStudents = dept.Students.Count,
                    Dept_Location = dept.Dept_Location,
                    studentNames = new List<string>()
                };
                foreach (var student in dept.Students)
                {
                    deptDTO.studentNames.Add($"{student.St_Fname} {student.St_Lname}");
                }
                deptsDTO.Add(deptDTO);
            }
            return Ok(deptsDTO);
        }
        [HttpPost("AddDept")]

        [Consumes("application/json")]
        public IActionResult AddDept([FromBody] DeptDTO departmentDto)

        {
            Department department = new Department()
            {
                Dept_Id = departmentDto.Dept_Id,
                Dept_Name = departmentDto.Dept_Name,
                Dept_Location = departmentDto.Dept_Location,

            };
            db.Departments.Add(department);
            db.SaveChanges();
            //Console.WriteLine(department);
            return Created("Dept Added", department);
            
        }
    }
}
