using MDDPlatform.Messages.Core;
using MDDPlatform.Messages.Wraper;

namespace MDDPlatform.Messages.Broker.Publishers
{
    public interface IMessagePublisher
    {
        void Publish(string message,string exchange, string routingKey,string exchangeType);
        Task PubblishAsync<T>(WrappedMessage<T> message,IPublishingStrategy strategy) where T : IMessage;
        Task PubblishAsync<T>(WrappedMessage<T> message) where T : IMessage;
        
    }
}