using Microsoft.EntityFrameworkCore;
using AccountService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using AccountService.InfrastructureContract.Interfaces.Query.Generic;

namespace AccountService.Infrastructure.EntityFrameWorkCore.Repository.Query.Generic
{
    public class GenericQueryRepository<T> : IGenericQueryRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericQueryRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public virtual IQueryable<T> GetQueryable()
        {
             return _dbSet.AsQueryable();
        }
    }
}
