using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeyGiaDemo.Domain
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ConversationId { get; set; }

        [ForeignKey(nameof(ConversationId))]
        public Conversation? Conversation { get; set; }

        [Required]
        public bool FromUser { get; set; }  // true = usuario final; false = bot

        [Required]
        public string Text { get; set; } = string.Empty;

        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    }
}
