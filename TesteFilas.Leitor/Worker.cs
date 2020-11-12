using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Queue.Domain.Configurations;
using Queue.Domain.Extensions;
using Queue.Domain.Interface;
using Queue.Domain.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Queue.CarReader
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConnection _connection;
        private readonly ICarService _carService;
        private readonly IRabbitService _rabbitService;


        public Worker(ILogger<Worker> logger, IConnection connection, ICarService carService, IRabbitService rabbitService)
        {
            _logger = logger;
            _connection = connection;
            _carService = carService;
            _rabbitService = rabbitService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                ConsumerQueue();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR IN READER:{ex.Message}", ex);
                throw;
            }
        }

        private void ConsumerQueue()
        {
            var channel = _connection.CreateModel();
            _rabbitService.DeclareQueue(ref channel);
            var consumer = _rabbitService.ConsumerQueue(ref channel);
            consumer.Received += (sender, response) =>
            {
                var model = sender as EventingBasicConsumer;
                ProcessMessage(consumer.Model, response.Body, response.DeliveryTag);
            };
            channel.BasicConsume(RabbitMQConfiguration.QueueName, false, consumer);
        }

        private async Task ReadMessage(IModel channel, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
                var msg = channel.BasicGet(queue: RabbitMQConfiguration.QueueName, false);
                ProcessMessage(channel, msg.Body, msg.DeliveryTag);
            }
        }

        private void ProcessMessage(IModel channel, ReadOnlyMemory<byte> body, ulong deliveryTag)
        {
            try
            {
                var carObj = Common.DeserializeMessage<Car>(body.ToArray());
                if (carObj == null) throw new Exception();
                _rabbitService.QueueBasicAck(deliveryTag, false, ref channel);
                _logger.LogInformation($"Lido: {JsonSerializer.Serialize(carObj)}");
                SendCarToServer(carObj);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Erro na Leitura : {JsonSerializer.Deserialize<dynamic>(body.ToArray())}");
                _logger.LogError($"ERRO:{ex.Message}", ex);
                _rabbitService.QueueBasicNack(deliveryTag, false, true, ref channel);
                throw;
            }
        }

        private void SendCarToServer(Car modelCar)
        {
            var enviado = _carService.PostCarro(modelCar).Result;
            if (enviado)
                _logger.LogInformation($"CAR SENT: {JsonSerializer.Serialize(modelCar)}");
            else
                _logger.LogInformation($"ERROR SEND CAR: {JsonSerializer.Serialize(modelCar)}");
        }
    }

}
