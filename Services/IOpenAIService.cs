using OpenAI.Chat;

namespace Summary_generator_API.Services
{
    public interface IOpenAIService
    {
        Task<string> GenerateContentAsync(string inputText);
    }
}
