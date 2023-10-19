using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Stack.ServiceLayer.Primitives.ConnectionFactories
{
    public interface IRabbitMQConnectionFactory
    {
        public IConnection CreateConnection();
    }
}