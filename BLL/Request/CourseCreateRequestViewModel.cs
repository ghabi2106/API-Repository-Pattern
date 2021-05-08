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
    public class CourseCreateRequestViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Credit { get; set; }
    }

    public class CourseCreateRequestViewModelValidator : AbstractValidator<CourseCreateRequestViewModel>
    {
        private readonly IServiceProvider _serviceProvider;


        public CourseCreateRequestViewModelValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RuleFor(x => x.Name).NotNull()
                .NotEmpty().MinimumLength(4).MaximumLength(25).MustAsync(NameExists).WithMessage("name exists in our system");
            RuleFor(x => x.Code).NotNull()
                .NotEmpty().MinimumLength(3).MaximumLength(10).MustAsync(CodeExists).WithMessage("code exists in our system");
            RuleFor(x => x.Credit).NotEmpty().NotNull();

        }

        private async Task<bool> CodeExists(string code, CancellationToken arg2)
        {
            if (string.IsNullOrEmpty(code))
            {
                return true;
            }

            var requiredService = _serviceProvider.GetRequiredService<ICourseService>();

            return !await requiredService.IsCodeExists(code);
        }

        private async Task<bool> NameExists(string name, CancellationToken arg2)
        {
            if (string.IsNullOrEmpty(name))
            {
                return true;
            }

            var requiredService = _serviceProvider.GetRequiredService<ICourseService>();

            return !await requiredService.IsNameExists(name);
        }
    }
}
