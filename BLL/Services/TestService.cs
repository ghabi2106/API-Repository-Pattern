using Bogus;
using DLL.DBContext;
using DLL.Models;
using DLL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface ITestService
    {
        Task InsertData();
        Task DummyData1();
        Task DummyData2();
        Task AddNewRoles();
        Task AddNewUser();
        Task CreateAndroidAndWebClient();
    }

    public class TestService : ITestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOpenIddictApplicationManager _openIddictApplicationManager;


        public TestService(IUnitOfWork unitOfWork,
            ApplicationDbContext context, RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IOpenIddictApplicationManager openIddictApplicationManager
            )
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _openIddictApplicationManager = openIddictApplicationManager;
        }

        public async Task InsertData()
        {
            var department = new Department()
            {
                Code = "arts",
                Name = "art department"
            };

            var student = new Student()
            {
                Email = "art@gmail.com",
                Name = "mr arts"
            };

            await _unitOfWork.DepartmentRepository.CreateAsync(department);
            await _unitOfWork.StudentRepository.CreateAsync(student);

            await _unitOfWork.SaveAsync();
        }

        public async Task DummyData1()
        {
            var StudentDummy = new Faker<Student>()
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name));
            var departmentDummy = new Faker<Department>()
                .RuleFor(o => o.Name, f => f.Name.FirstName())
                .RuleFor(o => o.Code, f => f.Name.LastName())
                .RuleFor(u => u.Students, f => StudentDummy.Generate(50).ToList());

            var departmentListWithStudent = departmentDummy.Generate(100).ToList();
            await _context.Departments.AddRangeAsync(departmentListWithStudent);

            await _context.SaveChangesAsync();
        }

        public async Task DummyData2()
        {
            // var courseDummy = new Faker<Course>()
            //     .RuleFor(o => o.Name, f => f.Name.FirstName())
            //     .RuleFor(o => o.Code, f => f.Name.LastName())
            //     .RuleFor(u => u.Credit, f => f.Random.Number(1, 10));
            // var courseDummyList = courseDummy.Generate(50).ToList();
            // await _context.Courses.AddRangeAsync(courseDummyList);
            // await _context.SaveChangesAsync();


            var studentIds = await _context.Students.Select(x => x.Id).ToListAsync();
            var allCourseId = await _context.Courses.Select(x => x.Id).ToListAsync();
            int count = 0;
            foreach (var course in allCourseId)
            {
                var courseStudent = new List<CourseStudent>();
                var students = studentIds.Skip(count).Take(5).ToList();
                foreach (var aStudent in students)
                {
                    courseStudent.Add(new CourseStudent()
                    {
                        CourseId = course,
                        StudentId = aStudent
                    });
                }

                await _context.CourseStudents.AddRangeAsync(courseStudent);
                await _context.SaveChangesAsync();
                count += 5;
            }
        }

        public async Task AddNewRoles()
        {
            var roleList = new List<string>()
            {
                "admin",
                "manager",
                "supervisor"
            };

            foreach (var role in roleList)
            {
                var exits = await _roleManager.FindByNameAsync(role);

                if (exits == null)
                {
                    await _roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = role
                    });
                }
            }
        }

        public async Task AddNewUser()
        {
            var userList = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    UserName = "assaad.ghabi@gmail.com",
                    Email = "assaad.ghabi@gmail.com",
                    FullName = "Assaad Ghabi"
                },
                new ApplicationUser()
                {
                    UserName = "assaad@gmail.com",
                    Email = "assaad@gmail.com",
                    FullName = "Assaad"
                },
                new ApplicationUser()
                {
                    UserName = "ghabi@gmail.com",
                    Email = "ghabi@gmail.com",
                    FullName = "Ghabi"
                },
            };

            foreach (var user in userList)
            {
                var userExits = await _userManager.FindByEmailAsync(user.Email);

                if (userExits == null)
                {
                    var insertedData = await _userManager.CreateAsync(user, "abc123$..A!");

                    if (insertedData.Succeeded)
                    {
                        var myRole = "";
                        if (user.Email == "assaad.ghabi@gmail.com")
                        {
                            myRole = "admin";
                        }
                        else if (user.Email == "assaad@gmail.com")
                        {
                            myRole = "manager";
                        }
                        else if (user.Email == "ghabi@gmail.com")
                        {
                            myRole = "supervisor";
                        }

                        await _userManager.AddToRoleAsync(user, myRole);
                    }
                }
            }
        }

        public async Task CreateAndroidAndWebClient()
        {
            var listOfClient = new List<OpenIddictApplicationDescriptor>()
            {
                new OpenIddictApplicationDescriptor()
                {
                    ClientId = "test_android_application",
                    ClientSecret = "test123",
                    DisplayName = "our android client",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Logout,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles,
                    }
                },
                new OpenIddictApplicationDescriptor()
                {
                    ClientId = "test_web_application",
                    ClientSecret = "test456",
                    DisplayName = "our web application client",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Logout,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles,
                    }
                }
            };

            foreach (var application in listOfClient)
            {
                var applicationExists = await _openIddictApplicationManager.FindByClientIdAsync(application.ClientId);

                if (applicationExists == null)
                {
                    await _openIddictApplicationManager.CreateAsync(application);
                }
            }
        }
    }
}
