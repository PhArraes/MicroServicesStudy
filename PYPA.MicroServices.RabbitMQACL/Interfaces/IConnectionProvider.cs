using RabbitMQ.Client;

namespace PYPA.MicroServices.RabbitMQACL
{
    public interface IConnectionProvider
    {
        IConnection GetConnection();
    }
}