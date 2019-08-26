using System.Threading.Tasks;

namespace MessageProcessingSimulator
{
    public interface IFileLogger
    {
        Task WriteLogAsync(Message message);
    }
}