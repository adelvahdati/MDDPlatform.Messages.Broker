using MDDPlatform.Messages.Core;

namespace MDDPlatform.Messages.Broker.Subscribers
{
    public abstract class SubscriptionStrategyBuilder<T> where T:IMessage{
        protected abstract string Exchange();
        protected abstract string ExchangeType();
        protected abstract string RoutingKey();
        protected abstract string Queue();
        public ISubscriptionStrategy Build()
        {
            var exchange = Exchange();
            var exchangeType = ExchangeType();
            var queue = Queue();
            var routingKey = RoutingKey();
            return new SubscriptionStrategy(exchange,exchangeType,routingKey,queue);
        }
    }
}