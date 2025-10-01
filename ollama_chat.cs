#:package OpenAI@2.3.*-*
#:property PublishAot=false

using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

OpenAIClientOptions options = new()
{
    Endpoint = new Uri("http://localhost:11434/v1")
};

ChatClient client = new("phi4-mini:3.8b", new ApiKeyCredential("no_key"), options);
ChatCompletion completion = client.CompleteChat("Write a very short one-sentence bedtime story about a unicorn.");
Console.WriteLine(completion.Content[0].Text);