using MDDPlatform.Messages.Broker.Options;
using MDDPlatform.Messages.BrokerConfiguration;
using MDDPlatform.Messages.Core;

namespace MDDPlatform.Messages.Broker.Subscribers
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

        internal ChannelAttributes Resolve<TMessage>() where TMessage : IMessage
        {
            var registery  = new PlaceholderRegistery();
            string exchange = registery.GetPlaceholder(ExchangeTemplate).Resolve<TMessage>();
            string routingKey = registery.GetPlaceholder(RoutingKeyTemplate).Resolve<TMessage>();
            string queue = registery.GetPlaceholder(QueueTemplate).Resolve<TMessage>();

            return new ChannelAttributes(exchange,routingKey,queue,ExchangeType);

        }
    }
}