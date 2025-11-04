using ProductService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Command.ProductBrand;

namespace ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.ProductBrand
{
    public class ProductBrandCommandRepository : IProductBrandCommandRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductBrandCommandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ProductBrandEntity productBrand)
        {
            _context.ProductBrands.Add(productBrand);
        }

        public void Delete(ProductBrandEntity productBrand)
        {
            _context.ProductBrands.Remove(productBrand);
        }

        public void Edit(ProductBrandEntity productBrand)
        {
            var entry = _context.Entry(productBrand);
            var key = _context.Model.FindEntityType(typeof(ProductEntity))?.FindPrimaryKey();
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
