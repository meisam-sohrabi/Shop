using Microsoft.Extensions.DependencyInjection;
using ProductService.ApplicationContract.DTO.UserPermission;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.Generic;
using System.Text.Json;

namespace ProductService.Application.Services.Hand
{
    public class PermissionAsignHandler
    {
        public static async Task HandleAsync(string message, IServiceProvider sp, CancellationToken cancellationToken)
        {
            using var scope = sp.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var repoCmd = scope.ServiceProvider.GetRequiredService<IGenericCommandRepository<LocalUserPermissionEntity>>();
            var data = JsonSerializer.Deserialize<UserPermissionDto>(message);
            if (data == null)
            {
                throw new Exception("data is null");
            }
            var userPermission = new LocalUserPermissionEntity
            {
                UserId = data.UserId,
                PermissionId = data.PermissionId
            };
            await repoCmd.AddAsync(userPermission);
            await unitOfWork.SaveChangesAsync();
        }

    }
}
