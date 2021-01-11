using System.Text;
using System.Threading.Tasks;
using Ev.ServiceBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace Meetup.AzureServiceBusTopicReceiver
{
    public class SecondSubscriptionHandler : IMessageHandler
    {
        private readonly ILogger<SecondSubscriptionHandler> _logger;

        public SecondSubscriptionHandler(ILogger<SecondSubscriptionHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleMessageAsync(MessageContext context)
        {
            _logger.LogInformation("Second Handler: Receiving message");
            _logger.LogInformation("Second Handler: Message Id : {0}", context.Message.MessageId);

            var message = Encoding.UTF8.GetString(context.Message.Body);

            _logger.LogInformation("Second Handler: Message : {0}", message);
            return Task.CompletedTask;
        }
    }
}