using System.Collections.Concurrent;

namespace Gudron
{
    public sealed class Test : Controller 
    {
        void Construction()
        {
            obj<MainServer>("MainServer");
            obj<DBServer>("DBServer");
        }
    }

    public sealed class DBServer : Controller 
    {
        void Construction()
        {
            obj<rabbitMQ.consumer.Direct, string>("RabbitMQ.Consumer.Direct",
                new rabbitMQ.ConsumerSettings
                {
                })
                .output_to(obj<PersonManager>("ClientManager").Add);

            obj<PersonManager, string>("ClientManager")
                .output_to(obj<rabbitMQ.publisher.Direct>("RabbitMQ.Publisher.Direct",
                    new rabbitMQ.PublisherSettings
                    {
                    })
                    .Send);
        }
    }

    public sealed class MainServer : Controller
    {
        BlockingCollection<RequestDB<Person>> _requestsDB = new BlockingCollection<RequestDB<Person>>();
        IInput<RequestDB<Person>> _inputToDB;

        void Construction()
        {
            listen_message<RequestDB<Person>>("RequestDB")
                .output_to(_requestsDB.Add);
            /*
            input_to<RequestDB, int> 
                (ref inputToClientManager, obj<ClientManager>("ClientManager").Add);

            obj<ClientManager, string>("ClientManager")
                .output_to(obj<rabbitMQ.publisher.Direct>("RabbitMQ.Publisher.Direct",
                    new rabbitMQ.PublisherSettings
                    {

                    })
                    .Send);

            obj<rabbitMQ.consumer.Direct, string>("RabbitMQ.Consumer.Direct",
                new rabbitMQ.ConsumerSettings
                {

                })
                .output_to(obj<ClientManager>("ClientManager").Add);
                */
        }

        void Start()
        {
            obj<Receive>("Receive", new Receive.Settings()
            {
                //...
            });

            add_thread("RequestDB.Update", () =>
            {
                if (_requestsDB.Count > 0)
                    _inputToDB.To(_requestsDB.Take());
            },
            1, Thread.Priority.Highest);
        }
    }
}
