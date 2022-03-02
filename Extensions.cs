using MDDPlatform.Messages.Brokers.Configurations;
using MDDPlatform.Messages.Brokers.Options;
using MDDPlatform.Messages.Dispatchers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDDPlatform.Messages.Brokers
{
    public static class Extensions 
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services,IConfiguration configuration, string sectionName)
        {
            services.AddSingleton<IMessageDispatcher,MessageDispatcher>();
            RabbitMQOption _option = new RabbitMQOption();
            configuration.GetSection(sectionName).Bind(_option);
            services.AddSingleton<RabbitMQOption>(_option);
            services.AddSingleton<IBusConfiguration>(s=>{
                BusConfiguration busConfiguration = new BusConfiguration(_option);
                return busConfiguration;
            });
            services.AddSingleton<IMessageBroker,MessageBroker>();
            return services;
        }
    }
}