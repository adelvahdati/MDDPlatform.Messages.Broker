using MDDPlatform.Messages.Brokers.Configurations;
using MDDPlatform.Messages.Brokers.Options;
using MDDPlatform.Messages.Core;

namespace MDDPlatform.Messages.Brokers.Subscribers
{
    public class BindingPolicy
    {
        public string MessageType { get; set; }
        public string ExchangeTemplate { get; set; }
        public string RoutingKeyTemplate { get; set; }
        public string QueueTemplate { get; set; }
        public string ExchangeType { get; set; }

        public BindingPolicy()
        {
            MessageType=string.Empty;
            ExchangeTemplate = string.Empty;
            RoutingKeyTemplate =string.Empty;
            QueueTemplate = string.Empty;
            ExchangeType = "topic";
        }
        public BindingPolicy(string messageType, string exchangeTemplate, string routingKeyTemplate, string queueTemplate, string exchangeType)
        {
            MessageType = messageType;
            ExchangeTemplate = exchangeTemplate;
            RoutingKeyTemplate = routingKeyTemplate;
            QueueTemplate = queueTemplate;
            ExchangeType = exchangeType;
        }

        internal IChannelAttributes Resolve<TMessage>() where TMessage : IMessage
        {
            var registery  = new PlaceholderRegistery();
            string exchange = registery.ResolvePlaceholder<TMessage>(ExchangeTemplate);
            string routingKey = registery.ResolvePlaceholder<TMessage>(RoutingKeyTemplate);
            string queue = registery.ResolvePlaceholder<TMessage>(QueueTemplate);

            return new ChannelAttributes(exchange,routingKey,queue,ExchangeType);
        }

        internal IChannelAttributes Resolve(Type type)
        {
            var registery  = new PlaceholderRegistery();
            string exchange = registery.ResolvePlaceholder(type,ExchangeTemplate);
            string routingKey = registery.ResolvePlaceholder(type,RoutingKeyTemplate);
            string queue = registery.ResolvePlaceholder(type,QueueTemplate);

            return new ChannelAttributes(exchange,routingKey,queue,ExchangeType);

        }
    }
}