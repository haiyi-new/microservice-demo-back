using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace Publisher.Controllers;


[ApiController]
[Route("/")]
public class WeatherForecastController : ControllerBase
{

    string rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {

        var factory = new ConnectionFactory { HostName = rabbitMqHost };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "hello",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        channel.QueueDeclare(queue: "report",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        channel.QueueDeclare(queue: "email",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

        // Publish a message to the "hello" queue
        const string helloMessage = "Hello World!";
        var helloBody = Encoding.UTF8.GetBytes(helloMessage);
        channel.BasicPublish(exchange: "",
                     routingKey: "hello",
                     basicProperties: null,
                     body: helloBody);
        // Publish a message to the "report" queue
        const string reportMessage = "Report message";
        var reportBody = Encoding.UTF8.GetBytes(reportMessage);
        channel.BasicPublish(exchange: "",
                     routingKey: "report",
                     basicProperties: null,
                     body: reportBody);

        // Publish a message to the "email" queue
        const string emailMessage = "Email message";
        var emailBody = Encoding.UTF8.GetBytes(emailMessage);
        channel.BasicPublish(exchange: "",
                     routingKey: "email",
                     basicProperties: null,
                     body: emailBody);

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
