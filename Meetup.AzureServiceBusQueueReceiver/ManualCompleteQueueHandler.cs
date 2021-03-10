using System;
using System.Text;
using System.Threading.Tasks;
using Ev.ServiceBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace Meetup.AzureServiceBusQueueReceiver
{
    public class ManualCompleteQueueHandler : IMessageHandler
    {
        private readonly ILogger<ManualCompleteQueueHandler> _logger;

        public ManualCompleteQueueHandler(ILogger<ManualCompleteQueueHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleMessageAsync(MessageContext context)
        {

            try
            {
                _logger.LogInformation("Receiving message");
                _logger.LogInformation("Message Id : {0}", context.Message.MessageId);

                var message = Encoding.UTF8.GetString(context.Message.Body);

                _logger.LogInformation("Message : {0}", message);
                throw new Exception("new Exception");

                await context.Receiver.CompleteAsync(context.Message.SystemProperties.LockToken);
            }
            catch (Exception e)
            {
                await context.Receiver.DeadLetterAsync(context.Message.SystemProperties.LockToken,
                    deadLetterReason: "Handler failed", deadLetterErrorDescription: e.Message);
            }
            
        }
    }
}