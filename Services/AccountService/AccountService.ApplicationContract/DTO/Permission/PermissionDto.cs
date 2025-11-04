namespace AccountService.ApplicationContract.DTO.Permission
{
    public class PermissionDto
    {
        public string Resource { get; set; }
        public string Action { get; set; }
        public string? Description { get; set; }
    }
}
