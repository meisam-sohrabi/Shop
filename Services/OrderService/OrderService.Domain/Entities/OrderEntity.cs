namespace OrderService.Domain.Entities
{
    public class OrderEntity : BaseEntity
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderedAt { get; set; } = DateTime.Now;
        public int ProductDetailId { get; set; }
        public string UserId { get; set; }

    }
}
