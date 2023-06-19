using RabbitMQ.Client;
using RabbitMQ.Client.Events;

/// <summary>
/// Получатель. 
/// </summary>
namespace Gudron.rabbitMQ.consumer
{
    public class Default : Controller.Output<string>
    {
        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _consumer;

        void Construction()
        { }

        void Start()
        { }

        void Received(object obj, BasicDeliverEventArgs e)
        {
            Console("Receive");
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
            // Обьявляем очередь куда мы и будем 
            // публиковать наши сообщения.
            _channel.QueueDeclare(queue: "dev-queue1",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            _consumer = new EventingBasicConsumer(_channel);

            // Получаем сообщение.
            _consumer.Received += Received;

            // Свяжим с очередью.
            _channel.BasicConsume(queue: "dev-queue1",
                                 autoAck: true,
                                 consumer: _consumer);
        }

        void Stop()
        {

        }
    }
}