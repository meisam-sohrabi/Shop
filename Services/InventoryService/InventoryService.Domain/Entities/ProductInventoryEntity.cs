namespace InventoryService.Domain.Entities
{
    public class ProductInventoryEntity : BaseEntity
    {
        public int Id { get; set; }
        public int QuantityChange { get; set; }
        public DateTime ChangeDate { get; set; } = DateTime.Now;
        public int ProductId { get; set; }
    }
}
