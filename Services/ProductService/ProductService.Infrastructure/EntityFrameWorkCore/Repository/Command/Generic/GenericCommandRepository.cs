using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using ProductService.InfrastructureContract.Interfaces.Command.Generic;

namespace ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.Generic
{
    public class GenericCommandRepository<T> : IGenericCommandRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericCommandRepository(ApplicationDbContext context)
        {
            _context = context;
           _dbSet = _context.Set<T>();
        }
        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(List<T> entity)
        {
           await _dbSet.AddRangeAsync(entity);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            var entry = _dbSet.Entry(entity);
            var key = _dbSet.EntityType.Model.FindEntityType(typeof(T))?.FindPrimaryKey();
            if (key != null)
            {
                foreach (var property in key.Properties)
                {
                    entry.Property(property.Name).IsModified = false;
                }
            }
        }
    }
}
