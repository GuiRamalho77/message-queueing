using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Queue.Domain.Interface;
using Queue.Domain.Models;
using RabbitMQ.Client;

namespace Queue.CarWriter
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
                var channel = _connection.CreateModel();
                _rabbitService.DeclareQueue(ref channel);

                var rand = new Random();
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                    var jsonCarro = JsonSerializer.Serialize(new Car { Brand = "Teste", Model = $"Ts {rand.Next(1, int.MaxValue)}" });
                    _rabbitService.CreateQueue(jsonCarro, ref channel);
                    _logger.LogInformation($"Linha Criada: {jsonCarro}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"ERRO NA CRIAÇÃO DA LINHA");
                _logger.LogError($"ERRO:{ex.Message}", ex);
                throw;
            }
        }

        private Car FindCar()
        {
            var listaCarros = new List<Car>(_carService.GetCars().Result);
            if (listaCarros.Count < 0) return null;
            return listaCarros.First();
        }
    }
}
