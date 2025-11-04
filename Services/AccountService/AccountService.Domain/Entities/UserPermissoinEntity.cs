using System.ComponentModel.DataAnnotations.Schema;

namespace AccountService.Domain.Entities
{
    public class UserPermissoinEntity
    {
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public CustomUserEntity User { get; set; }
        public int PermissionId { get; set; }
        [ForeignKey(nameof(PermissionId))]
        public PermissionEntity Permission { get; set; }
    }
}
