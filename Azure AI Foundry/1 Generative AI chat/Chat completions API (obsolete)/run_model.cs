// Install the .NET library via NuGet: dotnet add package Azure.AI.OpenAI --prerelease
using Azure;
using Azure.AI.OpenAI;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json;
using static System.Environment;

async Task RunAsync()
{
  // Retrieve the OpenAI endpoint from environment variables
  var endpoint = GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") ?? "https://azai103pavlo-6934-resource.cognitiveservices.azure.com/";
  if (string.IsNullOrEmpty(endpoint))
  {
      Console.WriteLine("Please set the AZURE_OPENAI_ENDPOINT environment variable.");
      return;
  }

  var key = GetEnvironmentVariable("AZURE_OPENAI_KEY");
  if (string.IsNullOrEmpty(key))
  {
      Console.WriteLine("Please set the AZURE_OPENAI_KEY environment variable.");
      return;
  }

    AzureKeyCredential credential = new AzureKeyCredential(key);

    // Initialize the AzureOpenAIClient
    AzureOpenAIClient azureClient = new(new Uri(endpoint), credential);

    // Initialize the ChatClient with the specified deployment name
    ChatClient chatClient = azureClient.GetChatClient("gpt-4.1");
    
    // List of messages to send
    var messages = new List<ChatMessage>
    {
        new SystemChatMessage(@"You are an AI assistant that helps people find information."),
              new UserChatMessage(@"I am going to Paris, what should I see?"),
              new AssistantChatMessage(@"Paris, the capital of France, is known for its stunning architecture, art museums, historical landmarks, and romantic atmosphere. Here are some of the top attractions to see in Paris:

1. The Eiffel Tower: The iconic Eiffel Tower is one of the most recognizable landmarks in the world and offers breathtaking views of the city.
2. The Louvre Museum: The Louvre is one of the world's largest and most famous museums, housing an impressive collection of art and artifacts, including the Mona Lisa.
3. Notre-Dame Cathedral: This beautiful cathedral is one of the most famous landmarks in Paris and is known for its Gothic architecture and stunning stained glass windows.

These are just a few of the many attractions that Paris has to offer. With so much to see and do, it's no wonder that Paris is one of the most popular tourist destinations in the world."),
              new UserChatMessage(@"What is so great about #1?"),
    };


    // Create chat completion options

    var options = new ChatCompletionOptions {
        Temperature = (float)0.7,
        MaxOutputTokenCount = 800,
        
        TopP=(float)0.95,
        FrequencyPenalty=(float)0,
        PresencePenalty=(float)0
    };

    try
    {
        // Create the chat completion request
        ChatCompletion completion = await chatClient.CompleteChatAsync(messages, options);

        // Print the response
        if (completion != null)
        {
            Console.WriteLine(JsonSerializer.Serialize(completion, new JsonSerializerOptions() { WriteIndented = true }));
        }
        else
        {
            Console.WriteLine("No response received.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

await RunAsync();

//using OpenAI;
//using OpenAI.Chat;
//using System.ClientModel;

//const string endpoint = "https://azai103pavlo-6934-resource.openai.azure.com/openai/v1";
//const string apiKey = "<your-api-key>";

//ChatClient client = new(
//    credential: new ApiKeyCredential(apiKey),
//    model: "MODEL-DEPLOYMENT-NAME",
//    options: new OpenAIClientOptions()
//    {
//        Endpoint = new($"{endpoint}"),
//    });

//ChatCompletion completion = client.CompleteChat(
//     [
//         new SystemChatMessage("You are a helpful assistant that talks like a pirate."),
//         new UserChatMessage("Hi, can you help me?"),
//         new AssistantChatMessage("Arrr! Of course, me hearty! What can I do for ye?"),
//         new UserChatMessage("What's the best way to train a parrot?"),
//     ]);

//Console.WriteLine($"Model={completion.Model}");
//foreach (ChatMessageContentPart contentPart in completion.Content)
//{
//    string message = contentPart.Text;
//    Console.WriteLine($"Chat Role: {completion.Role}");
//    Console.WriteLine("Message:");
//    Console.WriteLine(message);
//}