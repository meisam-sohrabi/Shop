using BaseConfig;
using MassTransit;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.Generic;

namespace ProductService.Application.Services.ProductConsumer
{
    public class AssignPermissionInsertConsumer : IConsumer<UerPermissionEventDot>
    {
        private readonly IGenericCommandRepository<LocalUserPermissionEntity> _genericCommandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignPermissionInsertConsumer(IGenericCommandRepository<LocalUserPermissionEntity> genericCommandRepository,IUnitOfWork unitOfWork)
        {
            _genericCommandRepository = genericCommandRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Consume(ConsumeContext<UerPermissionEventDot> context)
        {
            var userPermission = new LocalUserPermissionEntity
            {
                UserId = context.Message.UserId,
                PermissionId = context.Message.PermissionId
            };
            await _genericCommandRepository.AddAsync(userPermission);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
