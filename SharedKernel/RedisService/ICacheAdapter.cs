namespace RedisService
{
    public interface ICacheAdapter
    {
        void Set<T>(string key, T value);
        T Get<T>(string key);
    }
}
