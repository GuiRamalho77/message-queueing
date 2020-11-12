using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Queue.Domain.Interface
{
    public interface IRabbitService
    {
        void DeclareQueue(ref IModel channel);
        void CreateQueue(string jsonModel, ref IModel channel);
        EventingBasicConsumer ConsumerQueue(ref IModel channel);
        void QueueBasicAck(ulong deliveryTag, bool multiple, ref IModel channel);
        void QueueBasicNack(ulong deliveryTag, bool multiple, bool requeue, ref IModel channel);
    }
}
