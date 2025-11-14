using Microsoft.Extensions.DependencyInjection;
using ProductService.ApplicationContract.DTO.Permission;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.Generic;
using System.Text.Json;

namespace ProductService.Application.Services.Hand
{
    public static class PermissionHandler 
    {
        public static async Task HandleAsync(string message,IServiceProvider sp, CancellationToken cancellationToken)
        {
            using var scope = sp.CreateScope();

            var data = JsonSerializer.Deserialize<PermissionEventDto>(message);
            if (data == null)
            {
                throw new Exception("data is null");
            }
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var repoCmd = scope.ServiceProvider.GetRequiredService<IGenericCommandRepository<LocalPermissionEntity>>();

            var permission = new LocalPermissionEntity
            {
                Action = data.Action,
                Id = data.Id,
                Resource = data.Resource,
                Description = data.Description,
            };
            await repoCmd.AddAsync(permission);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
