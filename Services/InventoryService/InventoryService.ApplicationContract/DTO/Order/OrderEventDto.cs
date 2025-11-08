namespace InventoryService.ApplicationContract.DTO.Order
{
    public class OrderEventDto
    {
        public int ProductDetailId { get; set; }
        public int QuantityChange { get; set; }
        public string UserId { get; set; }
    }
}
