using MDDPlatform.Messages.Broker.Publishers;
using MDDPlatform.Messages.Broker.Subscribers;
using MDDPlatform.Messages.Core;

namespace MDDPlatform.Messages.BrokerConfiguration {
    public interface IBusConfiguration {
        public string HostName { get; set; }
        public int Port { get; set; }
        public void SetBindingPolicy(BindingPolicy policy);
        public void SetRoutingPolicy(RoutingPolicy policy);
        public BindingPolicy GetBindingPolicy<TMessage>() where TMessage:IMessage;
        public RoutingPolicy GetRoutingPolicy<TMessage>() where TMessage:IMessage;

        public ChannelAttributes ResolveBindingPolicy<TMessage>() where TMessage:IMessage;
        public ChannelAttributes ResolveRoutingPolicy<TMessage>(TMessage message) where TMessage : IMessage;
    }
}