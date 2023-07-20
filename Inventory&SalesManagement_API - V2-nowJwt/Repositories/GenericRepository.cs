using DatabaseModels.DatabaseContext;
using DatabaseModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class GenericRepository<T>:IGenericRepository<T> where T : class,new()
    {
        protected readonly Inventory_Sales_DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(Inventory_Sales_DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        //public async Task<IEnumerable<T>> GetAllNotDeletedProduct(DbSet<T> dbSet)
        //{
        //    var data = await dbSet
        //                .Where(x => x.IsDeleted == false)
        //                .ToListAsync();

        //    return data;
        //}

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }
    }
}
