using HeyGiaDemo.Domain;

namespace HeyGiaDemo.Services
{
    public class LeadScoringService
    {
        private readonly Dictionary<string, int> _keywords = new(StringComparer.OrdinalIgnoreCase)
        {
            {"comprar", 40},
            {"quiero", 25},
            {"precio", 30},
            {"cotizacion", 35},
            {"interesado", 30},
            {"plan", 10},
            {"urgente", 20},
        };

        public LeadScoreResult Score(string latestMessage, int currentScore)
        {
            var total = currentScore;
            var lower = latestMessage.ToLowerInvariant();
            var hits = new List<string>();

            foreach (var kv in _keywords)
            {
                if (lower.Contains(kv.Key))
                {
                    total += kv.Value;
                    hits.Add(kv.Key);
                }
            }

            var status = LeadStatus.Unqualified;
            if (total >= 80) status = LeadStatus.NeedsHuman;  // listo para traspaso
            else if (total >= 60) status = LeadStatus.Qualified;
            else if (total >= 20) status = LeadStatus.Engaged;

            return new LeadScoreResult(total, status, hits.Count > 0 ? "keywords:" + string.Join(',', hits) : null);
        }
    }
}
