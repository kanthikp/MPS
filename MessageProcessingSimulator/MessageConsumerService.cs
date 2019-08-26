using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessageProcessingSimulator
{
    public class MessageConsumerService : IHostedService
    {
        private readonly ILogger<MessageConsumerService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly AppOption _appOptions;

        private readonly IMessageConsumerFactory _messageConsumerFactory;

        public MessageConsumerService(ILogger<MessageConsumerService> logger, IOptions<AppOption> appOptions, IServiceProvider serviceProvider, IMessageConsumerFactory messageConsumerFactory)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _messageConsumerFactory = messageConsumerFactory;
            _appOptions = appOptions.Value;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {

            _logger.LogInformation("Starting Task");
            _logger.LogInformation("Read the queue ans based on the message type create consumer");
            _logger.LogInformation($"Number of Message Types: {_appOptions.MessageTypesCount}");

            if (_appOptions.MessageTypesCount < 1 || _appOptions.MessageTypesCount > 26)
            {
                throw new IndexOutOfRangeException("MessageTypesCount must be between 1 and 26");
            }

            //Task task = null ;
            using (var scope = _serviceProvider.CreateScope())
            {
                var messageQueue = scope.ServiceProvider.GetRequiredService<IMessageQueue>();
                while (!cancellationToken.IsCancellationRequested)
                {
                    var message = messageQueue.DeQueue();
                    if (message != null)
                    {
                        _logger.LogInformation($"Queuing Message: {message.SequenceNumber} created: {message.Created} of Type: {message.Type}");

                        var messageConsumer = _messageConsumerFactory.GetForType(message.Type);

                        await messageConsumer.ProcessAsync(message);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Task");
            return Task.CompletedTask;
        }
    }
}