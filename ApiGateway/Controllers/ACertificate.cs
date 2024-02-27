using System;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using io.fusionauth;
using Newtonsoft.Json;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("/")]
    public class ACertificateController : ControllerBase
    {

        string rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";

        private readonly ConnectionFactory _connectionFactory;

        public ACertificateController()
        {
            _connectionFactory = new ConnectionFactory { HostName = rabbitMqHost };
        }

        [HttpGet]
        public async Task<IActionResult> GetACertificate()
        {
            // Get the authorization header from the request
            string authorizationHeader = HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                // No authorization header provided
                return Unauthorized();
            }

            FusionAuthClient fusionAuthClient = new FusionAuthClient("b4p8Uhwx9HNo_KIGSrYKp2qdyy9_BmeF31HSsInBS3ktQq7JWGicFheh", "http://192.168.0.196:9011");


            // Access the bearer token from the Authorization header
            string bearerToken = HttpContext.Request.Headers["Authorization"];

            // Remove the "Bearer " prefix from the token
            string token = bearerToken.Replace("Bearer ", "");


            var a = await fusionAuthClient.IntrospectAccessTokenAsync("a92d900d-d460-4baa-b019-149894547bda", token);

            // Use the token as needed
            Console.WriteLine(a.successResponse["active"].GetType());

            if (!(bool)a.successResponse["active"])
            {
                return Unauthorized();
            }
            Console.WriteLine(a.successResponse);



            try
            {
                HelperFunctions.PublishDemoTask(_connectionFactory);
                return Ok("SMTP Mail and Report.TXT has been made");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the process
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
