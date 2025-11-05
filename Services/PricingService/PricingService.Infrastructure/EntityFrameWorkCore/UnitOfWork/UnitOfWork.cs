using PricingService.InfrastructureContract.Interfaces;
using PricingService.Infrastructure.EntityFrameWorkCore.AppDbContext;

namespace PricingService.Infrastructure.EntityFrameWorkCore.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _context.SaveChangesAsync();
        }
        private int getCurrentUserId()
        {
            return 0;
        }
        public void  BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveCommit()
        {
            await _context.Database.CommitTransactionAsync();
        }


        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollBackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
