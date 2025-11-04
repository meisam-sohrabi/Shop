using AccountService.Domain.Entities;

namespace AccountService.InfrastructureContract.Interfaces.Command.UserPermission
{
    public interface IUserPermissionCommandRepository
    {
        Task AssignPermissionToUser(UserPermissoinEntity userPermissoinEntity);
        void RevokePermissionFromUser(UserPermissoinEntity userPermissoinEntity);
    }
}
