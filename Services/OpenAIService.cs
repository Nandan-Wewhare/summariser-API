using Azure;
using Azure.AI.OpenAI;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace PPT_generator_API.Services
{
    // This service will interact with azure open ai

    public class OpenAIService
    {
        private readonly AzureOpenAIClient _client;
        private readonly string _deploymentName;

        public OpenAIService(IConfiguration configuration)
        {
            var endpoint = new Uri(configuration["AzureOpenAI:Endpoint"]);
            var apiKey = configuration["AzureOpenAI:ApiKey"];
            _deploymentName = configuration["AzureOpenAI:DeploymentName"];
            _client = new (new Uri("https://your-azure-openai-resource.com"), new ApiKeyCredential(""));
            ChatClient chatClient = _client.GetChatClient("my-gpt-35-turbo-deployment");
        }

        public async Task<string> GeneratePresentationContentAsync(string inputText)
        {
            
        }
    }

}
