using DLL.DBContext;
using DLL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Repositories
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
    }
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }

    //public interface IDepartmentRepository : IBaseRepository<Department>
    //{
    //    Task<Department> CreateAsync(Department department);
    //    Task<List<Department>> GetAllAsync();
    //    Task<Department> FindAsync(string code);
    //    Task<bool> IsCodeExists(string code);
    //    Task<bool> IsNameExists(string name);
    //    Task<bool> UpdateAsync(Department department);
    //    Task<bool> DeleteAsync(Department department);
    //}
    //public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    //{
    //    public DepartmentRepository(ApplicationDbContext context) : base(context)
    //    {
    //    }
    //    private readonly ApplicationDbContext _context;
    //    public DepartmentRepository(ApplicationDbContext context)
    //    {
    //        _context = context;
    //    }
    //    public async Task<Department> CreateAsync(Department department)
    //    {
    //        await _context.Departments.AddAsync(department);
    //        await _context.SaveChangesAsync();
    //        return department;
    //    }

    //    public async Task<List<Department>> GetAllAsync()
    //    {
    //        return await _context.Departments.ToListAsync();
    //    }

    //    public async Task<Department> FindAsync(string code)
    //    {
    //        return await _context.Departments.FirstOrDefaultAsync(x => x.Code == code);
    //    }

    //    public async Task<bool> IsCodeExists(string code)
    //    {
    //        return await _context.Departments.AnyAsync(x => x.Code == code);
    //    }

    //    public async Task<bool> IsNameExists(string name)
    //    {
    //        return await _context.Departments.AnyAsync(x => x.Name == name);
    //    }
    //    public async Task<bool> DeleteAsync(Department department)
    //    {
    //        _context.Departments.Remove(department);
    //        if (await _context.SaveChangesAsync() > 0)
    //            return true;
    //        return false;
    //    }

    //    public async Task<bool> UpdateAsync(Department department)
    //    {
    //        _context.Departments.Update(department);
    //        if (await _context.SaveChangesAsync() > 0)
    //            return true;
    //        return false;
    //    }
    //}
}
