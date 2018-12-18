using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamModels
{
    public class Sprint
    {
        public int ProjectID { get; set; }
        public int SprintID { get; set; }
        public string SprintName { get; set; }
        public DateTime SprintStartDate { get; set; }
        public DateTime SprintEndDate { get; set; }
    }
}
