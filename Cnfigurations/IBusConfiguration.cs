using MDDPlatform.Messages.Brokers.Publishers;
using MDDPlatform.Messages.Brokers.Subscribers;
using MDDPlatform.Messages.Core;

namespace MDDPlatform.Messages.Brokers.Configurations
{
    public interface IBusConfiguration {
        public string HostName { get; set; }
        public int Port { get; set; }
        public void SetBindingPolicy(BindingPolicy policy);
        public void SetRoutingPolicy(RoutingPolicy policy);
        public BindingPolicy GetBindingPolicy<TMessage>() where TMessage:IMessage;
        public RoutingPolicy GetRoutingPolicy<TMessage>() where TMessage:IMessage;

        public BindingPolicy GetBindingPolicy(Type type);
        public RoutingPolicy GetRoutingPolicy(Type type);


        public IChannelAttributes ResolveBindingPolicy<TMessage>() where TMessage:IMessage;
        public IChannelAttributes ResolveRoutingPolicy<TMessage>(TMessage message) where TMessage : IMessage;

        public IChannelAttributes ResolveBindingPolicy(Type type);
        public IChannelAttributes ResolveRoutingPolicy(Type type, IMessage message);
    }
}