using BLL.Request;
using BLL.Services;
using DLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class DepartmentController : MainApiController
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly IDepartmentService _departmentService;

        public DepartmentController(ILogger<DepartmentController> logger, IDepartmentService departmentService)
        {
            _logger = logger;
            _departmentService = departmentService;
        }

        // GET: DepartmentController
        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {
            return Ok(await _departmentService.GetAllAsync());
        }

        // GET: DepartmentController/Details/5
        [HttpGet(template:"{code}")]
        public async Task<ActionResult> DetailsAsync(string code)
        {
            return Ok(await _departmentService.FindAsync(code));
        }

        // POST: DepartmentController/Create
        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromForm] DepartmentCreateRequestViewModel department)
        {
            return Ok(await _departmentService.CreateAsync(department));
        }

        // PUT: DepartmentController/Edit/5
        [HttpPut(template:"{code}")]
        public async Task<ActionResult> EditAsync(string code, [FromForm]Department department)
        {
            return Ok(await _departmentService.UpdateAsync(code, department));
        }

        // DELETE: DepartmentController/Delete/5
        [HttpDelete(template: "{code}")]
        public async Task<ActionResult> DeleteAsync(string code)
        {
            return Ok(await _departmentService.DeleteAsync(code));
        }
    }

    //public static class DepartmentStatic {
    //    public static List<Department> AllDepartments = new List<Department>();
    //    public static Department CreateDepartment(Department department)
    //    {
    //        AllDepartments.Add(department);
    //        return department;
    //    }
    //    public static List<Department> GetAllDepartment()
    //    {
    //        return AllDepartments;
    //    }
    //    public static Department GetDepartment(int id)
    //    {
    //        return AllDepartments.FirstOrDefault(x=>x.Id == id);
    //    }
    //    public static Department UpdateDepartment(int id, Department department)
    //    {
    //        Department result = new Department();
    //        foreach(var adepartment in AllDepartments)
    //        {
    //            if(adepartment.Id == department.Id)
    //            {
    //                adepartment.Name = department.Name;
    //                result = adepartment;
    //            }
    //        }
    //        return result;
    //    }
    //    public static Department DeleteDepartment(int id)
    //    {
    //        var result = AllDepartments.FirstOrDefault(x => x.Id == id);
    //        AllDepartments = AllDepartments.Where(x => x.Id != id).ToList();
    //        return result;
    //    }

    //}
}
