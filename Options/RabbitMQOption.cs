using MDDPlatform.Messages.Brokers.Publishers;
using MDDPlatform.Messages.Brokers.Subscribers;

namespace MDDPlatform.Messages.Brokers.Options
{
    public class  RabbitMQOption
    {
        public string HostName { get; set; }
        public int Port { get; set; }

        public List<BindingPolicy> binding {get;set;}

        public List<RoutingPolicy> routing {get;set;}

        public RabbitMQOption()
        {
            binding = new List<BindingPolicy>();
            routing = new List<RoutingPolicy>(); 
        }
    }

}