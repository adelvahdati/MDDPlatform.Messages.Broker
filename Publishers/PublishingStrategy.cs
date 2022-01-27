namespace MDDPlatform.Messages.Broker.Publishers
{

    public class PublishingStrategy : IPublishingStrategy
    {
        private readonly string _exchange;
        private readonly string _exchangeType;
        private readonly string _routingKey;
        private readonly string _queue;

        public string Exchange => _exchange;

        public string RoutingKey => _routingKey;

        public string Queue => _queue;

        public string ExchangeType => _exchangeType;

        public PublishingStrategy(string exchange, string exchangeType, string routingKey, string queue)
        {
            _exchange = exchange;
            _exchangeType = exchangeType;
            _routingKey = routingKey;
            _queue = queue;
        }
    }
}