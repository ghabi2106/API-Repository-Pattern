using DLL.DBContext;
using DLL.Models;
using DLL.ResponseViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Repositories
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<StudentCourseViewModel> GetStudentWithCoursesAsync(int studentId);
    }
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<StudentCourseViewModel> GetStudentWithCoursesAsync(int studentId)
        {
            return await _context.Students
                .Include(x => x.CourseStudents).ThenInclude(x => x.Course)
                .Select(x => new StudentCourseViewModel()
                {
                    StudentId = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Courses = x.CourseStudents.Select(x => x.Course).ToList()
                })
                .FirstOrDefaultAsync(x => x.StudentId == studentId);
        }
    }

    //public interface IStudentRepository : IBaseRepository<Student>
    //{
    //    Task<StudentCourseViewModel> GetSpecificStudentCourseListAsync(int studentId);
    //    Task<Student> CreateAsync(Student student);
    //    Task<List<Student>> GetAllAsync();
    //    Task<Student> FindAsync(string email);
    //    Task<bool> IsEmailExists(string email);
    //    Task<bool> IsNameExists(string name);
    //    Task<Student> UpdateAsync(string email, Student student);
    //    Task<Student> DeleteAsync(string email);
    //}
    //public class StudentRepository : BaseRepository<Student>, IStudentRepository
    //{
    //    private readonly ApplicationDbContext _context;

    //    public StudentRepository(ApplicationDbContext context) : base(context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<StudentCourseViewModel> GetSpecificStudentCourseListAsync(int studentId)
    //    {
    //        return await _context.Students
    //            .Include(x => x.CourseStudents).ThenInclude(x => x.Course)
    //            .Select(x => new StudentCourseViewModel()
    //            {
    //                StudentId = x.Id,
    //                Name = x.Name,
    //                Email = x.Email,
    //                Courses = x.CourseStudents.Select(x => x.Course).ToList()
    //            })
    //            .FirstOrDefaultAsync(x => x.StudentId == studentId);
    //    }
    //    private readonly ApplicationDbContext _context;
    //    public StudentRepository(ApplicationDbContext context)
    //    {
    //        _context = context;
    //    }
    //    public async Task<Student> CreateAsync(Student student)
    //    {
    //        await _context.Students.AddAsync(student);
    //        await _context.SaveChangesAsync();
    //        return student;
    //    }

    //    public async Task<List<Student>> GetAllAsync()
    //    {
    //        return await _context.Students.ToListAsync();
    //    }

    //    public async Task<Student> FindAsync(string email)
    //    {
    //        return await _context.Students.FirstOrDefaultAsync(x => x.Email == email);
    //    }

    //    public async Task<Student> FindByEmailAsync(string email)
    //    {
    //        return await _context.Students.FirstOrDefaultAsync(x => x.Email == email);
    //    }

    //    public async Task<bool> IsNameExists(string name)
    //    {
    //        return await _context.Students.AnyAsync(x => x.Name == name);
    //    }

    //    public async Task<bool> IsEmailExists(string email)
    //    {
    //        return await _context.Students.AnyAsync(x => x.Email == email);
    //    }

    //    public async Task<Student> DeleteAsync(string email)
    //    {
    //        Student student = await _context.Students.FirstOrDefaultAsync(x => x.Email == email);
    //        _context.Students.Remove(student);
    //        await _context.SaveChangesAsync();
    //        return student;
    //    }

    //    public async Task<Student> UpdateAsync(string email, Student student)
    //    {
    //        Student oldStudent = await _context.Students.FirstOrDefaultAsync(x => x.Email == email);
    //        oldStudent.Name = student.Name;
    //        oldStudent.Email = student.Email;
    //        _context.Students.Update(oldStudent);
    //        return student;
    //    }
    //}
}
