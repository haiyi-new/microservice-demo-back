using System;
using System.Text;
// using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
public static class HelperFunctions
{
    public static void PublishDemoTask(ConnectionFactory connectionFactory)
    {

        // Establish the RabbitMQ connection
        using (var connection = connectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
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
            const string reportMessage = "Report.txt has been made";
            var reportBody = Encoding.UTF8.GetBytes(reportMessage);
            channel.BasicPublish(exchange: "",
                         routingKey: "report",
                         basicProperties: null,
                         body: reportBody);

            // Publish a message to the "email" queue
            const string emailMessage = "SMTP Mail Sent";
            var emailBody = Encoding.UTF8.GetBytes(emailMessage);
            channel.BasicPublish(exchange: "",
                         routingKey: "email",
                         basicProperties: null,
                         body: emailBody);
        }




    }
}