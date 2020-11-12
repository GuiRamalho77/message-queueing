using System;
using Microsoft.Extensions.DependencyInjection;
using Queue.Domain.Interface;
using Queue.Domain.Services;
using Queue.Service.Interfaces;
using Queue.Service.Services;
using RabbitMQ.Client;

namespace Queue.Domain.Extensions
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddRabbitConnection(this IServiceCollection services, string rabbitUri = null)
        {
            try
            {
                var connectionFactory = new ConnectionFactory
                {
                    Uri = new Uri(ProcessUrlString("RABBIT_URI", rabbitUri))
                };
                var connection = connectionFactory.CreateConnection();
                services.AddSingleton(connection);
                services.AddSingleton<IRabbitService, RabbitService>();
                return services;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static IServiceCollection AddCarService(this IServiceCollection services, string urlApi = null)
        {
            try
            {
                var url = ProcessUrlString("API_SERVICE_BASE_URL", urlApi);
                services.AddSingleton<IApplicationService, ApplicationService>(a => new ApplicationService(url));
                services.AddSingleton<ICarService, CarService>();
                return services;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        //public static IServiceCollection AddRabbitService(this IServiceCollection services)
        //{
        //    try
        //    {
        //        services.AddSingleton<IRabbitService, RabbitService>();
        //        return services;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw;
        //    }
        //}

        private static string ProcessUrlString(string envName, string url = null)
        {
            var envUrl = Environment.GetEnvironmentVariable(envName);
            if (url.IsNullOrEmpty() && envUrl.IsNullOrEmpty())
                throw new ArgumentNullException(envName);
            return url.IsNullOrEmpty() ? envUrl : url;
        }
    }
}
