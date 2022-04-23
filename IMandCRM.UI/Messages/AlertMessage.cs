using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Messages
{
    public class AlertMessage
    {
        public bool ResponseStatus { get; set; }
        public string MessageType { get; set; }
        public string MessageText { get; set; }
    }
}
