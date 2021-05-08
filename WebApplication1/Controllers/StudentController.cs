using BLL.Request;
using BLL.Services;
using DLL.Models;
using LightQuery.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class StudentController : MainApiController
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentService _studentService;

        public StudentController(ILogger<StudentController> logger, IStudentService studentService)
        {
            _logger = logger;
            _studentService = studentService;
        }

        // GET: StudentController
        [AsyncLightQuery(forcePagination: true, defaultPageSize: 10, defaultSort: "studentId desc")]
        [HttpGet]
        public ActionResult IndexAsync()
        {
            return Ok(_studentService.GetAll());
        }

        // GET: StudentController/Details/5
        [HttpGet(template: "{email}")]
        public async Task<ActionResult> DetailsAsync(string email)
        {
            return Ok(await _studentService.FindAsync(email));
        }

        // POST: StudentController/Create
        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromForm] StudentCreateRequestViewModel student)
        {
            return Ok(await _studentService.CreateAsync(student));
        }

        // PUT: StudentController/Edit/5
        [HttpPut(template: "{email}")]
        public async Task<ActionResult> EditAsync(string email, [FromForm] Student student)
        {
            return Ok(await _studentService.UpdateAsync(email, student));
        }

        // DELETE: StudentController/Delete/5
        [HttpDelete(template: "{email}")]
        public async Task<ActionResult> DeleteAsync(string email)
        {
            return Ok(await _studentService.DeleteAsync(email));
        }
    }

    //public static class StudentStatic
    //{
    //    public static List<Student> AllStudents = new List<Student>();
    //    public static Student CreateStudent(Student student)
    //    {
    //        AllStudents.Add(student);
    //        return student;
    //    }
    //    public static List<Student> GetAllStudent()
    //    {
    //        return AllStudents;
    //    }
    //    public static Student GetStudent(int id)
    //    {
    //        return AllStudents.FirstOrDefault(x => x.Id == id);
    //    }
    //    public static Student UpdateStudent(int id, Student student)
    //    {
    //        Student result = new Student();
    //        foreach (var astudent in AllStudents)
    //        {
    //            if (astudent.Id == student.Id)
    //            {
    //                astudent.Name = student.Name;
    //                result = astudent;
    //            }
    //        }
    //        return result;
    //    }
    //    public static Student DeleteStudent(int id)
    //    {
    //        var result = AllStudents.FirstOrDefault(x => x.Id == id);
    //        AllStudents = AllStudents.Where(x => x.Id != id).ToList();
    //        return result;
    //    }

    //}
}
