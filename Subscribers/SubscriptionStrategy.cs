namespace MDDPlatform.Messages.Broker.Subscribers {
    public class SubscriptionStrategy : ISubscriptionStrategy
    {
        private readonly string _exchange;
        private readonly string _exchangeType;
        private readonly string _routingKey;
        private readonly string _queue;

        public string Exchange => _exchange;

        public string RoutingKey => _routingKey;

        public string Queue => _queue;

        public string ExchangeType => _exchangeType;

        public SubscriptionStrategy(string exchange, string exchangeType, string routingKey, string queue)
        {
            _exchange = exchange;
            _exchangeType = exchangeType;
            _routingKey = routingKey;
            _queue = queue;
        }
    }
}