using System.Text;
using MDDPlatform.Messages.Brokers.Configurations;
using MDDPlatform.Messages.Commands;
using MDDPlatform.Messages.Core;
using MDDPlatform.Messages.Dispatchers;
using MDDPlatform.Messages.Events;
using MDDPlatform.Messages.Wrapers;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MDDPlatform.Messages.Brokers
{
    public class MessageBroker : IMessageBroker
    {
        private ICollection<string> _exchanges;
        private readonly IBusConfiguration _configuration;
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        

        public MessageBroker(IBusConfiguration configuration,IMessageDispatcher messageDispatcher)
        {
            _exchanges = new List<string>();
            _configuration = configuration;
            _messageDispatcher = messageDispatcher;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration.HostName,
                Port = _configuration.Port
            };
            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += OnConnectionShutdown;

            _channel = _connection.CreateModel();
        }
        private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
        }
        public async Task PublishAsync(IEvent @event)
        {
            Type type = @event.GetType();
            IChannelAttributes channelAttributes = _configuration.ResolveRoutingPolicy(type,@event);
            var txtMessage = JsonConvert.SerializeObject(new WrappedMessage<IEvent>(@event));            
            await Publish(txtMessage,channelAttributes);                       
        }

        public async Task PublishAsync(IEnumerable<IEvent> events)
        {
            foreach(var @event in events){
                await PublishAsync(@event);
            }
        }

        public async Task PublishAsync<TEvent>(IWrappedMessage<TEvent> wrappedEvent) where TEvent : IEvent
        {
            IChannelAttributes channelAttributes = _configuration.ResolveRoutingPolicy<TEvent>(wrappedEvent.Body);
            var txtMessage = JsonConvert.SerializeObject(wrappedEvent);
            await Publish(txtMessage,channelAttributes);
        }

        public async Task SendAsync(ICommand command)
        {
            Type type = command.GetType();
            IChannelAttributes channelAttributes = _configuration.ResolveRoutingPolicy(type,command);
            var txtMessage = JsonConvert.SerializeObject(new WrappedMessage<ICommand>(command));
            await Publish(txtMessage,channelAttributes);                       
        }

        public async Task SendAsync(IEnumerable<ICommand> commands)
        {
            foreach(var command in commands)
            {
                await SendAsync(command);
            }
        }

        public async Task SendAsync<TCommand>(IWrappedMessage<TCommand> wrappedCommand) where TCommand : ICommand
        {
            IChannelAttributes channelAttributes = _configuration.ResolveRoutingPolicy<TCommand>(wrappedCommand.Body);
            var txtMessage = JsonConvert.SerializeObject(wrappedCommand);
            await Publish(txtMessage,channelAttributes);
        }

        public Task SubscribeAsync<TMessage>() where TMessage : IMessage
        {
            IChannelAttributes channelAttribute = _configuration.ResolveBindingPolicy<TMessage>();

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
                    try
                    {
                        var wrrapedMessage = JsonConvert.DeserializeObject<WrappedMessage<TMessage>>(message);
                        if (wrrapedMessage != null)
                        {
                            TMessage orginalMessage = wrrapedMessage.Body;
                            if (typeof(IEvent).IsAssignableFrom(typeof(TMessage)))
                            {

                                if (_messageDispatcher != null && orginalMessage != null)
                                {
                                    await _messageDispatcher.HandleAsync((IEvent)orginalMessage);
                                }
                                    
                            }
                            if (typeof(ICommand).IsAssignableFrom(typeof(TMessage)))
                            {
                                if (_messageDispatcher != null && orginalMessage != null)
                                    await _messageDispatcher.HandleAsync((ICommand)orginalMessage);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Message Broker Exception -> Error to Consume message");
                        Console.WriteLine(ex.Message);
                    }
                };
                _channel.BasicConsume(_queueName, autoAck: true, consumer);

            }
            return Task.CompletedTask;
        }
        private Task Publish(string message, IChannelAttributes channelAttributes)
        {
            var exchange = channelAttributes.Exchange;
            var exchangeType = channelAttributes.ExchangeType;
            var routingKey = channelAttributes.RoutingKey;
            
            Publish(message, exchange, routingKey, exchangeType);
            return Task.CompletedTask;
        }
        private Task Publish(string message, string exchange, string routingKey, string exchangeType)
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

            }
            return Task.CompletedTask;
        }
    }
}