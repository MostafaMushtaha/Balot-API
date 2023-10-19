using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Stack.ServiceLayer.Primitives.ConnectionFactories
{
    public class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
    {
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;

        public RabbitMQConnectionFactory(string hostName, string userName, string password)
        {
            _hostName = hostName;
            _userName = userName;
            _password = password;
        }

        public IConnection CreateConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };

            return factory.CreateConnection();
        }
    }
}