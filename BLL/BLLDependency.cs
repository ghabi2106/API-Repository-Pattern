using BLL.Helpers;
using BLL.Request;
using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class BLLDependency
    {
        public static void AllDependency(IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<ITestService, TestService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ICourseStudentService, CourseStudentService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IFileValidate, FileValidate>();

            AllFluentValidationDependency(services);
        }

        private static void AllFluentValidationDependency(IServiceCollection services)
        {
            services.AddTransient<IValidator<DepartmentCreateRequestViewModel>, DepartmentCreateRequestViewModelValidator>();
            services.AddTransient<IValidator<StudentCreateRequestViewModel>, StudentCreateRequestViewModelValidator>();
            services.AddTransient<IValidator<CourseCreateRequestViewModel>, CourseCreateRequestViewModelValidator>();
            services.AddTransient<IValidator<CourseAssignCreateViewModel>, CourseAssignCreateViewModelValidator>();
        }
    }
}
