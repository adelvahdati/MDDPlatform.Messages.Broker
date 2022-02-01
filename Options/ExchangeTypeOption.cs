namespace MDDPlatform.Messages.Broker.Options
{
    public class ExchangeTypeOption
    {
        public string Value { get; protected set; } = string.Empty;
        private ExchangeTypeOption(string value)
        {
            Value = value;
        }
        public static ExchangeTypeOption Direct()
        {
            return new ExchangeTypeOption("direct");
        }
        public static ExchangeTypeOption Fanout()
        {
            return new ExchangeTypeOption("fanout");
        }
        public static ExchangeTypeOption Topic()
        {
            return new ExchangeTypeOption("topic");
        }
        public static ExchangeTypeOption Headers()
        {
            return new ExchangeTypeOption("headers");
        }

    }
}