namespace MessageProcessingSimulator
{
    public interface IMessageQueue
    {
        void EnQueue(Message message);

        Message DeQueue();
    }
}