namespace ProductService.ApplicationContract.DTO.Transaction
{
    public class ProductTransactionServiceDto
    {
        public int categoryId { get; set; }
        public int productBrandId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductQuantity { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string? DetailDescription { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}