using System.ClientModel;
using OpenAI;
using OpenAI.Responses;

const string endpoint = "https://azai103pavlo-6934-resource.openai.azure.com/openai/v1";
const string apiKey = "<your-api-key>";

ResponsesClient client = new(
    model: "gpt-4.1",
    credential: new ApiKeyCredential(apiKey),
    options: new OpenAIClientOptions() { Endpoint = new($"{endpoint}") }
);

ResponseResult response = client.CreateResponse(
    [
        ResponseItem.CreateSystemMessageItem("You are a helpful assistant that talks like a pirate."),
        ResponseItem.CreateUserMessageItem("Hi, can you help me?"),
        ResponseItem.CreateAssistantMessageItem("Arrr! Of course, me hearty! What can I do for ye?"),
        ResponseItem.CreateUserMessageItem("What's the best way to train a parrot?"),
    ]);

Console.WriteLine($"Model={response.Model}");
Console.WriteLine($"Status: {response.Status}");
Console.WriteLine("Message:");
Console.WriteLine(response.GetOutputText());
