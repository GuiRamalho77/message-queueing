namespace Queue.Domain.Configurations
{
    public class RabbitMQConfiguration
    {
        public const string QueueName = "Test.Queue";
        public const bool Durable = true;
        public const bool Exclusive = false;
        public const bool AutoDelete = false;
    }
}
