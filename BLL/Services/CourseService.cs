using BLL.Request;
using DLL.Models;
using DLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface ICourseService
    {
        Task<Course> CreateAsync(CourseCreateRequestViewModel request);
        Task<List<Course>> GetAllAsync();
        Task<Course> UpdateAsync(string code, Course department);
        Task<Course> DeleteAsync(string code);
        Task<Course> FindAsync(string code);

        Task<bool> IsCodeExists(string code);
        Task<bool> IsNameExists(string name);
        Task<bool> IsIdExists(int id);

    }

    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;


        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<Course> CreateAsync(CourseCreateRequestViewModel request)
        {
            var course = new Course();
            course.Code = request.Code;
            course.Name = request.Name;
            course.Credit = request.Credit;
            await _unitOfWork.CourseRepository.CreateAsync(course);

            if (await _unitOfWork.SaveAsync())
            {
                return course;
            }

            throw new ApplicationValidationException("course insert has some problem");
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _unitOfWork.CourseRepository.GetList();
        }

        public async Task<Course> UpdateAsync(string code, Course aCourse)
        {
            var course = await _unitOfWork.CourseRepository.FindAsync(x => x.Code == code);

            if (course == null)
            {
                throw new ApplicationValidationException("course not found");
            }

            if (!string.IsNullOrWhiteSpace(aCourse.Code))
            {
                var existsAlreadyCode = await _unitOfWork.CourseRepository.FindAsync(x => x.Code == code);
                if (existsAlreadyCode != null)
                {
                    throw new ApplicationValidationException("your updated code already present in our system");
                }

                course.Code = aCourse.Code;
            }

            if (!string.IsNullOrWhiteSpace(aCourse.Name))
            {
                var existsAlreadyCode = await _unitOfWork.CourseRepository.FindAsync(x => x.Name == aCourse.Name);
                if (existsAlreadyCode != null)
                {
                    throw new ApplicationValidationException("your updated name already present in our system");
                }

                course.Name = aCourse.Name;
            }

            _unitOfWork.CourseRepository.Update(course);
            if (await _unitOfWork.SaveAsync())
            {
                return course;
            }

            throw new ApplicationValidationException("in update have some problem");
        }

        public async Task<Course> DeleteAsync(string code)
        {
            var course = await _unitOfWork.CourseRepository.FindAsync(x => x.Code == code);

            if (course == null)
            {
                throw new ApplicationValidationException("course not found");
            }

            _unitOfWork.CourseRepository.Delete(course);
            if (await _unitOfWork.SaveAsync())
            {
                return course;
            }

            throw new ApplicationValidationException("some problem for delete data");
        }

        public async Task<Course> FindAsync(string code)
        {
            var course = await _unitOfWork.CourseRepository.FindAsync(x => x.Code == code);

            if (course == null)
            {
                throw new ApplicationValidationException("course not found");
            }

            return course;
        }

        public async Task<bool> IsCodeExists(string code)
        {
            return await _unitOfWork.CourseRepository.Exists(x => x.Code == code);
        }

        public async Task<bool> IsNameExists(string name)
        {
            return await _unitOfWork.CourseRepository.Exists(x => x.Name == name);
        }

        public async Task<bool> IsIdExists(int id)
        {
            return await _unitOfWork.CourseRepository.Exists(x => x.Id == id);
        }


    }
}
