using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Utility.Exceptions;
using Utility.Models;
using Newtonsoft.Json;

namespace WebApplication1.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;
        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            _next = next;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
            catch (Exception exception) {
                await HandleExceptionAsync(context, exception, _environment);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, IWebHostEnvironment environment)
        {
            var code = HttpStatusCode.InternalServerError;
            var errors = new ApiErrorResponse() { 
                StatusCode = (int)code
            };
            if (environment.IsDevelopment())
            {
                errors.Details = exception.StackTrace;
            } else
            {
                errors.Details = exception.Message;
            }
            switch (exception)
            {
                case ApplicationValidationException e:
                    errors.Message = e.Message;
                    errors.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    break;
                default:
                    errors.Message = "Something went wrong";
                    break;
            }
            var result = JsonConvert.SerializeObject(errors);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errors.StatusCode;
            await context.Response.WriteAsync(result);
        }
    }
}
