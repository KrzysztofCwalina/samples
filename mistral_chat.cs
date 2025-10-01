#:package OpenAI@2.3.*-*
#:property PublishAot=false

using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var key = Environment.GetEnvironmentVariable("MISTRAL_API_KEY");

OpenAIClientOptions options = new()
{
    Endpoint = new Uri("https://api.mistral.ai/v1"),
};

ChatClient client = new("mistral-large-latest", new ApiKeyCredential(key), options);
ChatCompletion completion = client.CompleteChat("Write a very short one-sentence bedtime story about a unicorn.");
Console.WriteLine(completion.Content[0].Text);