using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI.Ollama;

var config = new OllamaConfig
{
    Endpoint = "http://192.168.1.6:11434",
    TextModel = new OllamaModelConfig("deepseek-r1:1.5b", 131072),
    EmbeddingModel = new OllamaModelConfig("deepseek-r1:1.5b", 2048)
};

var memory = new KernelMemoryBuilder()
    .WithOllamaTextGeneration(config)
    .WithOllamaTextEmbeddingGeneration(config)
    .Build<MemoryServerless>();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Processing document, please wait...");
Console.ResetColor();

await memory.ImportDocumentAsync("mega.txt", documentId: "DOC001");
Console.WriteLine("Model is ready, you can start querying.");

while(await memory.IsDocumentReadyAsync("DOC001"))
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write("Query: ");
    Console.ResetColor();

    var query = Console.ReadLine();
    if (query == "exit")
    {
        break;
    }

    var answer = await memory.AskAsync("DOC001", query);
    
    Console.Write($"{answer.Result}");
    
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("\n Souces:");

    foreach (var source in answer.RelevantSources)
    {
        Console.WriteLine($"- {source.SourceName} - {source.SourceUrl} - {source.Link}");
    }
}