using Microsoft.KernelMemory;
using Microsoft.KernelMemory.Service.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Warning);

builder.AddKernelMemory(kernelMemoryBuilder => kernelMemoryBuilder
    .WithOpenAITextGeneration(
        new OpenAIConfig()
        {
            Endpoint = "https://api.openai.com/v1",
            APIKey = "APIKey",
            TextModel = "gpt-4.1-nano",
        }
    )
    .WithOpenAITextEmbeddingGeneration(
        new OpenAIConfig()
        {
            Endpoint = "https://api.openai.com/v1",
            APIKey = "APIKey",
            EmbeddingModel = "text-embedding-3-small"
        }
    ));

var app = builder.Build();

app.AddKernelMemoryEndpoints();

var memoryService = app.Services.GetRequiredService<IKernelMemory>();
var documentStream = new MemoryStream(Encoding.UTF8.GetBytes("Dies ist geheim!"));
await memoryService.ImportDocumentAsync(documentStream, "geheime-nachricht.txt");

app.Run();