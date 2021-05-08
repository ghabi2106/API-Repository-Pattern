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
    public class DepartmentCreateRequestViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class DepartmentCreateRequestViewModelValidator : AbstractValidator<DepartmentCreateRequestViewModel>
    {
        private readonly IServiceProvider _serviceProvider;
        public DepartmentCreateRequestViewModelValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(25)
                .MustAsync(NameExists).WithMessage("Name already exists"); ;
            RuleFor(x => x.Code).NotNull().NotEmpty().MinimumLength(3).MaximumLength(25)
                .MustAsync(CodeExists).WithMessage("Code already exists"); ;
        }

        private async Task<bool> CodeExists(string code, CancellationToken arg2)
        {
            if (string.IsNullOrEmpty(code))
                return true;
            var requiredService = _serviceProvider.GetRequiredService<IDepartmentService>();
            return !await requiredService.IsCodeExists(code);
        }

        private async Task<bool> NameExists(string name, CancellationToken arg2)
        {
            if (string.IsNullOrEmpty(name))
                return true;
            var requiredService = _serviceProvider.GetRequiredService<IDepartmentService>();
            return !await requiredService.IsNameExists(name);
        }
    }
}
