namespace ProductService.ApplicationContract.DTO.Product
{
    public class ProductRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int ProductBrandId { get; set; }
    }
}
