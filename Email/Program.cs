using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;

// Connect to RabbitMQ
string rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
string password = Environment.GetEnvironmentVariable("PASSWORD");
string username = Environment.GetEnvironmentVariable("USERNAME");

var factory = new ConnectionFactory() { HostName = rabbitMqHost };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Declare a queue
channel.QueueDeclare(queue: "email",
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

channel.BasicConsume(queue: "email",
                     autoAck: true,
                     consumer: consumer);

await tcs.Task;

async Task ProcessMessageAsync()
{
    var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
    {
        Credentials = new NetworkCredential(username, password),
        EnableSsl = true
    };
    client.Send("from@example.com", "to@example.com", "Hello world", "Hello from micro service");

    Console.WriteLine("Email sent.");
}
