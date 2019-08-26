using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessageProcessingSimulator
{
    public class MessageGenerator : IMessageGenerator
    {
        private readonly AppOption _appOptions;

        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<MessageGenerator> _logger;

        public MessageGenerator(IOptions<AppOption> appOptions, IServiceProvider serviceProvider, ILogger<MessageGenerator> logger)
        {
            _appOptions = appOptions.Value;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(MessageType messageType, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting generator for Message Type: {messageType.ToString()}");

            var sequenceNumber = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = Message.CreateOfType(messageType.ToString(), sequenceNumber);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var messageQueue = scope.ServiceProvider.GetRequiredService<IMessageQueue>();
                    _logger.LogInformation($"Queuing Message: {message.SequenceNumber} created: {message.Created} of Type: {message.Type}");
                    messageQueue.EnQueue(message);
                }

                await Task.Delay(_appOptions.MessageGeneratorIntervalInMilliSecs, cancellationToken);

                sequenceNumber++;
            }
        }
    }
}