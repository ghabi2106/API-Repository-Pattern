using BLL.Request;
using BLL.Services;
using DLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
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

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("testing")]
        public async Task<IActionResult> Testing()
        {
            var loginUser = new RequestMaker()
            {
                Principal = User
            };
            await _courseService.Testing(loginUser);
            return Ok("test");
        }


        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "admin")]
        [HttpGet("testing1")]
        public async Task<IActionResult> Testing1()
        {
            var loginUser = new RequestMaker()
            {
                Principal = User
            };

            return Ok("test1");
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "manager")]
        [HttpGet("testing2")]
        public async Task<IActionResult> Testing2()
        {
            var loginUser = new RequestMaker()
            {
                Principal = User
            };

            return Ok("test2");
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "supervisor")]
        [HttpGet("testing3")]
        public async Task<IActionResult> Testing3()
        {
            var loginUser = new RequestMaker()
            {
                Principal = User
            };

            return Ok("test3");
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "admin,manager")]
        [HttpGet("testing4")]
        public async Task<IActionResult> Testing4()
        {
            var loginUser = new RequestMaker()
            {
                Principal = User
            };

            return Ok("test4");
        }

    }
}
