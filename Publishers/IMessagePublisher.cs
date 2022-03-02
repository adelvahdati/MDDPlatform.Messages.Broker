using MDDPlatform.Messages.Core;
using MDDPlatform.Messages.Wrapers;

namespace MDDPlatform.Messages.Brokers.Publishers
{
    public interface IMessagePublisher
    {
        void Publish(string message,string exchange, string routingKey,string exchangeType);
        Task PubblishAsync<T>(WrappedMessage<T> message) where T : IMessage;
        
    }
}