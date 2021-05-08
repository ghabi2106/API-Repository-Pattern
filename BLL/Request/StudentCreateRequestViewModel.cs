using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Request
{
    public class StudentCreateRequestViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
    }
    public class StudentCreateRequestViewModelValidator : AbstractValidator<StudentCreateRequestViewModel>
    {
        private readonly IServiceProvider _serviceProvider;
        public StudentCreateRequestViewModelValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(3)
                .MustAsync(NameExists).WithMessage("Name already exists");
            RuleFor(x => x.Email).EmailAddress()
                .MustAsync(EmailExists).WithMessage("Email already exists");
            RuleFor(x => x.DepartmentId).GreaterThan(0)
                .MustAsync(DepartmentExists).WithMessage("Department not exists");
        }

        private async Task<bool> EmailExists(string email, CancellationToken arg2)
        {
            if (string.IsNullOrEmpty(email))
                return true;
            var requiredService = _serviceProvider.GetRequiredService<IStudentService>();
            return !await requiredService.IsEmailExists(email);
        }

        private async Task<bool> NameExists(string name, CancellationToken arg2)
        {
            if (string.IsNullOrEmpty(name))
                return true;
            var requiredService = _serviceProvider.GetRequiredService<IStudentService>();
            return !await requiredService.IsNameExists(name);
        }

        private async Task<bool> DepartmentExists(int id, CancellationToken arg2)
        {

            if (id == 0)
            {
                return true;
            }
            var requiredService = _serviceProvider.GetRequiredService<IDepartmentService>();

            return !await requiredService.IsIdExists(id);
        }
    }
}
