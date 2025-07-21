using HeyGiaDemo.Domain;

namespace HeyGiaDemo.Services
{
    public interface IPromotorService
    {
        Task<(string botReply, Conversation updatedConv)> SendUserMessageAsync(int? conversationId, string? externalUserId, string userMessage, CancellationToken ct = default);
    }
}
