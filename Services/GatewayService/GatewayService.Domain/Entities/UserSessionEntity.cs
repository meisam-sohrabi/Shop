using System.ComponentModel.DataAnnotations;

namespace GatewayService.Domain.Entities
{
    public class UserSessionEntity
    {
        public int Id { get; set; }
        public DateTime LoginTime { get; set; } = DateTime.Now;
        public DateTime? LogoutTime { get; set; }

        public string UserId { get; set; }
        public string Username { get; set; }

    }
}
