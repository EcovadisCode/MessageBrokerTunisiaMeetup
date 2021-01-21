using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ev.ServiceBus;
using Ev.ServiceBus.Abstractions;
using Meetup.Consts;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Meetup.AzureServiceBusTopicSender
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

            var registry = serviceProvider.GetRequiredService<IServiceBusRegistry>();
            var topicSender = registry.GetTopicSender(MeetupConsts.MeetupTopicName);

            var message = new Message(Encoding.UTF8.GetBytes("here is a message"));
            await topicSender.SendAsync(message);

            await host.StopAsync(CancellationToken.None);
        }

        private static ServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            services.AddServiceBus(o => o.WithConnection(MeetupConsts.ServiceBusConnectionString));
            services.RegisterServiceBusTopic(MeetupConsts.MeetupTopicName);

            return services;
        }
    }
}