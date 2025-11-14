using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductService.ApplicationContract.DTO.ProductDetail;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.ProductDetail;
using ProductService.InfrastructureContract.Interfaces.Query.ProductDetail;
using System.Text.Json;

namespace ProductService.Application.Services.Hand
{
    public static class ProductHandler
    {
        public static async Task HandleAsync(string message, IServiceProvider sp, CancellationToken cancellationToken)
        {
            using var scope = sp.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var repoCmd = scope.ServiceProvider.GetRequiredService<IProductDetailCommandRepository>();
            var repoQry = scope.ServiceProvider.GetRequiredService<IProductDetailQueryRepository>();
            var data = JsonSerializer.Deserialize<ProductDetailEventDto>(message);
            if (data == null)
            {
                throw new Exception("data is null");
            }
            var detail = await repoQry.GetQueryable().FirstOrDefaultAsync(c => c.Id == data.Id);
            if (detail == null)
            {
                throw new Exception("Product Detail not found.");
            }

            detail.Quantity = data.Quantity;
            repoCmd.Edit(detail);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
