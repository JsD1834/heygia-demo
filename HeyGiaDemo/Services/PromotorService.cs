using HeyGiaDemo.Context;
using HeyGiaDemo.Domain;
using HeyGiaDemo.Services.Llm;
using Microsoft.EntityFrameworkCore;
using System;

namespace HeyGiaDemo.Services
{
    public class PromotorService: IPromotorService
    {
        private readonly HeyGiaDemoDbContext _db;
        private readonly ILlmClient _llm;
        private readonly LeadScoringService _scoring;

        public PromotorService(HeyGiaDemoDbContext db, ILlmClient llm, LeadScoringService scoring)
        {
            _db = db;
            _llm = llm;
            _scoring = scoring;
        }

        public async Task<(string botReply, Conversation updatedConv)> SendUserMessageAsync(int? conversationId, string? externalUserId, string userMessage, CancellationToken ct = default)
        {
            Conversation? conv = null;

            if (conversationId.HasValue)
            {
                conv = await _db.Conversations.Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == conversationId.Value, ct);
            }
            else if (!string.IsNullOrWhiteSpace(externalUserId))
            {
                conv = await _db.Conversations.Include(c => c.Messages).FirstOrDefaultAsync(c => c.ExternalUserId == externalUserId, ct);
            }

            if (conv is null)
            {
                conv = new Conversation { ExternalUserId = externalUserId };
                _db.Conversations.Add(conv);
                await _db.SaveChangesAsync(ct);
                // recarga con include
                conv = await _db.Conversations.Include(c => c.Messages).FirstAsync(c => c.Id == conv.Id, ct);
            }

            // Agregar mensaje de usuario
            var userMsg = new ChatMessage { FromUser = true, Text = userMessage, ConversationId = conv.Id };
            conv.Messages.Add(userMsg);

            // Scoring
            var scoreResult = _scoring.Score(userMessage, conv.LeadScore);
            conv.LeadScore = Math.Clamp(scoreResult.Score, 0, 100);
            conv.LeadStatus = scoreResult.Status;
            conv.UpdatedAt = DateTimeOffset.UtcNow;

            // Historial para LLM
            var history = conv.Messages.Select(m => (m.FromUser, m.Text));
            var botText = await _llm.GenerateAsync(history, userMessage, ct);

            // Si lead está muy caliente, fuerza mensaje de traspaso
            if (conv.LeadStatus is LeadStatus.NeedsHuman or LeadStatus.Qualified)
            {
                botText += "\n\nParece que estás muy interesado. En breve un asesor humano se pondrá en contacto contigo. ¿Podrías dejar un correo o teléfono?";
            }

            var botMsg = new ChatMessage { FromUser = false, Text = botText, ConversationId = conv.Id };
            conv.Messages.Add(botMsg);
            conv.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return (botText, conv);
        }
    }
}
