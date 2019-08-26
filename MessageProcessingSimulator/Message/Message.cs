using System;

namespace MessageProcessingSimulator
{
    public class Message
    {
        public static Message CreateOfType(string type, int sequenceNumber)
        {
            return new Message
            {
                Type = type,
                SequenceNumber = sequenceNumber
            };
        }

        public DateTime Created { get; } = DateTime.Now;

        public int SequenceNumber { get; private set; }

        public string Type { get; private set; }

        public override string ToString()
        {
            return $"{SequenceNumber}|{Type}|{Created.ToString("ddMMyyyy hh:mm:ss.fff")}";
        }
    }
}