using System.Threading;
using System.Threading.Tasks;

namespace MessageProcessingSimulator
{
    public interface IMessageGenerator
    {
        Task StartAsync(MessageType messageType, CancellationToken cancellationToken);
    }
}