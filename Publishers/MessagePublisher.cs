using System.Text;
using System.Text.Json;
using MDDPlatform.Messages.BrokerConfiguration;
using MDDPlatform.Messages.Core;
using MDDPlatform.Messages.Wraper;
using RabbitMQ.Client;

namespace MDDPlatform.Messages.Broker.Publishers
{
    public class MessagePublisher : IMessagePublisher
    {
        private ICollection<string> _exchanges;

        private readonly IBusConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessagePublisher(IBusConfiguration configuration)
        {
            _exchanges = new List<string>();
            _configuration = configuration;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration.HostName,
                Port = _configuration.Port
            };
            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += OnConnectionShutdown;

            _channel = _connection.CreateModel();
            Console.WriteLine("---> Message Publisher Connected to RabbitMQ");
        }

        public Task PubblishAsync<T>(WrappedMessage<T> message,IPublishingStrategy strategy) where T : IMessage
        {            
            var exchange = strategy.Exchange;
            var exchangeType = strategy.ExchangeType;
            var routingKey = strategy.RoutingKey;
            var txtMessage = JsonSerializer.Serialize<WrappedMessage<T>>(message);
            Publish(txtMessage, exchange, routingKey, exchangeType);
            return Task.CompletedTask;
        }

        public void Publish(string message, string exchange, string routingKey, string exchangeType)
        {
            if (_connection.IsOpen)
            {
                if (!_exchanges.Contains(exchange))
                {
                    _channel.ExchangeDeclare(exchange, type: exchangeType);
                    _exchanges.Add(exchange);
                }

                var body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish(exchange: exchange,
                                        routingKey: routingKey,
                                        basicProperties: null,
                                        body: body);

                Console.WriteLine("---> Message Published : " + message);
            }
        }

        private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("---> Connection Shutdown ...");
        }
    }
}