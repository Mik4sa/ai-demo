using AiDemo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

// Warten bis KernelMemory und MCP gestartet sind
await Task.Delay(1500);

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Warning);

var deployment = "gpt-4.1-nano";
var endpoint = "https://api.openai.com/v1";
var apiKey = "APIKey";

builder.Services.AddKernel()
    .AddOpenAIChatCompletion(deployment, new Uri(endpoint), apiKey)
    .Plugins
        .AddFromType<DemoPlugin>();

builder.Services.AddHostedService<PromptHostedService>();

using IHost host = builder.Build();

await host.RunAsync();
