using ProductService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Query.Generic;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Query.Category;

namespace ProductService.Infrastructure.EntityFrameWorkCore.Repository.Query.Category
{
    public class CategoryQueryRepository : GenericQueryRepository<CategoryEntity>,ICategoryQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryQueryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


    }
}
