using Dapper;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using ProductService.ApplicationContract.DTO.Product;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Query.Product;
using System.Data;
namespace ProductService.Infrastructure.EntityFrameWorkCore.Repository.Query.Product
{
    public class ProductQueryRepository : IProductQueryRespository
    {
        private readonly ApplicationDbContext _context;

        public ProductQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        #region GetByParametersThroughSP
        public async Task<List<ProductWithInventoryDto>> GetProductsByDateAndTextAsync(string? textSearch, DateTime? startDate, DateTime? endDate)
        {
            var procedureName = "GetProductByDateAndTextSearch";
            var parameters = new { textSearch, startDate, endDate };
            using (var connection = _context.Database.GetDbConnection())
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                var affectedRows = await connection.QueryAsync<ProductWithInventoryDto>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                return affectedRows.ToList();
            }

        }
        #endregion



        #region Get
        public IQueryable<ProductEntity> GetQueryable()
        {
            return _context.Products.AsQueryable();
        }
        #endregion

    }
}
