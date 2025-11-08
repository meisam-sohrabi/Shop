using ProductService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Query.ProductDetail;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Query.Generic;

namespace ProductService.Infrastructure.EntityFrameWorkCore.Repository.Query.ProductDetail
{
    public class ProductDetailQueryRepository : GenericQueryRepository<ProductDetailEntity>,IProductDetailQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductDetailQueryRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
