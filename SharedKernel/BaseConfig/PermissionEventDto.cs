namespace BaseConfig
{
    public class PermissionEventDto
    {
        public int Id { get; set; }
        public string Resource { get; set; }
        public string Action { get; set; }
        public string? Description { get; set; }
    }
}
