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
    public class MessagePublisherService : IHostedService
    {
        private readonly ILogger<MessagePublisherService> _logger;

        private readonly IServiceProvider _serviceProvider;

        private readonly AppOption _appOptions;

        public MessagePublisherService(ILogger<MessagePublisherService> logger, IOptions<AppOption> appOptions, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _appOptions = appOptions.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Task");
            _logger.LogInformation($"Number of Message Types: {_appOptions.MessageTypesCount}");

            if (_appOptions.MessageTypesCount < 1 || _appOptions.MessageTypesCount > 26)
            {
                throw new IndexOutOfRangeException("MessageTypesCount must be between 1 and 26");
            }

            var tasks = Enumerable.Range(1, _appOptions.MessageTypesCount)
                .Select(i => (MessageType)Enum.Parse(typeof(MessageType), i.ToString()))
                //.Select(mt => Task.Factory.StartNew(action: async () =>
                .Select(mt => new Task(action: async () =>
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var messageGenerator = scope.ServiceProvider.GetRequiredService<IMessageGenerator>();
                        await messageGenerator.StartAsync(mt, cancellationToken);
                    }
                }, cancellationToken))
                .ToList();

            tasks.ForEach(t => t.Start());
            Task.WaitAll(tasks.ToArray(), cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Task");
            return Task.CompletedTask;
        }
    }
}