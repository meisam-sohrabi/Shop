namespace InventoryService.InfrastructureContract.Interfaces.Query.Generic
{
    public interface IGenericQueryRepository<T>
    {
        IQueryable<T> GetQueryable();
    }
}
