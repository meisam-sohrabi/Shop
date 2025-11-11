namespace AccountService.Domain.Entities
{
    public class OutBoxMessageEntity
    {
        public Guid Id { get; set; }
        public string Event { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool Sent { get; set; } = false;
        public DateTime? SentAt { get; set; }
    }
}
