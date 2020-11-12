using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Queue.CarReader;
using Queue.Domain.Extensions;

namespace TesteFilas.Leitor
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
                    services.AddRabbitConnection("amqp://localhost");
                    services.AddCarService();
                    //services.AddRabbitService();
                    services.AddHostedService<Worker>();

                });
    }
}
