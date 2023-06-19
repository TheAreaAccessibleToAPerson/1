namespace Gudron.rabbitMQ
{
    public class ConsumerSettings
    {
        public string HostName{set;get;} = "";

        public string Exchange {set;get;} = "";
        public string RoutingKey {set;get;} = "";
        public RabbitMQ.Client.IBasicProperties BasicProperties {set;get;} = null;

        public string QueueName {set;get;} = "";
        public bool IsDurable{set;get;} = false;
        public bool IsExclusive{set;get;} = false;

        /// <summary>
        /// true: Очередь создается во время подключения первого клинта и удаляется в момент когда все клинты 
        /// отсоединились.
        /// </summary>
        /// <value></value>
        public bool IsAutoDelete{set;get;} = false;
        public IDictionary<string, object> Arguments{set;get;} = null;

        public bool IsAutoAck{set;get;} = true;
    }
}