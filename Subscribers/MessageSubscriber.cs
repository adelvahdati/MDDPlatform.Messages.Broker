using System.Text;
using MDDPlatform.Messages.Commands;
using MDDPlatform.Messages.Core;
using MDDPlatform.Messages.Events;
using MDDPlatform.Messages.Dispatchers;
using MDDPlatform.Messages.Wrapers;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MDDPlatform.Messages.Broker.Subscribers;
using MDDPlatform.Messages.Brokers.Configurations;

namespace MDDPlatform.Messages.Brokers.Subscribers
{
    public class MessageSubscriber : IMessageSubscriber
    {
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly IBusConfiguration _configuration;
        private readonly ICollection<string> _exchanges;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageSubscriber(IMessageDispatcher messageDispatcher, IBusConfiguration configuration)
        {
            _exchanges = new List<string>();
            _messageDispatcher = messageDispatcher;
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration.HostName,
                Port = _configuration.Port
            };
            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += OnConnectionShutdown;

            _channel = _connection.CreateModel();
            Console.WriteLine("---> Message Subscriber Connected to RabbitMQ");
        }

        private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--->Subscriber : Connection Shutdown ...");
        }

        public void Subscribe(string exchange, string exchangeType, string routingKey, Func<string, Task> handler)
        {
            if (_connection.IsOpen)
            {
                if (!_exchanges.Contains(exchange))
                {
                    _channel.ExchangeDeclare(exchange, exchangeType);
                    _exchanges.Add(exchange);
                }
                var _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(_queueName, exchange, routingKey);

                var consumer = new EventingBasicConsumer(_channel);

                Console.WriteLine($"Subscriber Listen to the Channel : Exchange : {exchange} , routingKey : {routingKey}");

                consumer.Received += async (model, ea) =>
                {

                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    await handler(message);
                };
                _channel.BasicConsume(_queueName, autoAck: true, consumer);
            }
        }

        public Task SubscribeAsync<T>() where T : IMessage
        {
            IChannelAttributes channelAttribute = _configuration.ResolveBindingPolicy<T>();

            if (_connection.IsOpen)
            {
                if (!_exchanges.Contains(channelAttribute.Exchange))
                {
                    _channel.ExchangeDeclare(channelAttribute.Exchange, channelAttribute.ExchangeType);
                    _exchanges.Add(channelAttribute.Exchange);
                }
                var _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(_queueName, channelAttribute.Exchange, channelAttribute.RoutingKey);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine("---> Message Received By Subscriber : " + message);
                    try
                    {
                        var wrrapedMessage = JsonConvert.DeserializeObject<WrappedMessage<T>>(message);
                        if (wrrapedMessage != null)
                        {
                            T orginalMessage = wrrapedMessage.Body;
                            if (typeof(IEvent).IsAssignableFrom(typeof(T)))
                            {
                                if (_messageDispatcher == null) Console.WriteLine("---> Message Dispatcher is null");
                                if (orginalMessage == null) Console.WriteLine("Orginal Message is null");

                                if (_messageDispatcher != null && orginalMessage != null)
                                    await _messageDispatcher.HandleAsync((IEvent)orginalMessage);
                            }
                            if (typeof(ICommand).IsAssignableFrom(typeof(T)))
                            {
                                if (_messageDispatcher != null && orginalMessage != null)
                                    await _messageDispatcher.HandleAsync((ICommand)orginalMessage);
                            }
                        }
                        else
                        {
                            Console.WriteLine("---> Deserialized Message is null");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                };
                _channel.BasicConsume(_queueName, autoAck: true, consumer);

            }
            return Task.CompletedTask;
        }
    }
}