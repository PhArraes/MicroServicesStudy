namespace PYPA.MicroServices.RabbitMQACL
{
    public interface IPublisher
    {
        void AddTask();
        void Publish(string message);
    }
}