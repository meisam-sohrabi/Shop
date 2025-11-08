namespace OrderService.InfrastructureContract.Interfaces.Command.Generic
{
    public interface IGenericCommandRepository<T>
    {
        Task AddAsync(T entity);
        void Delete(T entity);
        void Edit(T entity);
    }
}
