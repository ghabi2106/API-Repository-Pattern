using BLL.Request;
using DLL.Models;
using DLL.Repositories;
using DLL.ResponseViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Exceptions;
using Utility.Models;

namespace BLL.Services
{
    public interface ICourseStudentService
    {
        Task<ApiSuccessResponse> CreateAsync(CourseAssignCreateViewModel request);

        Task<StudentCourseViewModel> CourseListAsync(int studentId);
    }

    public class CourseStudentService : ICourseStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseStudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiSuccessResponse> CreateAsync(CourseAssignCreateViewModel request)
        {
            var isStudentAlreadyEntroll = await _unitOfWork.CourseStudentRepository.Exists(x =>
                x.CourseId == request.CourseId &&
                x.StudentId == request.StudentId);

            if (isStudentAlreadyEntroll)
            {
                throw new ApplicationValidationException("this student already enroll this course");
            }

            var courseStudent = new CourseStudent()
            {
                CourseId = request.CourseId,
                StudentId = request.StudentId
            };

            await _unitOfWork.CourseStudentRepository.CreateAsync(courseStudent);

            if (await _unitOfWork.SaveAsync())
            {
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "student enroll successfully"
                };
            }
            throw new ApplicationValidationException("something wrong for enrollment");
        }

        public async Task<StudentCourseViewModel> CourseListAsync(int studentId)
        {
            return await _unitOfWork.StudentRepository.GetStudentWithCoursesAsync(studentId);
        }
    }
}
