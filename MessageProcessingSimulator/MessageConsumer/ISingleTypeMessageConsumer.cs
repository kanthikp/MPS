using System.Threading;
using System.Threading.Tasks;

namespace MessageProcessingSimulator
{
    public interface ISingleTypeMessageConsumer
    {
        Task ProcessAsync(Message message);
    }
}