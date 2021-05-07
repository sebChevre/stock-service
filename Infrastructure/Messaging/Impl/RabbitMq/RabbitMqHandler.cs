using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace StockApi.Infrastructure.Messaging.Impl.RabbitMq
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

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };

                channel.BasicConsume(queue: "hello",
                                    autoAck: true,
                                    consumer: consumer);
                }

        }

    }

}