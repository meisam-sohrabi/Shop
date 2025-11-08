namespace OrderService.ApplicationContract.DTO.Order
{
    public class OrderResponseDto
    {
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderedAt { get; set; }
    }
}
