using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamModels
{
    public class WorkTask
    {
        public int TaskID { get; set; }
        public int WorkItemID { get; set; }
        public string TaskName { get; set; }
        public int StateID { get; set; }
        public string Username { get; set; }
        public int UserID { get; set; }
    }
}
