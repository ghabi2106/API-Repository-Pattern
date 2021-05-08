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
    public interface IStudentService
    {
        Task<Student> CreateAsync(StudentCreateRequestViewModel studentRequest);
        IQueryable<Student> GetAll();
        Task<Student> UpdateAsync(string email, Student student);
        Task<Student> DeleteAsync(string email);
        Task<Student> FindAsync(string email);
        Task<bool> IsEmailExists(string email);
        Task<bool> IsNameExists(string email);
        Task<bool> IsIdExists(int id);
    }

    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;


        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Student> CreateAsync(StudentCreateRequestViewModel studentRequest)
        {
            var student = new Student()
            {
                Email = studentRequest.Email,
                Name = studentRequest.Name,
                DepartmentId = studentRequest.DepartmentId
            };
            await _unitOfWork.StudentRepository.CreateAsync(student);
            if (await _unitOfWork.SaveAsync())
            {
                return student;
            }

            throw new ApplicationValidationException("insert has some problem");
        }

        public IQueryable<Student> GetAll()
        {
            return _unitOfWork.StudentRepository.QueryAll();
        }

        public async Task<Student> UpdateAsync(string email, Student student)
        {
            var dbStudent = await _unitOfWork.StudentRepository.FindAsync(x => x.Email == email);

            if (dbStudent == null)
            {
                throw new ApplicationValidationException("student not found");
            }

            dbStudent.Name = student.Name;
            _unitOfWork.StudentRepository.Update(dbStudent);
            if (await _unitOfWork.SaveAsync())
            {
                return dbStudent;
            }

            throw new ApplicationValidationException("update has some issue");
        }

        public async Task<Student> DeleteAsync(string email)
        {
            var dbStudent = await _unitOfWork.StudentRepository.FindAsync(x => x.Email == email);

            if (dbStudent == null)
            {
                throw new ApplicationValidationException("student not found");
            }


            _unitOfWork.StudentRepository.Delete(dbStudent);
            if (await _unitOfWork.SaveAsync())
            {
                return dbStudent;
            }

            throw new ApplicationValidationException("delete has some issue");
        }

        public async Task<Student> FindAsync(string email)
        {
            return await _unitOfWork.StudentRepository.FindAsync(x => x.Email == email);
        }

        public async Task<bool> IsEmailExists(string email)
        {
            return await _unitOfWork.StudentRepository.Exists(x => x.Email == email);
        }

        public async Task<bool> IsNameExists(string name)
        {
            return await _unitOfWork.StudentRepository.Exists(x => x.Name == name);
        }

        public async Task<bool> IsIdExists(int id)
        {
            return await _unitOfWork.StudentRepository.Exists(x => x.Id == id);
        }
    }
    //public interface IStudentService
    //{
    //    Task<Student> CreateAsync(StudentCreateRequestViewModel request);
    //    Task<List<Student>> GetAllAsync();
    //    Task<Student> FindAsync(string email);
    //    Task<bool> IsEmailExists(string email);
    //    Task<bool> IsNameExists(string name);
    //    Task<Student> UpdateAsync(string email, Student student);
    //    Task<Student> DeleteAsync(string email);
    //    Task<bool> IsIdExists(int id);
    //}
    //public class StudentService : IStudentService
    //{
    //    private readonly IStudentRepository _studentRepository;
    //    public StudentService(IStudentRepository studentRepository)
    //    {
    //        _studentRepository = studentRepository;
    //    }
    //    public async Task<Student> CreateAsync(StudentCreateRequestViewModel request)
    //    {
    //        Student student = new Student()
    //        {
    //            Email = request.Email,
    //            Name = request.Name
    //        };
    //        await _studentRepository.CreateAsync(student);
    //        if (await _studentRepository.SaveAsync())
    //            return student;
    //        throw new ApplicationValidationException("Some problem with delete data");
    //    }

    //    public async Task<List<Student>> GetAllAsync()
    //    {
    //        return await _studentRepository.GetList();
    //    }

    //    public async Task<Student> FindAsync(string email)
    //    {
    //        return await _studentRepository.FindAsync(x => x.Email == email) ??
    //            throw new ApplicationValidationException("Student not found");
    //    }

    //    public async Task<bool> IsEmailExists(string email)
    //    {
    //        return await _studentRepository.Exists(x => x.Email == email);
    //    }

    //    public async Task<bool> IsNameExists(string name)
    //    {
    //        return await _studentRepository.Exists(x => x.Name == name);
    //    }
    //    public async Task<Student> DeleteAsync(string email)
    //    {
    //        Student student = await _studentRepository.FindAsync(x => x.Email == email);
    //        if (student == null)
    //            throw new ApplicationValidationException("Student not found");
    //        _studentRepository.Delete(student);
    //        if (await _studentRepository.SaveAsync())
    //            return student;
    //        throw new ApplicationValidationException("Some problem with delete data");
    //    }

    //    public async Task<Student> UpdateAsync(string email, Student student)
    //    {
    //        Student oldStudent = await _studentRepository.FindAsync(x => x.Email == email);
    //        if (oldStudent == null)
    //            throw new ApplicationValidationException("Student not found");
    //        if (!string.IsNullOrWhiteSpace(student.Email))
    //        {
    //            if (await _studentRepository.Exists(x => x.Email == student.Email) && student.Email != email)
    //                throw new ApplicationValidationException("The email already exists");
    //            oldStudent.Email = student.Email;
    //        }
    //        if (!string.IsNullOrWhiteSpace(student.Email))
    //        {
    //            if (await _studentRepository.Exists(x => x.Name == student.Name) && student.Name != oldStudent.Name)
    //                throw new ApplicationValidationException("The name already exists");
    //            oldStudent.Name = student.Name;
    //        }
    //        _studentRepository.Update(oldStudent);
    //        if (await _studentRepository.SaveAsync())
    //            return oldStudent;
    //        throw new ApplicationValidationException("Some problem with delete data");
    //    }
    //}
}
