using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessageProcessingSimulator
{
    public class SingleTypeMessageConsumer : ISingleTypeMessageConsumer
    {
        private readonly AppOption _appOptions;

        private readonly ILogger<MessageGenerator> _logger;

        private readonly IFileLogger _fileLogger;

        private readonly object _lockObject = new object();

        public SingleTypeMessageConsumer(IOptions<AppOption> appOptions, ILogger<MessageGenerator> logger, IFileLogger fileLogger)
        {
            _appOptions = appOptions.Value;
            _logger = logger;
            _fileLogger = fileLogger;
        }

        public Task ProcessAsync(Message message)
        {
            lock (_lockObject)
            {
                _fileLogger.WriteLogAsync(message).GetAwaiter().GetResult();
                Task.Delay(_appOptions.MessageConsumerIntervalInMilliSecs);
                return Task.CompletedTask;
            }
        }
    }
}