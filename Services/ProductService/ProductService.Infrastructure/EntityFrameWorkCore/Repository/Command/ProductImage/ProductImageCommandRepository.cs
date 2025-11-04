using ProductService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.Generic;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Command.ProductImage;

namespace ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.ProductImage
{
    public class ProductImageCommandRepository : GenericCommandRepository<ProductImageEntity>, IProductImageCommandRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductImageCommandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // other implimentation here also can be added.
    }
}
