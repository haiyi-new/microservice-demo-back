using System;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;


namespace ApiGateway.Controllers
{



    [ApiController]
    [Route("/not")]

    public class NonACertificateController : ControllerBase

    {


        string rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        private readonly ConnectionFactory _connectionFactory;

        public NonACertificateController()
        {
            _connectionFactory = new ConnectionFactory { HostName = rabbitMqHost };
        }

        [HttpGet]
        public IActionResult GetStatement()
        {
            string statement = "This is a Non Authenticate Page";

            try
            {
                HelperFunctions.PublishDemoTask(_connectionFactory);
                return Ok(statement);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the process
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }
    }

}