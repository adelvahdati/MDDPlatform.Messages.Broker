using MDDPlatform.Messages.Brokers.Options;
using MDDPlatform.Messages.Brokers.Publishers;
using MDDPlatform.Messages.Brokers.Subscribers;
using MDDPlatform.Messages.Core;


namespace MDDPlatform.Messages.Brokers.Configurations
{
    public class BusConfiguration : IBusConfiguration
    {
        private List<BindingPolicy> _bindingPolicies;
        private List<RoutingPolicy> _routingPolicies;

        public string HostName { get; set; }
        public int Port { get; set; }

        public IReadOnlyCollection<RoutingPolicy> RoutingPolicies => _routingPolicies;
        public IReadOnlyCollection<BindingPolicy> BindingPolicies => _bindingPolicies;

        public BusConfiguration(string hostName, int port)
        {
            HostName = hostName;
            Port = port;
            _bindingPolicies = new List<BindingPolicy>();
            _routingPolicies = new List<RoutingPolicy>();            
        }
        public BusConfiguration(RabbitMQOption option){
            HostName = option.HostName;
            Port = option.Port;
            _bindingPolicies = new List<BindingPolicy>();
            _routingPolicies = new List<RoutingPolicy>();            
            foreach(var routingPolicy in option.routing)
            {
                _routingPolicies.Add(new RoutingPolicy(routingPolicy.MessageType,
                                                        routingPolicy.ExchangeTemplate,
                                                        routingPolicy.RoutingKeyTemplate,
                                                        routingPolicy.QueueTemplate,
                                                        routingPolicy.ExchangeType));
            }
            foreach(var bindingPolicy in option.binding){
                _bindingPolicies.Add(new BindingPolicy(bindingPolicy.MessageType,
                                                        bindingPolicy.ExchangeTemplate,
                                                        bindingPolicy.RoutingKeyTemplate,
                                                        bindingPolicy.QueueTemplate,
                                                        bindingPolicy.ExchangeType));
            }
        }
        public void SetBindingPolicy(BindingPolicy policy)
        {
            BindingPolicy? _policy = _bindingPolicies.Where(bp => bp.MessageType == policy.MessageType).FirstOrDefault();
            if (_policy == null)
                _bindingPolicies.Add(policy);
            else
                _policy = policy;
        }

        public void SetRoutingPolicy(RoutingPolicy policy)
        {
            RoutingPolicy? _policy = _routingPolicies.Where(rp => rp.MessageType == policy.MessageType).FirstOrDefault();
            if (_policy == null)
                _routingPolicies.Add(policy);
            else
                _policy = policy;
        }

        public BindingPolicy GetBindingPolicy<TMessage>() where TMessage : IMessage
        {
            var messageType = typeof(TMessage).Name;
            return BindingPolicyOf(messageType);
        }
        public BindingPolicy GetBindingPolicy(Type type)
        {
            var messageType = type.Name;
            return BindingPolicyOf(messageType);
        }
        private BindingPolicy BindingPolicyOf(string messageType){
            BindingPolicy? _policy = _bindingPolicies.Where(bp => bp.MessageType == messageType).FirstOrDefault();
            if(_policy!=null)
                return _policy;
            else
            {
                _policy = _bindingPolicies.Where(bp => bp.MessageType == "default").FirstOrDefault();
                if(_policy!=null) return _policy;
            }
            return new BindingPolicy(messageType,messageType,"#","","topic");
        }

        public RoutingPolicy GetRoutingPolicy<TMessage>() where TMessage : IMessage
        {
            var messageType = typeof(TMessage).Name;
            return GetRoutingPolicyOf(messageType);
        }
        public RoutingPolicy GetRoutingPolicy(Type type)
        {
            var messageType = type.Name;
            return GetRoutingPolicyOf(messageType);
        }
        private RoutingPolicy GetRoutingPolicyOf(string messageType){
            RoutingPolicy? _policy = _routingPolicies.Where(rp => rp.MessageType == messageType).FirstOrDefault();
            if(_policy!=null)
                return _policy;
            else{
                _policy = _routingPolicies.Where(rp => rp.MessageType == "default").FirstOrDefault();
                if(_policy!=null) return _policy;
            }
            return new RoutingPolicy(messageType,messageType,"","","topic");
        }


        public IChannelAttributes ResolveBindingPolicy<TMessage>() where TMessage : IMessage
        {
            var policy = GetBindingPolicy<TMessage>();            
            return policy.Resolve<TMessage>();
        }

        public IChannelAttributes ResolveRoutingPolicy<TMessage>(TMessage message) where TMessage : IMessage
        {
            var policy = GetRoutingPolicy<TMessage>();
            return policy.Resolve<TMessage>(message);
        }
        public IChannelAttributes ResolveBindingPolicy(Type type)
        {
            var policy = GetBindingPolicy(type);
            return policy.Resolve(type);

        }
        public IChannelAttributes ResolveRoutingPolicy(Type type, IMessage message)
        {
            var policy = GetRoutingPolicy(type);
            return policy.Resolve(type,message);
        }
    }
}