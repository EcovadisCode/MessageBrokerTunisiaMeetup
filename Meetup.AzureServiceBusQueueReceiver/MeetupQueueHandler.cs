using System.Text;
using System.Threading.Tasks;
using Ev.ServiceBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace Meetup.AzureServiceBusQueueReceiver
{
    public class MeetupQueueHandler : IMessageHandler
    {
        private readonly ILogger<MeetupQueueHandler> _logger;

        public MeetupQueueHandler(ILogger<MeetupQueueHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleMessageAsync(MessageContext context)
        {
            _logger.LogInformation("Receiving message");
            _logger.LogInformation("Message Id : {0}", context.Message.MessageId);

            var message = Encoding.UTF8.GetString(context.Message.Body);

            // throw new Exception("Something went wrong");

            _logger.LogInformation("Message : {0}", message);

            return Task.CompletedTask;
        }
    }
}