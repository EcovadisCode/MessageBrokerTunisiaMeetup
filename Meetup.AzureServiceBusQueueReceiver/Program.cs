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

namespace Meetup.AzureServiceBusQueueReceiver
{
    internal class Program
    {
        private static readonly ILogger Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

        private static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            var hostedServices = serviceProvider.GetServices<IHostedService>();
            var host = hostedServices.OfType<ServiceBusHost>().First();

            await host.StartAsync(CancellationToken.None);

            await Task.Delay((int) TimeSpan.FromMinutes(1).TotalMilliseconds);

            await host.StopAsync(CancellationToken.None);
        }

        private static ServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Logger, true));
            services.AddScoped<MeetupQueueHandler>();
            services
                .AddServiceBus()
                .ConfigureServiceBus(options =>
                {
                    options.RegisterQueue(MeetupConsts.MeetupQueueName)
                        .WithConnectionString(MeetupConsts.ServiceBusConnectionString)
                        .WithCustomMessageHandler<MeetupQueueHandler>();
                });

            return services;
        }
    }
}