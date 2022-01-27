using MDDPlatform.Messages.Core;

namespace MDDPlatform.Messages.Wraper
{
    public class WrappedMessage<T> where T : IMessage
    {
        public T Body { get;  set; }
        public string MessageId { get; set; }

        public string CorrelationId { get; set; }
        public string MessageType { get; set; }

        public string MessageContext { get; set; }
        public IDictionary<string, object> Headers { get; set; }
        public WrappedMessage()
        {
                        
        }
        public WrappedMessage(T body, string? messageId = null, string? correlationId = null, string? messageContext = null, IDictionary<string, object>? headers = null)
        {
            Body = body;
            MessageType = body.GetType().ToString();
            MessageId = string.IsNullOrEmpty(messageId) ? string.Empty : messageId;
            CorrelationId = string.IsNullOrEmpty(correlationId) ? string.Empty : correlationId; ;
            MessageContext = string.IsNullOrEmpty(messageContext) ? string.Empty : messageContext;
            Headers = (headers == null) ? new Dictionary<string, object>() : headers;
        }
    }
}