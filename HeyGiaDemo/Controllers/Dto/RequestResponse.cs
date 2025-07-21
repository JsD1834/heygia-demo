using Microsoft.AspNetCore.Mvc;

namespace HeyGiaDemo.Controllers.Dto
{
    public record ChatRequest(int? ConversationId, string? ExternalUserId, string Message);

    public record ChatResponse(int ConversationId, string BotReply, int LeadScore, string LeadStatus, DateTimeOffset UpdatedAt);
}
