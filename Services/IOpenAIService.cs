using OpenAI.Chat;

namespace PPT_generator_API.Services
{
    public interface IOpenAIService
    {
        Task<string> GeneratePresentationContentAsync(string inputText);
    }
}
