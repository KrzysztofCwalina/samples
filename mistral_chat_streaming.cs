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

ChatCompletionOptions disableStreamOptions = new();
System.Reflection.FieldInfo filed = typeof(ChatCompletionOptions).GetField("_additionalBinaryDataProperties", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
filed.SetValue(disableStreamOptions, new Dictionary<string, BinaryData> { { "stream_options",  BinaryData.FromString("{}") } });

CollectionResult<StreamingChatCompletionUpdate> completion = client.CompleteChatStreaming(["Write a very short one-sentence bedtime story about a unicorn."], disableStreamOptions);
foreach (var update in completion)
{
    Console.Write(update.ContentUpdate[0].Text);
}

