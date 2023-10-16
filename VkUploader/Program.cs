// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using VkUploader;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()) // Directory where the json files are located
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var apiController = new VkApiController(configuration);


await apiController.SendMessage(222996729, $"Message text {DateTime.UtcNow}", "test.png");
await apiController.SendMessageWithDock(2000000001, $"Message text", "test.pdf");


Console.WriteLine("Ok");

