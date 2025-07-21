namespace HeyGiaDemo.Domain
{
    public record LeadScoreResult(int Score, LeadStatus Status, string? Reason = null);
}
