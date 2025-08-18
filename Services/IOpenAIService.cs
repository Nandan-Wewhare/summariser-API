using OpenAI.Chat;

namespace PPT_generator_API.Services
{
    public interface IOpenAIService
    {
        Task<string> GenerateContentAsync(string inputText);
    }
}
