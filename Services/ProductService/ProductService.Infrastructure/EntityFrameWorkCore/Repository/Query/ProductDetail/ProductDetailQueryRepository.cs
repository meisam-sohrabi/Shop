using ProductService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Query.ProductDetail;

namespace ProductService.Infrastructure.EntityFrameWorkCore.Repository.Query.ProductDetail
{
    public class ProductDetailQueryRepository : IProductDetailQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductDetailQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<ProductDetailEntity> GetQueryable()
        {
            return _context.ProductDetails.AsQueryable();
        }
    }
}
