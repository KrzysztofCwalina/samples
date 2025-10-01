#:package OpenAI@2.3.*-*
#:package Microsoft.Extensions.Logging@7.*-*
#:package Microsoft.Extensions.Logging.Console@7.*-*
#:property PublishAot=false

using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.ClientModel.Primitives;

var key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

ClientLoggingOptions logging = new();
logging.EnableMessageContentLogging = true;
logging.EnableMessageLogging = true;
logging.LoggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Trace);
});

OpenAIClientOptions options = new()
{
    ClientLoggingOptions = logging,
};

ChatClient client = new("mistral-large-latest", new ApiKeyCredential(key), options);
ChatCompletion completion = client.CompleteChat("Write a very short one-sentence bedtime story about a unicorn.");
Console.WriteLine(completion.Content[0].Text);

