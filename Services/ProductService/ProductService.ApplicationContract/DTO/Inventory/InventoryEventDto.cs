namespace ProductService.ApplicationContract.DTO.Inventory
{
    public class InventoryEventDto
    {
        public string UserId { get; set; }
        public int QuantityChange { get; set; }
        public int ProductDetailId { get; set; }
    }
}
