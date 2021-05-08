using BLL.Request;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class CourseEntrollController : MainApiController
    {
        private readonly ICourseStudentService _courseStudentService;

        public CourseEntrollController(ICourseStudentService courseStudentService)
        {
            _courseStudentService = courseStudentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseAssignCreateViewModel request)
        {

            return Ok(await _courseStudentService.CreateAsync(request));
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> CourseList(int studentId)
        {

            return Ok(await _courseStudentService.CourseListAsync(studentId));
        }
    }
}
