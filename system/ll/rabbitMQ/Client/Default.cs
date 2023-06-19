using RabbitMQ.Client;

/// <summary>
/// Оправитель. 
/// </summary>
namespace Gudron.rabbitMQ.publisher
{
    /// <summary>
    /// Простой обменик.
    /// Сюда поподают сообщение которые не подошли не одному обменику по роутигу. 
    /// Если никак не подписывать оправляемое сообщение в поле exchange сообщение 
    /// будут поподать в DefaultExchange.
    /// Он неявно связан со всеми очередями по routingKey который должен быть равен
    /// имени очереди и невозможно привязать данный обменик к какой либо очереди.
    ///
    /// Сообщение будут поподать в данный обменик если в параметр имени exchenge
    /// будет отправляться пустая строка.
    ///
    /// Суть обменика по умолчанию смотреть в параметр routingKey и пытаться отыскать
    /// очередь с таким названием куда и будет отправленно сообщение.
    /// </summary>
    public class Default : Controller.LocalField<PublisherSettings>
    {
        private IConnection _connection;
        private IModel _channel;

        void Construction()
        {
        }

        void Start()
        {
        }

        public void Send(string message)
        {
            // Публикуем сообщение в очередь.
            _channel.BasicPublish(exchange: "",// Если пустая строка, то будет отправлятся в Default.
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

            // Обьявляем очередь куда мы и будем 
            // публиковать наши сообщения.
            _channel.QueueDeclare(queue: Field.QueueName,
                                 durable: Field.IsDurable,
                                 exclusive: Field.IsExclusive,
                                 autoDelete: Field.IsAutoDelete,
                                 arguments: Field.Arguments);
        }

        void Stop()
        {

        }
    }
}