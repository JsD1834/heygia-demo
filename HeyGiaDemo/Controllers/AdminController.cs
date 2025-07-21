using HeyGiaDemo.Context;
using HeyGiaDemo.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace HeyGiaDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly HeyGiaDemoDbContext _db;

        public AdminController(HeyGiaDemoDbContext db) => _db = db;

        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations(CancellationToken ct)
        {
            var items = await _db.Conversations
                .OrderByDescending(c => c.UpdatedAt)
                .Select(c => new {
                    c.Id,
                    c.ExternalUserId,
                    c.LeadStatus,
                    c.LeadScore,
                    c.CreatedAt,
                    c.UpdatedAt,
                    MessageCount = c.Messages.Count
                })
                .ToListAsync(ct);

            return Ok(items);
        }

        [HttpGet("conversation/{id:int}")]
        public async Task<IActionResult> GetConversation(int id, CancellationToken ct)
        {
            var conv = await _db.Conversations.Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == id, ct);
            if (conv == null) return NotFound();

            var dto = new
            {
                conv.Id,
                conv.ExternalUserId,
                conv.LeadStatus,
                conv.LeadScore,
                conv.CreatedAt,
                conv.UpdatedAt,
                Messages = conv.Messages.OrderBy(m => m.Timestamp).Select(m => new { m.Id, m.FromUser, m.Text, m.Timestamp })
            };
            return Ok(dto);
        }

        // Endpoint rápido para marcar manualmente lead como NeedsHuman
        [HttpPost("conversation/{id:int}/mark-needs-human")]
        public async Task<IActionResult> MarkNeedsHuman(int id, CancellationToken ct)
        {
            var conv = await _db.Conversations.FindAsync([id], ct);
            if (conv == null) return NotFound();

            conv.LeadStatus = LeadStatus.NeedsHuman;
            conv.UpdatedAt = DateTimeOffset.UtcNow;
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
