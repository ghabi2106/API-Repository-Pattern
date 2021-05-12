using BLL.Request;
using BLL.Services;
using DLL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class CourseController : MainApiController
    {
        private readonly ICourseService _courseService;


        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _courseService.GetAllAsync());
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Find(string code)
        {
            return Ok(await _courseService.FindAsync(code));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CourseCreateRequestViewModel request)
        {
            return Ok(await _courseService.CreateAsync(request));
        }

        [HttpPut("{code}")]
        public async Task<IActionResult> Update(string code, Course course)
        {
            return Ok(await _courseService.UpdateAsync(code, course));
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            return Ok(await _courseService.DeleteAsync(code));
        }

    }
}
