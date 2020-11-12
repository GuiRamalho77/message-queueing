using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Queue.CarWriter;
using Queue.Domain.Extensions;
using Queue.Domain.Interface;
using Queue.Domain.Services;

namespace Queue.Writer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddRabbitConnection("amqp://localhost");
                    services.AddRabbitConnection();
                    //services.AddRabbitService();
                    services.AddCarService("http://localhost:3000/api/carros/");
                    services.AddHostedService<Worker>();
                });
    }
}
