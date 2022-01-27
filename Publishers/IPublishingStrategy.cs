namespace MDDPlatform.Messages.Broker.Publishers
{
    public interface IPublishingStrategy
    {        
        string Exchange {get;}
        string RoutingKey {get;}
        string Queue {get;}
        string ExchangeType {get;}
    }

}