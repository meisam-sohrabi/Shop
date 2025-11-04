namespace ProductService.Domain.Entities
{
    public class BaseEntity
    {
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        public DateTime? ModifyDate { get; set; }
        public string? CreateBy { get; set; }
        public string? ModifyBy { get; set; }
        public bool Status { get; set; } = true;

    }
}
