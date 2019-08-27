using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MessageProcessingSimulator
{
    public class FileLogger : IFileLogger
    {
        private readonly AppOption _appOptions;
        public FileLogger(IOptions<AppOption> appOptions)
        {
            _appOptions = appOptions.Value;
        }

        public Task WriteLogAsync(Message message)
        {
            try
            {
                if(!Directory.Exists(_appOptions.LogFileDirectory))
                    Directory.CreateDirectory(_appOptions.LogFileDirectory);
                var logFilename = $"{_appOptions.LogFileDirectory}//{ message.Type}.log";
                using (StreamWriter writer = new StreamWriter(logFilename, true, System.Text.Encoding.UTF8))
                {
                    return writer.WriteLineAsync(message.ToString());
                }
            }
            catch
            {
                throw;
            }
        }

    }
}