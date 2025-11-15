namespace ProductService.InfrastructureContract.DapperModel
{
    public class ProductWithInventoryRecord
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductQuantity { get; set; }
        public string CategoryName { get; set; }
        public bool CategoryStatus { get; set; }
        public string ProductBrand { get; set; }
        public string ProductBrandDescription { get; set; }
        public int? QuantityChange { get; set; }
        public DateTime? ChangeDate { get; set; }
    }
}
