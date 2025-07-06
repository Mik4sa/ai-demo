using Microsoft.Extensions.Hosting;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using ModelContextProtocol.SemanticKernel.Extensions;

namespace AiDemo;

internal sealed class PromptHostedService(
    Kernel kernel,
    IChatCompletionService chatCompletionService) : IHostedLifecycleService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task StartedAsync(CancellationToken cancellationToken)
    {
        // #1 - Einfacher, direkter Prompt
        FunctionResult promptResult = await kernel.InvokePromptAsync("Was ist deine Aufgabe?", cancellationToken: cancellationToken);
        string? promptResultString = promptResult.GetValue<string>();
        Console.WriteLine("Frage  : Was ist deine Aufgabe?");
        Console.WriteLine("Antwort: " + promptResultString);
        Console.WriteLine();
        Console.WriteLine();


        // #2 - Chat Prompt mit SystemMessage
        ChatHistory history1 = [];
        history1.AddSystemMessage("Du bist ein hilfreicher Assistent und antwortest AUSSCHLIEßLICH auf Deutsch.");
        history1.AddUserMessage("Wie ist dein Name?");

        PromptExecutionSettings promptExecutionSettings1 = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(), };
        ChatMessageContent message1 = await chatCompletionService.GetChatMessageContentAsync(history1, promptExecutionSettings1, kernel, cancellationToken);
        Console.WriteLine("Frage  : Wie ist dein Name?");
        Console.WriteLine("Antwort: " + message1);
        Console.WriteLine();
        Console.WriteLine();


        // #3 Chat Prompt über kernelMemory
        IKernelMemory kernelMemory = new MemoryWebClient("http://localhost:5025/");
        ChatHistory history2 = [];
        history2.AddSystemMessage("Du bist ein hilfreicher Assistent und antwortest AUSSCHLIEßLICH auf Deutsch.");
        history2.AddUserMessage("In deinem Speicher sollte eine geheime Nachricht hinterlegt sein. Wie lautet diese?");

        PromptExecutionSettings promptExecutionSettings2 = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(), };
        kernel.Plugins.AddFromObject(new MemoryPlugin(kernelMemory));
        ChatMessageContent message2 = await chatCompletionService.GetChatMessageContentAsync(history2, promptExecutionSettings2, kernel, cancellationToken);
        Console.WriteLine("Frage  : In deinem Speicher sollte eine geheime Nachricht hinterlegt sein. Wie lautet diese?");
        Console.WriteLine("Antwort: " + message2);
        Console.WriteLine();
        Console.WriteLine();


        // #4 Chat Prompt über ModelContextProtocol  (MCP)
        ChatHistory history3 = [];
        history3.AddSystemMessage("Du bist ein hilfreicher Assistent und antwortest AUSSCHLIEßLICH auf Deutsch.");
        history3.AddUserMessage("Welcher Tag ist heute?");

        PromptExecutionSettings promptExecutionSettings3 = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(), };
        await kernel.Plugins.AddMcpFunctionsFromSseServerAsync("localhost MCP", "http://localhost:5227", cancellationToken: cancellationToken);
        ChatMessageContent message3 = await chatCompletionService.GetChatMessageContentAsync(history3, promptExecutionSettings3, kernel, cancellationToken);
        Console.WriteLine("Frage  : Welcher Tag ist heute?");
        Console.WriteLine("Antwort: " + message3);
    }

    public Task StartingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
