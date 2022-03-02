using MDDPlatform.Messages.Brokers.Configurations;
using MDDPlatform.Messages.Brokers.Options;
using MDDPlatform.Messages.Core;

namespace MDDPlatform.Messages.Brokers.Publishers
{
    public class RoutingPolicy
    {
        public string MessageType { get; set; }
        public string ExchangeTemplate { get; set; }
        public string RoutingKeyTemplate { get; set; }
        public string QueueTemplate { get; set; }
        public string ExchangeType { get; set; }

        public RoutingPolicy()
        {
            MessageType=string.Empty;
            ExchangeTemplate = string.Empty;
            RoutingKeyTemplate =string.Empty;
            QueueTemplate = string.Empty;
            ExchangeType = "topic";
        }
        public RoutingPolicy(string messageType, string exchangeTemplate, string routingKeyTemplate, string queueTemplate, string exchangeType)
        {
            MessageType = messageType;
            ExchangeTemplate = exchangeTemplate;
            RoutingKeyTemplate = routingKeyTemplate;
            QueueTemplate = queueTemplate;
            ExchangeType = exchangeType;
        }

        internal IChannelAttributes Resolve<TMessage>(TMessage message) where TMessage : IMessage
        {
            var registery  = new PlaceholderRegistery();
            string exchange = registery.ResolvePlaceholder<TMessage>(ExchangeTemplate,message);
            string routingKey = registery.ResolvePlaceholder<TMessage>(RoutingKeyTemplate,message);
            string queue = registery.ResolvePlaceholder<TMessage>(QueueTemplate,message);
            return new ChannelAttributes(exchange,routingKey,queue,ExchangeType);
        }
        internal IChannelAttributes Resolve(Type type , IMessage message)
        {
            var registery  = new PlaceholderRegistery();
            string exchange = registery.ResolvePlaceholder(type,ExchangeTemplate,message);
            string routingKey = registery.ResolvePlaceholder(type,RoutingKeyTemplate,message);
            string queue = registery.ResolvePlaceholder(type,QueueTemplate,message);
            return new ChannelAttributes(exchange,routingKey,queue,ExchangeType);
        }
    }
}