namespace MDDPlatform.Messages.Broker.Exceptions{
    public class LackOfMessagePublishingStrategy : Exception
    {
        public LackOfMessagePublishingStrategy(string? message) : base(message)
        {
            
        }
    }
}