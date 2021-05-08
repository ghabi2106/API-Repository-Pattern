using DLL.DBContext;
using DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Repositories
{
    public interface ICourseStudentRepository : IBaseRepository<CourseStudent>
    {
    }

    public class CourseStudentRepository : BaseRepository<CourseStudent>, ICourseStudentRepository
    {
        public CourseStudentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
