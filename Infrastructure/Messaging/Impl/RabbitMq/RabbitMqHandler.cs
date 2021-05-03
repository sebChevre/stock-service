using System;
using RabbitMQ.Client;
using System.Text;
using BeerApi.Services.Event;

namespace BeerApi.Infrastructure.Messaging.Impl.RabbitMq
{


    public class RabbitMqHandler
    {


        private readonly ConnectionFactory _connectionFactory;

        public RabbitMqHandler(){
            _connectionFactory = new ConnectionFactory() { HostName = "localhost" };
        }
        public void Send(string beerId)
        {
            
            using(var connection = _connectionFactory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var b = new BeerCreatedEvent(){
                    BeerId = beerId,
                    Location = "/api/beer/" + beerId
                    
                };

                string message = b.AsJson();

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                    routingKey: "hello",
                                    basicProperties: null,
                                    body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

        }

    }

}