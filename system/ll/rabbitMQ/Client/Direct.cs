using RabbitMQ.Client;
using RabbitMQ.Client.Events;

/// <summary>
/// Оправитель. 
/// </summary>
namespace Gudron.rabbitMQ.publisher
{
    public class Direct : Controller.LocalField<PublisherSettings>
    {
        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _consumer;

        public void Send(string message)
        {
            // Публикуем сообщение в очередь.
            _channel.BasicPublish(exchange: Field.Exchange,
                                routingKey: Field.RoutingKey,
                                basicProperties: Field.BasicProperties,
                                body: System.Text.Encoding.UTF8.GetBytes(message));
        }

        void Configurate()
        {
            // Будет управлять версией протокола, аутентификацией и тд.
            _connection = new ConnectionFactory()

            // Настраиваем хост.
            { HostName = Field.HostName }
                    // Управляет версией протокола аутентификации.
                    .CreateConnection();

            // Создает канал.
            // Сдесь будет выполнятся основная работа с очередью.
            _channel = _connection.CreateModel();

            // Маршрутизация осущесвляется по параметру routingKey.
            _channel.ExchangeDeclare(exchange: Field.Exchange, type: ExchangeType.Direct);
        }
    }
}