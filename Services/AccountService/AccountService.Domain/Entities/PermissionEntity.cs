using System.ComponentModel.DataAnnotations;

namespace AccountService.Domain.Entities
{
    public class PermissionEntity : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Resource { get; set; } 
        public string Action { get; set; }
        public string? Description { get; set; }
        public ICollection<UserPermissoinEntity> UserPermissions { get; set; } = new List<UserPermissoinEntity>();

    }
}
