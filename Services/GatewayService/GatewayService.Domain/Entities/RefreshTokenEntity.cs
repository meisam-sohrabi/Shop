namespace GatewayService.Domain.Entities
{
    public class RefreshTokenEntity
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
        public CustomUserEntity? User { get; set; }
    }
}
