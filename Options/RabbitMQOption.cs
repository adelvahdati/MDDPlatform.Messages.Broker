using MDDPlatform.Messages.Broker.Publishers;
using MDDPlatform.Messages.Broker.Subscribers;

namespace MDDPlatform.Messages.Broker.Options{
    public class  RabbitMQOption
    {
        public string HostName { get; set; }
        public int Port { get; set; }

        public List<BindingPolicy> binding {get;set;}

        public List<RoutingPolicy> routing {get;set;}

        public RabbitMQOption()
        {
            
        }
    }

}