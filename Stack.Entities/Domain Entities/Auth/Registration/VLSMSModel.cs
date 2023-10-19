using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.Entities.DomainEntities
{
    public class VLSMSModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SMSText { get; set; }
        public string SMSLang { get; set; }
        public string SMSSender { get; set; }
        public string SMSReceiver { get; set; }

        public string SMSID { get; set; }

        //public string Validity { get; set; }
    }
}
