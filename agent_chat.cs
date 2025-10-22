#:package Microsoft.Agents.AI.OpenAI@1.0.0-preview.251016.1
#:package OpenAI@2.5.0
#:package Azure.Identity@1.17.0
#:property nowarn=OPENAI001

// Copyright (c) Microsoft. All rights reserved.

// This sample shows how to create and use a simple AI agent with Azure OpenAI Chat Completion as the backend.

using Azure.Identity;
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel.Primitives;

var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") ?? throw new InvalidOperationException("AZURE_OPENAI_ENDPOINT is not set.");
var deploymentName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ?? throw new InvalidOperationException("AZURE_OPENAI_DEPLOYMENT_NAME is not set.");
Console.WriteLine($"Using endpoint: {endpoint}");
Console.WriteLine($"Using deployment: {deploymentName}");

string scope = "https://cognitiveservices.azure.com/.default";
OpenAIClientOptions options = new()
{
    Endpoint = new Uri(endpoint),
};
ChatClient chat = new(deploymentName, new BearerTokenPolicy(new AzureCliCredential(), scope), options);
AIAgent agent = chat.CreateAIAgent(instructions: "You are good at telling jokes.", name: "Joker");

// Invoke the agent and output the text result.
Console.WriteLine(await agent.RunAsync("Tell me a joke about a pirate."));
