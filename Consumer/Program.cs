using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;

string rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";

Console.WriteLine(rabbitMqHost);

var factory = new ConnectionFactory { HostName = rabbitMqHost };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var tcs = new TaskCompletionSource<bool>();

var consumer = new EventingBasicConsumer(channel);
consumer.Received += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    await ProcessMessageAsync(message);
};
channel.BasicConsume(queue: "hello",
                     autoAck: true,
                     consumer: consumer);

await tcs.Task;

async Task ProcessMessageAsync(string message)
{
    // Your custom message processing logic here
    // For example, you can send an email, persist the message to a database, or perform any other relevant operations.
    Console.WriteLine($" [x] Processed {message}");
}