namespace InventoryService.ApplicationContract.DTO.ProductInventory
{
    public class ProductInventoryRequestDto
    {
        public string UserId { get; set; }
        public int QuantityChange { get; set; }
        public int ProductId { get; set; }
    }
}
