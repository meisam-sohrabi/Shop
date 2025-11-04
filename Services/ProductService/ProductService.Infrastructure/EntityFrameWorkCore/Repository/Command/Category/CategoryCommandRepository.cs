using ProductService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.Generic;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Command.Category;

namespace ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.Category
{
    public class CategoryCommandRepository : GenericCommandRepository<CategoryEntity>,ICategoryCommandRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryCommandRepository(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }

        // other implimentations can be added here

    }
}
