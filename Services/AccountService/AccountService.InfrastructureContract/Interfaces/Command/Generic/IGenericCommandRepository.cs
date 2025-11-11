namespace AccountService.InfrastructureContract.Interfaces.Command.Generic
{
    public interface IGenericCommandRepository<T>
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entity);
        void Delete(T entity);
        void Edit(T entity);
    }
}
