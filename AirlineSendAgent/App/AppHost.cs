using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using AirlineSendAgent.Clients;
using AirlineSendAgent.Data;
using AirlineSendAgent.Dtos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AirlineSendAgent.App
{

    public class AppHost : IAppHost
    {
        private readonly SendAgentDbContext _context;
        private readonly IWebHookClient _webhookClient;

        public AppHost(SendAgentDbContext context, IWebHookClient webHookClient)
        {
            _context = context;
            _webhookClient = webHookClient;
        }
        public void Run()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName,
                    exchange: "trigger",
                    routingKey: ""
                );

                var consumer = new EventingBasicConsumer(channel);
                Console.WriteLine("Listening on the message bus...");

                consumer.Received += async (ModuleHandle, ea) =>
                {
                    Console.WriteLine("Event is triggered!");

                    var body = ea.Body;
                    var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                    var message = JsonSerializer.Deserialize<NotificationMessageDto>(notificationMessage);

                    var webhookToSend = new FlightDetailChangePayloadDto()
                    {
                        WebhookType = message.WebhookType,
                        WebhookURI = string.Empty,
                        Secret = string.Empty,
                        Publisher = string.Empty,
                        OldPrice = message.OldPrice,
                        NewPrice = message.NewPrice,
                        FlightCode = message.FlightCode
                    };

                    foreach (var whs in _context.WebhookSubscriptions.Where(subs => subs.WebhookType.Equals(message.WebhookType)))
                    {
                        webhookToSend.WebhookURI = whs.WebhookURI;
                        webhookToSend.Secret = whs.Secret;
                        webhookToSend.Publisher = whs.WebhookPublisher;

                        await _webhookClient.SendWebHook(webhookToSend);
                    }

                };

                channel.BasicConsume(queue: queueName,
                    autoAck: true,
                    consumer: consumer);
                Console.ReadLine();
            }
        }
    }
}