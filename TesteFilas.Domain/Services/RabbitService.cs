using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Queue.Domain.Configurations;
using Queue.Domain.Extensions;
using Queue.Domain.Interface;
using Queue.Domain.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Queue.Domain.Services
{
    public class RabbitService : IRabbitService
    {
        //private readonly IConnection _connection;

        //public RabbitService(IConnection connection)
        //{
        //    _connection = connection;
        //}

        public void DeclareQueue(ref IModel channel) =>
            channel.QueueDeclare(queue: RabbitMQConfiguration.QueueName, durable: RabbitMQConfiguration.Durable, exclusive: RabbitMQConfiguration.Exclusive,
                autoDelete: RabbitMQConfiguration.AutoDelete, arguments: null);

        public void CreateQueue(string jsonModel, ref IModel channel) =>
            channel.BasicPublish(exchange: string.Empty, routingKey: RabbitMQConfiguration.QueueName, basicProperties: null,
                body: Encoding.UTF8.GetBytes(jsonModel));


        public EventingBasicConsumer ConsumerQueue(ref IModel channel)
        {
            channel.BasicQos(0, 1, false);
            return new EventingBasicConsumer(channel);
        }

        public void QueueBasicAck(ulong deliveryTag, bool multiple, ref IModel channel) => channel.BasicAck(deliveryTag, multiple);
        public void QueueBasicNack(ulong deliveryTag, bool multiple, bool requeue, ref IModel channel) => channel.BasicNack(deliveryTag, multiple, requeue);

    }
}
