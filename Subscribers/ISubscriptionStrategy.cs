namespace MDDPlatform.Messages.Broker.Subscribers 
{
    public interface ISubscriptionStrategy {
        string Exchange {get;}
        string RoutingKey {get;}
        string Queue {get;}
        string ExchangeType {get;}

    }
}