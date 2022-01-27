using System.Linq.Expressions;
using MDDPlatform.Messages.Core;

namespace MDDPlatform.Messages.Broker.Publishers
{
    public abstract class PublishingStrategyBuilder<T> where T : IMessage
    {
        protected abstract Expression<Func<T, string>> ExchangeExpression();
        protected abstract Expression<Func<T, string>> ExchangeTypeExpression();
        protected abstract Expression<Func<T, string>> RoutingKeyExpression();
        protected abstract Expression<Func<T, string>> QueueExpression();

        protected string Exchange(T message)
        {
            Func<T, string> exchangeFunc = ExchangeExpression().Compile();
            return exchangeFunc(message);
        }

        protected string ExchangeType(T message)
        {
            Func<T, string> exchangeTypeFunc = ExchangeTypeExpression().Compile();
            return exchangeTypeFunc(message);
        }

        protected string RoutingKey(T message)
        {
            Func<T, string> routingKeyFunc = RoutingKeyExpression().Compile();
            return routingKeyFunc(message);
        }
        protected string Queue(T message)
        {
            Func<T, string> queueFunc = QueueExpression().Compile();
            return queueFunc(message);
        }

        public IPublishingStrategy Build(T message)
        {
            var exchange = Exchange(message);
            var exchangeType = ExchangeType(message);
            var queue = Queue(message);
            var routingKey = RoutingKey(message);
            return new PublishingStrategy(exchange,exchangeType,routingKey,queue);
        }
    }
}