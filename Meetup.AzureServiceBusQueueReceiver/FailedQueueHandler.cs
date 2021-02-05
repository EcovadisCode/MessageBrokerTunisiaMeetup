using System;
using System.Text;
using System.Threading.Tasks;
using Ev.ServiceBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace Meetup.AzureServiceBusQueueReceiver
{
    public class FailedQueueHandler : IMessageHandler
    {
        private readonly ILogger<FailedQueueHandler> _logger;

        public FailedQueueHandler(ILogger<FailedQueueHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleMessageAsync(MessageContext context)
        {
            _logger.LogInformation("Receiving message");
            _logger.LogInformation("Message Id : {0}", context.Message.MessageId);
            throw new Exception("Something went wrong");
        }
    }
}