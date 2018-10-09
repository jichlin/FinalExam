using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Base
{
    public class AlertMessage
    {
        public AlertMessage(string type, string message) 
        {
            this.type = type;
            this.message = message;
        }
        public AlertMessage(string type, string message, string form)
        {
            this.type = type;
            this.message = message;
            this.form = form;
        }
        public string type { get; set; }
        public string message { get; set; }
        public string form { get; set; }
    }
}
