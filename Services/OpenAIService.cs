using Azure.AI.OpenAI;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using OpenAI.Chat;
using System.ClientModel;

namespace PPT_generator_API.Services
{
    // This service will interact with azure open ai

    public class OpenAIService: IOpenAIService
    {
        private readonly AzureOpenAIClient _client;
        private readonly string _deploymentName;
        private readonly ChatClient _chatClient;

        public OpenAIService(IConfiguration configuration)
        {
            var client = new SecretClient(vaultUri: new Uri("https://pptgen-kv.vault.azure.net/"), credential: new DefaultAzureCredential());
            var endpoint = new Uri(client.GetSecret(configuration["AzureOpenAI:Endpoint"]).Value.Value);
            var apiKey = client.GetSecret(configuration["AzureOpenAI:ApiKey"]).Value.Value;
            _deploymentName = client.GetSecret(configuration["AzureOpenAI:DeploymentName"]).Value.Value;
            _client = new(endpoint, new ApiKeyCredential(apiKey));
            _chatClient = _client.GetChatClient(_deploymentName);
        }

        public async Task<string> GenerateContentAsync(string inputText)
        {
            var chatResult = await _chatClient.CompleteChatAsync([new SystemChatMessage("You are a helpful assistant that summarizes the document"), new SystemChatMessage($"Summarize the following content:\\n\\n{inputText}")]);
            return chatResult.Value.Content[0].Text;
        }
    }
}
