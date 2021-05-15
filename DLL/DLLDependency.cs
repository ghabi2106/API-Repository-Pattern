using DLL.DBContext;
using DLL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public static class DLLDependency
    {
        public static void AllDependency(IServiceCollection services, IConfiguration Configuration)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), serverVersion)
            //);

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            //services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            //services.AddTransient<IStudentRepository, StudentRepository>();
        }
    }
}
