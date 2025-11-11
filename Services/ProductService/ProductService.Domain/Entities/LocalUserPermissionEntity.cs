using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Domain.Entities
{
    public class LocalUserPermissionEntity
    {
        public string UserId { get; set; }
        public int PermissionId { get; set; }
    }
}
