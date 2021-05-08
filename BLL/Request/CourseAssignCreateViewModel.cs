using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Request
{
    public class CourseAssignCreateViewModel
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }

    public class CourseAssignCreateViewModelValidator : AbstractValidator<CourseAssignCreateViewModel>
    {
        private readonly IServiceProvider _serviceProvider;


        public CourseAssignCreateViewModelValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RuleFor(x => x.CourseId).NotNull()
                .NotEmpty().MustAsync(CourseIdExists).WithMessage("course not exits in our system");
            RuleFor(x => x.StudentId).NotNull()
                .NotEmpty().MustAsync(StudentIdExists).WithMessage("student id not exists in our system");

        }

        private async Task<bool> CourseIdExists(int courseId, CancellationToken arg2)
        {


            var requiredService = _serviceProvider.GetRequiredService<ICourseService>();

            return !await requiredService.IsIdExists(courseId);



        }

        private async Task<bool> StudentIdExists(int studentId, CancellationToken arg2)
        {


            var requiredService = _serviceProvider.GetRequiredService<IStudentService>();

            return !await requiredService.IsIdExists(studentId);
        }
    }
}
