#:package OpenAI@2.2.*-*
#:package Microsoft.Extensions.Logging.Console@9.0.*
#:property PublishAot=false

using OpenAI;
using OpenAI.Embeddings;
using System.ClientModel;
using System.ClientModel.Primitives;
using Microsoft.Extensions.Logging;

ClientLoggingOptions loggingOptions = new()
{
    EnableLogging = true,
    EnableMessageContentLogging = true,
    EnableMessageLogging = true,
    LoggerFactory = LoggerFactory.Create(builder => builder
        .AddConsole()
        .SetMinimumLevel(LogLevel.Trace))
};

OpenAIClientOptions options = new()
{
    Endpoint = new Uri("http://localhost:11434/v1"),
    ClientLoggingOptions = loggingOptions
};

EmbeddingClient client = new("nomic-embed-text:v1.5", new ApiKeyCredential("no_key"), options);
OpenAIEmbedding embedding = client.GenerateEmbedding("Write a very short one-sentence bedtime story about a unicorn.");
ReadOnlyMemory<float> floats = embedding.ToFloats();
Console.WriteLine($"Embedding has {floats.Length} floats.");