using System.Text.RegularExpressions;

namespace HeyGiaDemo.Services.Llm
{
    public class LocalRuleLlmClient: ILlmClient
    {
        public Task<string> GenerateAsync(IEnumerable<(bool fromUser, string text)> history, string userMessage, CancellationToken ct = default)
        {
            var text = userMessage ?? string.Empty;

            if (Regex.IsMatch(text, "precio|costo|cuanto vale", RegexOptions.IgnoreCase))
                return Task.FromResult("Nuestros precios varían según el plan. ¿Buscas algo básico, estándar o premium?");

            if (Regex.IsMatch(text, "interesad|comprar|quiero", RegexOptions.IgnoreCase))
                return Task.FromResult("Excelente. ¿Podrías compartirme tu correo o teléfono para que un asesor te contacte?");

            if (Regex.IsMatch(text, "hola|buenas", RegexOptions.IgnoreCase))
                return Task.FromResult("¡Hola! Soy tu asesor virtual. Cuéntame qué necesitas y te ayudo.");

            return Task.FromResult("Puedo ayudarte con información de productos, precios y conexión con un asesor. ¿Qué estás buscando?");
        }
    }
}
