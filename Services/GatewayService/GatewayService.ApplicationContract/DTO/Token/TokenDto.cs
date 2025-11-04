namespace GatewayService.ApplicationContract.DTO.Token
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string? ImageUrl { get; set; }
    }
}
