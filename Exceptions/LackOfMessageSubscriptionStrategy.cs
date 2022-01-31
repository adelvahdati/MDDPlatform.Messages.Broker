namespace MDDPlatform.Messages.Broker.Exceptions{
    public class LackOfMessageSubscriptionStrategy : Exception
    {
        public LackOfMessageSubscriptionStrategy(string? message) : base(message)
        {
            
        }
    }
}