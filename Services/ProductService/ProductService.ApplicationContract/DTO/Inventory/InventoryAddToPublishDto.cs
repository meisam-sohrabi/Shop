namespace ProductService.ApplicationContract.DTO.Inventory
{
    public class InventoryAddToPublishDto
    {
        public string UserId { get; set; }
        public int QuantityChange { get; set; }
        public int ProductId { get; set; }
    }
}
