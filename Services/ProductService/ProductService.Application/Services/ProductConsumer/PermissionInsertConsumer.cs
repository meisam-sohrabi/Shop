using BaseConfig;
using MassTransit;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.Generic;

namespace ProductService.Application.Services.ProductConsumer
{
    public class PermissionInsertConsumer : IConsumer<PermissionEventDto>
    {
        private readonly IGenericCommandRepository<LocalPermissionEntity> _genericCommandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionInsertConsumer(IGenericCommandRepository<LocalPermissionEntity> genericCommandRepository,IUnitOfWork unitOfWork)
        {
            _genericCommandRepository = genericCommandRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Consume(ConsumeContext<PermissionEventDto> context)
        {
            var permission = new LocalPermissionEntity
            {
                Action = context.Message.Action,
                Id = context.Message.Id,
                Resource = context.Message.Resource,
                Description = context.Message.Description,
            };
            await _genericCommandRepository.AddAsync(permission);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
