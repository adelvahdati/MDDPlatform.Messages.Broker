namespace MDDPlatform.Messages.Brokers.Configurations
{
    public interface IChannelAttributes
    {
        string Exchange { get; set;}
        string RoutingKey { get; set;}
        string Queue { get; set;}
        string ExchangeType { get;set;}
    }
}