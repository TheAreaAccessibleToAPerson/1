using RabbitMQ.Client;
using RabbitMQ.Client.Events;

/// <summary>
/// Получатель. 
/// </summary>
namespace Gudron.rabbitMQ.consumer
{
    public class Direct : Controller.Output<string>.LocalField<ConsumerSettings>
    {
        IConnection _connection; 
        IModel _channel; 
        EventingBasicConsumer _consumer;

        void Received(object obj, BasicDeliverEventArgs e)
        {
            output(System.Text.Encoding.UTF8.GetString(e.Body.ToArray()));
        }

        void Configurate()
        {
            _connection = new ConnectionFactory()
            // Настраиваем хост.
            { HostName = "LocalHost" }
                    // Управляет версией протокола аутентификации.
                    .CreateConnection();

            // Создает канал.
            // Сдесь будет выполнятся основная работа с очередью.
            _channel = _connection.CreateModel();

            // Маршрутизация осущесвляется по параметру routingKey.
            _channel.ExchangeDeclare(exchange: Field.Exchange, type: ExchangeType.Direct);

            var queueName = _channel.QueueDeclare().QueueName;

            // Свяжим нашу очередь с обмеником.
            _channel.QueueBind(queue: queueName,
                                    exchange: Field.Exchange,
                                    routingKey: Field.RoutingKey);

            _consumer = new EventingBasicConsumer(_channel);

            // Получаем сообщение.
            _consumer.Received += Received;

            // Свяжим с очередью.
            _channel.BasicConsume(queue: queueName,
                                 autoAck: Field.IsAutoAck, 
                                 consumer: _consumer);
        }

        void Stop()
        {

        }
    }
}