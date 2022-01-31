using MDDPlatform.Messages.Core;

namespace MDDPlatform.Messages.Broker.Subscribers
{
    public interface IMessageSubscriber 
    {
        public void Subscribe(string exchange, string exchangeType, string routingKey, Func<string,Task> handler);
        public Task SubscribeAsync<TMessage>(ISubscriptionStrategy strategy) where TMessage : IMessage;
        public Task SubscribeAsync<TMessage>() where TMessage : IMessage;
    }    
}