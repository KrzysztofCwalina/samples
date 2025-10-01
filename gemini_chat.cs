#:package OpenAI@2.2.*-*
#:package System.ClientModel@1.5.0

using System.ClientModel;
using System.ClientModel.Primitives;
using System.Text.Json;
using OpenAI.Chat;

string key = Environment.GetEnvironmentVariable("GEMINI_API_KEY")!;
ApiKeyCredential credential = new(key);
ApiKeyAuthenticationPolicy auth = ApiKeyAuthenticationPolicy.CreateHeaderApiKeyPolicy(credential, "X-goog-api-key");

ClientPipelineOptions pipelineOptions = new();
pipelineOptions.AddPolicy(auth, PipelinePosition.PerCall);

ClientPipeline pipeline = ClientPipeline.Create(pipelineOptions);
PipelineMessage message = pipeline.CreateMessage();
message.Request.Uri = new Uri("https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent");
message.Request.Method = "POST";
message.Request.Headers.Add("Content-Type", "application/json");

GeminiChatRequest request = new(ChatMessage.CreateUserMessage("Explain how AI works in a few words"));
message.Request.Content = request;

pipeline.Send(message);

GeminiChatCompletion completion = ModelReaderWriter.Read<GeminiChatCompletion>(message.Response!.Content)!;
Console.WriteLine(completion);

class GeminiChatCompletion : IJsonModel<GeminiChatCompletion>
{
    public string Text { get; private set; } = string.Empty;
    public ChatFinishReason FinishReason { get; private set; }
    public GeminiChatCompletion Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options)
    {
        GeminiChatCompletion completion = new();

        JsonDocument documet = JsonDocument.ParseValue(ref reader);
        JsonElement root = documet.RootElement;
        JsonElement candidates = root.GetProperty("candidates");

        foreach (JsonElement candidate in candidates.EnumerateArray())
        {
            string finishReason = candidate.GetProperty("finishReason").GetString();
            completion.FinishReason = Enum.Parse<ChatFinishReason>(finishReason, ignoreCase: true);

            JsonElement content = candidate.GetProperty("content");
            JsonElement parts = content.GetProperty("parts");
            foreach (JsonElement part in parts.EnumerateArray())
            {
                if (part.TryGetProperty("text", out JsonElement textElement))
                {
                    completion.Text += textElement.GetString();
                }
            }
        }

        return completion;
    }

    public GeminiChatCompletion Create(BinaryData data, ModelReaderWriterOptions options)
    {
        Utf8JsonReader reader = new Utf8JsonReader(data.ToMemory().Span);
        return Create(ref reader, options);
    }

    public string GetFormatFromOptions(ModelReaderWriterOptions options)
        => "J";

    public void Write(Utf8JsonWriter writer, ModelReaderWriterOptions options)
    {
        throw new NotImplementedException();
    }

    public BinaryData Write(ModelReaderWriterOptions options)
    {
        throw new NotImplementedException();
    }

    override public string ToString() => Text;
}

class GeminiChatRequest : BinaryContent
{
    public GeminiChatRequest(params ChatMessage[] message)
    {
        Messages = new List<ChatMessage>(message);
    }

    public IList<ChatMessage> Messages { get; } 
    public override void Dispose()
    {
        throw new NotImplementedException();
    }

    public override bool TryComputeLength(out long length)
    {
        length = 0;
        return false;
    }

    public override void WriteTo(Stream stream, CancellationToken cancellationToken = default)
    {
        Utf8JsonWriter writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
        writer.WriteStartObject();
        writer.WritePropertyName("contents");
        writer.WriteStartArray();
        foreach (var message in Messages)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("parts");
            writer.WriteStartArray();
            foreach (ChatMessageContentPart part in message.Content)
            {
                writer.WriteStartObject();
                writer.WriteString("text", part.Text);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        writer.WriteEndArray();
        writer.WriteEndObject();
        writer.Flush();
        stream.Flush();
    }

    public override Task WriteToAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}