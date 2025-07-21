using System.Net.Http.Headers;
using System.Text.Json;

namespace HeyGiaDemo.Services.Llm
{
    public class HuggingFaceLlmClient: ILlmClient
    {
        private readonly HttpClient _http;
        private readonly string _modelId;

        public HuggingFaceLlmClient(HttpClient http, IConfiguration config)
        {
            _http = http;
            _modelId = config["Llm:HuggingFaceModel"] ?? "HuggingFaceH4/zephyr-7b-beta";

            var token = config["Llm:HuggingFaceToken"];
            if (!string.IsNullOrWhiteSpace(token))
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<string> GenerateAsync(IEnumerable<(bool fromUser, string text)> history, string userMessage, CancellationToken ct = default)
        {
            // Construimos prompt simple estilo chat
            var messages = new List<Dictionary<string, string>>();
            foreach (var (fromUser, text) in history)
            {
                messages.Add(new() { { "role", fromUser ? "user" : "assistant" }, { "content", text } });
            }
            messages.Add(new() { { "role", "user" }, { "content", userMessage } });

            var payload = new { inputs = messages }; // Algunos modelos requieren formato distinto.
            using var resp = await _http.PostAsJsonAsync($"https://api-inference.huggingface.co/models/{_modelId}", payload, ct);
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadAsStringAsync(ct);

            // Respuesta puede variar según modelo; haz parsing defensivo.
            try
            {
                using var doc = JsonDocument.Parse(json);
                // Muchos endpoints devuelven array
                if (doc.RootElement.ValueKind == JsonValueKind.Array && doc.RootElement.GetArrayLength() > 0)
                {
                    var first = doc.RootElement[0];
                    if (first.TryGetProperty("generated_text", out var gt))
                        return gt.GetString() ?? string.Empty;
                }
            }
            catch { /* swallow y fallback */ }

            return "[Respuesta no parseada del modelo remoto]";
        }
    }
}
