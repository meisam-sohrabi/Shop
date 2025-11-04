using ProductService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Query.ProductBrand;

namespace ProductService.Infrastructure.EntityFrameWorkCore.Repository.Query.ProductBrand
{
    public class ProductBrandQueryRepository : IProductBrandQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductBrandQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<ProductBrandEntity> GetQueryable()
        {
            return _context.ProductBrands.AsQueryable();
        }
    }
}
