namespace MessageProcessingSimulator
{
    public interface IMessageConsumerFactory
    {
        ISingleTypeMessageConsumer GetForType(string messageType);
    }
}