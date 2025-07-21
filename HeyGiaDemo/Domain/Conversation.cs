using System.ComponentModel.DataAnnotations;

namespace HeyGiaDemo.Domain
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        // Identificador externo (e.g. teléfono, email, sesión web). Opcional en el demo.
        public string? ExternalUserId { get; set; }

        public LeadStatus LeadStatus { get; set; } = LeadStatus.Unqualified;

        public int LeadScore { get; set; } = 0; // 0-100 escala simple.

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public List<ChatMessage> Messages { get; set; } = new();
    }
}
