using System.Threading.Tasks;
using Ev.ServiceBus.Abstractions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;

namespace Meetup.AzureServiceBusQueueReceiver
{
    internal class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleExceptionAsync(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            // TODO add your exception handling behaviour here
            _logger.LogError("Something happened during message handling", exceptionReceivedEventArgs.Exception);
            return Task.CompletedTask;
        }
    }
}