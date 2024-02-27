using System;
using System.IO;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;

// Connect to RabbitMQ
string rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";

var factory = new ConnectionFactory() { HostName = rabbitMqHost };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Declare a queue
channel.QueueDeclare(queue: "report",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var tcs = new TaskCompletionSource<bool>();

// Create a consumer
var consumer = new EventingBasicConsumer(channel);

// Consume messages from the queue
consumer.Received += async (model, ea) =>
{
    await ProcessMessageAsync();
};

channel.BasicConsume(queue: "report",
                     autoAck: true,
                     consumer: consumer);

await tcs.Task;

async Task ProcessMessageAsync()
{
    // Generate the report content
    var reportContent = $"Report generated on {DateTime.Now.ToString()}";

    // Save the report content to a TXT file
    var filePath = $"./reports/report_{DateTime.Now:yyyyMMddHHmmss}.txt";

    File.WriteAllText(filePath, reportContent);

    Console.WriteLine("Report generated and saved.");
}
