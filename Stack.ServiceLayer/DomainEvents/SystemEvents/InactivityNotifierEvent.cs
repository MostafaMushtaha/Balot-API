using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Stack.ServiceLayer.DomainEvents
{
    public class InactivityNotifierEvent : INotification
    {
        public InactivityNotifierEvent(string UserID, string Message)
        {
            this.UserID = UserID;
            this.Message = Message;
        }

        public string UserID { get; }
        public string Message { get; set; }
    }
}