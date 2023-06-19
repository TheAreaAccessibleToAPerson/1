namespace Gudron.rabbitMQ
{
    public class PublisherSettings
    {
        public string HostName{init;get;} = "";

        public string Exchange {init;get;} = "";
        public string RoutingKey {init;get;} = "";
        public RabbitMQ.Client.IBasicProperties BasicProperties {init;get;} = null;

        public string QueueName {init;get;} = "";
        public bool IsDurable{init;get;} = false;
        public bool IsExclusive{init;get;} = false;
        public bool IsAutoDelete{init;get;} = false;
        public IDictionary<string, object> Arguments{init;get;} = null;
    }
}