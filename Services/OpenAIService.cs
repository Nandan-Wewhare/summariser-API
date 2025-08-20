using Azure.AI.OpenAI;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using OpenAI.Chat;
using System.ClientModel;

namespace Summary_generator_API.Services
{
    public class OpenAIService : IOpenAIService
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

        public async Task<string> GenerateContentAsync(string parsedResume, string jobDescription)
        {
            var chatResult = await _chatClient.CompleteChatAsync([new SystemChatMessage("You are a helpful assistant that will analyse the resume and a job description, and give a % probability that the resume gets selected by any ATS. The job description could be a link also, so in that case you need to parse the job description from there. If you cannot do that, just flag it and give the % - nothing else."), new SystemChatMessage($"Analyse the following resume and job description:\\n\\n{parsedResume} \\n\\n{jobDescription}")]);
            return chatResult.Value.Content[0].Text;
        }
    }
}
