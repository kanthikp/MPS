using System.Threading.Tasks;

namespace MessageProcessingSimulator
{
    public class FileLogger : IFileLogger
    {

      
        public Task WriteLogAsync(Message message)
        {
            

            try
            {
                if(!System.IO.Directory.Exists(@"../../../logfiles"))
                    System.IO.Directory.CreateDirectory(@"../../../logfiles");
                var logFilename = @"../../../LogFiles/" + message.Type + ".log";
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(logFilename, true, System.Text.Encoding.UTF8))
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