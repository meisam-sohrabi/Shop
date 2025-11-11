namespace ProductService.Domain.Entities
{
    public class LocalPermissionEntity
    {
        public int Id { get; set; }
        public string Resource { get; set; }
        public string Action { get; set; }
        public string? Description { get; set; }
    }
}
