using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Infrastructure.Persistence.Repositories.Generic
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<Entity> _dbSet;

        public GenericRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Entity>();

        }

        public virtual async Task<Entity> AddAsync(Entity entity)
        {
            await _dbContext.Set<Entity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(Entity entity, int id)
        {
            var entry = await _dbContext.Set<Entity>().FindAsync(id);
            _dbContext.Entry(entry).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteINTAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Entity with id {id} not found");

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        public virtual async Task DeleteAsync(Entity entity)
        {
            _dbContext.Set<Entity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<List<Entity>> GetAllAsync()
        {
            return await _dbContext.Set<Entity>().ToListAsync();//Deferred execution
        }

        public virtual async Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties)
        {
            var query = _dbContext.Set<Entity>().AsQueryable();

            foreach (string property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<Entity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Entity>().FindAsync(id);
        }

        public async Task<List<Entity>> AddRangeAsync(List<Entity> entities)
        {
            await _dbContext.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
            return entities;
        }
        public IQueryable<Entity> GetAllAsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<bool> ExistsAsync(Expression<Func<Entity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

    }
}
