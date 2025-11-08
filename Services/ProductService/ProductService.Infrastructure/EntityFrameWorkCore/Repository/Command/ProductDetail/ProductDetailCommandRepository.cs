using ProductService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Command.ProductDetail;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.Generic;

namespace ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.ProductDetail
{
    public class ProductDetailCommandRepository : GenericCommandRepository<ProductDetailEntity>,IProductDetailCommandRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductDetailCommandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

       
    }
}
