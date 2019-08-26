using System.Collections.Concurrent;

namespace MessageProcessingSimulator
{
    public class MessageQueue : ConcurrentQueue<Message>, IMessageQueue
    {
        public Message DeQueue()
        {
            TryDequeue(out var message);
            return message;
        }

        public void EnQueue(Message message)
        {
            Enqueue(message);
        }

    }
}