using System.Runtime.ExceptionServices;
using System;
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
            obj<rabbitMQ.consumer.Default, string>("RabbitMQ.Consumer.Default",
                new rabbitMQ.ConsumerSettings
                {
                    QueueName = "dev_queue",
                    RoutingKey = "dev_queue"
                })
                .output_to((message) => 
                {
                    Console("HELLO");
                });

/*
            obj<DBManager, string>("ClientManager")
                .output_to(obj<rabbitMQ.publisher.Default>("RabbitMQ.Publisher.Direct",
                    new rabbitMQ.PublisherSettings
                    {
                    })
                    .Send);
                    */
        }
    }

    public sealed class MainServer : Controller
    {
        BlockingCollection<RequestDB> _requestsDB = new BlockingCollection<RequestDB>();
        IInput<RequestDB> _inputToDB;

        void Construction()
        {
            listen_message<RequestDB>("RequestDB")
                .output_to(_requestsDB.Add);

            input_to<RequestDB> 
                (ref _inputToDB, obj<DBManager>("DBManager").Add);

            obj<DBManager, string>("DBManager")
                .output_to(obj<rabbitMQ.publisher.Default>("RabbitMQ.Publisher.Default",
                    new rabbitMQ.PublisherSettings
                    {
                        HostName = "LocalHost",
                        RoutingKey = "dev-queue1",
                    })
                    .Send);

            /*
            obj<rabbitMQ.consumer.Direct, string>("RabbitMQ.Consumer.Direct",
                new rabbitMQ.ConsumerSettings
                {

                })
                .output_to(obj<DBManager>("DBManager").Add);
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
