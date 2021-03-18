using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlazorEntre2Ages.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BlazorEntre2Ages.Models
{
    public class Rabbit : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly RabbitSettings _settings;
        private readonly string _queueName;
        public Rabbit(IChatService chatService, IMessageService messageService, IOptionsMonitor<RabbitSettings> monitor)
        {
            _chatService = chatService;
            _messageService = messageService;
            _settings = monitor.CurrentValue;
            var factory = new ConnectionFactory()
            {
                HostName = _settings.Host,
                Port = _settings.Port
            };
            
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange:"entre2ages", type:ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(_queueName, exchange:"entre2ages", routingKey:"");
        }
        
        public async Task SendMessage(Message message)
        {
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish(exchange: "entre2ages",
                routingKey: "",
                basicProperties: null,
                body: body);
            await _messageService.Send(message);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Message message = null;

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                message = JsonConvert.DeserializeObject<Message>(json);
                HandleMessage(message);
            };
            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            return Task.CompletedTask;
        }

        protected void HandleMessage(Message message)
        {
            _chatService.HandleMessage(message);
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

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }

    }
}
