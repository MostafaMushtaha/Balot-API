using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Stack.ServiceLayer.DomainEvents
{
    public class UserRegisteredEvent : INotification
    {
        public UserRegisteredEvent(string ID, string fullName)
        {
            this.ID = ID;
            this.fullName = fullName;
        }

        public string ID { get; }
        public string fullName { get; }
    }
}