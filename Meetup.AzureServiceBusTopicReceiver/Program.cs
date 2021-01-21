using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ev.ServiceBus;
using Ev.ServiceBus.Abstractions;
using Meetup.Consts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Meetup.AzureServiceBusTopicReceiver
{
    internal class Program
    {
        private static readonly ILogger Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

        private static async Task Main(string[] args)
        {
            var serviceCollection = ConfigureServices();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var hostedServices = serviceProvider.GetServices<IHostedService>();
            var host = hostedServices.OfType<ServiceBusHost>().First();
            await host.StartAsync(CancellationToken.None);

            await Task.Delay((int) TimeSpan.FromMinutes(2).TotalMilliseconds);

            await host.StopAsync(CancellationToken.None);
        }

        private static ServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(Logger, true));

            services
                .AddScoped<FirstSubscriptionHandler>()
                .AddScoped<SecondSubscriptionHandler>();
            
            services.AddServiceBus(o => o.WithConnection(MeetupConsts.ServiceBusConnectionString));
                
            services.RegisterServiceBusSubscription(MeetupConsts.MeetupTopicName, MeetupConsts.MeetupTopicFirstSubscriptionName)
                .WithCustomMessageHandler<FirstSubscriptionHandler>();

            services.RegisterServiceBusSubscription(MeetupConsts.MeetupTopicName, MeetupConsts.MeetupTopicSecondSubscriptionName)
                .WithCustomMessageHandler<SecondSubscriptionHandler>();
            
            return services;
        }
    }
}