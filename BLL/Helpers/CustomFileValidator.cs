using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Resources;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Http;

namespace BLL.Helpers
{
    public class CustomFileValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        private readonly IFileValidate _fileValidate;

        public CustomFileValidator(IFileValidate fileValidate)
        {
            _fileValidate = fileValidate;
        }

        public override string Name => "CustomFileValidator";
        protected override string GetDefaultMessageTemplate(string errorCode) 
            => "{PropertyName} must be a valid image type.";

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            var fileToValidate = value as IFormFile;


            var (valid, errorMessage) = _fileValidate.ValidateFile(fileToValidate);

            if (valid) return true;
            context.MessageFormatter.AppendArgument("ErrorMessage", errorMessage);
            return false;
        }
    }
}
