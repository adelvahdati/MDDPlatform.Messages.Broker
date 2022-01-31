using MDDPlatform.Messages.Broker.Options;

namespace MDDPlatform.Messages.BrokerConfiguration 
{
    public class ChannelAttributes 
    {
        // private readonly string _exchange;
        // private readonly string _routingKey;
        // private readonly string _queue;
        // private readonly string _exchangeType;

        public string Exchange {get; set;}

        public string RoutingKey {get; set;}

        public string Queue {get; set;}

        public string ExchangeType {get; set;}
        public ChannelAttributes(string exchange,string routingKey,string queue,string exchangeType)
        {
            Exchange = exchange;
            RoutingKey = routingKey;
            Queue = queue;
            ExchangeType = exchangeType;
        }

    }
}