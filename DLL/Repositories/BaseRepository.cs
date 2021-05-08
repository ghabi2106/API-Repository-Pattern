using DLL.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Repositories
{
    public interface IBaseRepository<T> where T:class
    {
        IQueryable<T> QueryAll(Expression<Func<T, bool>> expression = null);
        Task<List<T>> GetList(Expression<Func<T, bool>> expression = null);
        Task CreateAsync(T entry);
        Task CreateRangeAsync(List<T> entryList);
        void Update(T entry);
        void UpdateRange(List<T> entryList);
        void Delete(T entry);
        void DeleteRange(List<T> entryList);
        Task<T> FindAsync(Expression<Func<T, bool>> expression);
        Task<bool> SaveAsync();
        Task<bool> Exists(Expression<Func<T, bool>> expression);
    }

    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(T entry)
        {
            await _context.Set<T>().AddAsync(entry);
        }
        public async Task CreateRangeAsync(List<T> entryList)
        {
            await _context.Set<T>().AddRangeAsync(entryList);
        }

        public void Delete(T entry)
        {
            _context.Set<T>().Remove(entry);
        }

        public void DeleteRange(List<T> entryList)
        {
            _context.Set<T>().RemoveRange(entryList);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<List<T>> GetList(Expression<Func<T, bool>> expression = null)
        {
            return expression != null ? 
                await _context.Set<T>().AsQueryable().Where(expression).AsNoTracking().ToListAsync() :
                await _context.Set<T>().AsQueryable().AsNoTracking().ToListAsync();
        }

        public IQueryable<T> QueryAll(Expression<Func<T, bool>> expression = null)
        {
            return expression != null ? _context.Set<T>().AsQueryable().Where(expression).AsNoTracking() :
                _context.Set<T>().AsQueryable().AsNoTracking();
        }

        public void Update(T entry)
        {
            _context.Set<T>().Update(entry);
        }

        public void UpdateRange(List<T> entryList)
        {
            _context.Set<T>().UpdateRange(entryList);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<bool> Exists(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().AnyAsync(expression);
        }
    }
}
