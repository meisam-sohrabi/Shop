using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Command.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ProductService.Infrastructure.EntityFrameWorkCore.AppDbContext;

namespace ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.Product
{
    public class ProductCommandRepository : IProductCommandRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductCommandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Add
        public void Add(ProductEntity product)
        {
            _context.Products.Add(product);
        }
        #endregion

        #region Delete
        public void Delete(ProductEntity product)
        {
            _context.Remove(product);
        }
        #endregion

        #region Edit
        public void Edit(ProductEntity product)
        {
            var entry = _context.Entry(product);
            var key = _context.Model.FindEntityType(typeof(ProductEntity))?.FindPrimaryKey();
            if (key != null)
            {
                foreach (var property in key.Properties)
                {
                    entry.Property(property.Name).IsModified = false;
                }
            }
        }

        #endregion

        #region EditAToPUsingSP
        public async Task EditArabicToPersianSP(DateTime start,DateTime end)
        {
            var pStart = new SqlParameter("@start", start);
            var pEnd = new SqlParameter("@end", end);
            await _context.Database.ExecuteSqlRawAsync("EXEC ProductArabicRevision @start,@end",pStart,pEnd);
        }
        #endregion

    }
}
