using System.Text;
using System.Threading.Tasks;
using Ev.ServiceBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace Meetup.AzureServiceBusTopicReceiver
{
    public class FirstSubscriptionHandler : IMessageHandler
    {
        private readonly ILogger<FirstSubscriptionHandler> _logger;

        public FirstSubscriptionHandler(ILogger<FirstSubscriptionHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleMessageAsync(MessageContext context)
        {
            _logger.LogInformation("First Handler: Receiving message");
            _logger.LogInformation("First Handler: Message Id : {0}", context.Message.MessageId);

            var message = Encoding.UTF8.GetString(context.Message.Body);

            _logger.LogInformation("First Handler: Message : {0}", message);

            return Task.CompletedTask;
        }
    }
}