using HeyGiaDemo.Controllers.Dto;
using HeyGiaDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeyGiaDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IPromotorService _promotor;

        public ChatController(IPromotorService promotor)
        {
            _promotor = promotor;
        }

        [HttpPost]
        public async Task<ActionResult<ChatResponse>> Post([FromBody] ChatRequest req, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(req.Message))
                return BadRequest("Message required");

            var (botReply, conv) = await _promotor.SendUserMessageAsync(req.ConversationId, req.ExternalUserId, req.Message, ct);

            return Ok(new ChatResponse(conv.Id, botReply, conv.LeadScore, conv.LeadStatus.ToString(), conv.UpdatedAt));
        }
    }
}
