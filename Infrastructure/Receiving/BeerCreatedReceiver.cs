
using System.Threading;
using System.Threading.Tasks;
using StockApi.Infrastructure.MongoDB;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System;
using StockApi.Infrastructure.Configuration;
using MongoDB.Driver;
using StockApi.Models;
using System.Text.Json;
using BeerApi.Services.Event;

namespace StockApi.Infrastructure.Receiving
{

    public class BeerCreatedReceicer : BackgroundService
    {
        private readonly ConnectionFactory _connectionFactory;

        //private readonly StockService _stockService;

        private IConnection _connection;

        
        private IModel _channel;
        private readonly MongoDBHandler _mongoDBHandler;

        private readonly IMongoCollection<Stock> _stocks;
         string _rabbitMqHost;
        int _rabbitMqPort;

        public BeerCreatedReceicer(IStockstoreDatabaseSettings settings){
            
            _mongoDBHandler = new MongoDBHandler(settings);
            _stocks =  _mongoDBHandler.GetDataBase().GetCollection<Stock>(settings.StockCollectionName);
            _rabbitMqHost =  Environment.GetEnvironmentVariable("RABBITMQ_HOST");
            _rabbitMqPort = Int32.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"));

            _connectionFactory = new ConnectionFactory() { HostName = _rabbitMqHost, Port = _rabbitMqPort };
            InitializeRabbitMqListener();
        }

         private void InitializeRabbitMqListener()
        {
            
            _connection = _connectionFactory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
            
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                

                HandleMessage(content);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            _channel.BasicConsume("hello", false, consumer);

            return Task.CompletedTask;
        }

        private void HandleMessage(string msg)
        {
            
            BeerCreatedEvent beerCreatedEvent = JsonSerializer.Deserialize<BeerCreatedEvent>(msg);
            Console.WriteLine(beerCreatedEvent.AsJson());

            Stock stock = new Stock(){
                BeerId = beerCreatedEvent.BeerId,
                StockDisponible = 0,
                StockReserve = 0
            };

            _stocks.InsertOne(stock);
           
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }
    }

}