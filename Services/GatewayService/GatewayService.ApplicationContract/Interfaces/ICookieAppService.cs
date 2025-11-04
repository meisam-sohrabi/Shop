namespace GatewayService.ApplicationContract.Interfaces
{
    public interface ICookieAppService
    {
        void SetCookie(string key, string valus, TimeSpan? expireTime = null);
        string GetCookie(string key);
        bool DeleteCookie(string key);
    }
}
