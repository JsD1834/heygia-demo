namespace HeyGiaDemo.Services.Llm
{
    public interface ILlmClient
    {
        /// <summary>
        /// Envía el historial (opcional) + mensaje actual y retorna la respuesta generada.
        /// </summary>
        Task<string> GenerateAsync(IEnumerable<(bool fromUser, string text)> history, string userMessage, CancellationToken ct = default);
    }
}
