using MDDPlatform.Messages.Broker.Options;
using MDDPlatform.Messages.BrokerConfiguration;
using MDDPlatform.Messages.Core;

namespace MDDPlatform.Messages.Broker.Publishers
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
            Console.WriteLine(string.Format("---> Routing Policy Resolved : Exchange = {0}, RoutingKey= {1} , ExchageType = {2}",exchange,routingKey,ExchangeType));
            return new ChannelAttributes(exchange,routingKey,queue,ExchangeType);
        }
    }
}