namespace AccountService.Domain.Entities
{
    public class BaseEntity
    {
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? CreateBy { get; set; }
        public int? ModifyBy { get; set; }
        public bool Status { get; set; } = true;

    }
}
