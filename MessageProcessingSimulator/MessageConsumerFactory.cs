using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

namespace MessageProcessingSimulator
{
    public class MessageConsumerFactory : IMessageConsumerFactory
    {
        private readonly Dictionary<string, ISingleTypeMessageConsumer> _consumers = new Dictionary<string, ISingleTypeMessageConsumer>();

        private readonly IServiceProvider _serviceProvider;

        public MessageConsumerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISingleTypeMessageConsumer GetForType(string messageType)
        {
            if (_consumers.ContainsKey(messageType))
            {
                return _consumers[messageType];
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var messageConsumer = scope.ServiceProvider.GetRequiredService<ISingleTypeMessageConsumer>();
                _consumers.Add(messageType, messageConsumer);

                return messageConsumer;
            }
        }
    }
}