using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamModels
{
    public class WorkItem
    {
        public int SprintID { get; set; }
        public int WorkItemID { get; set; }
        public string WorkItemName { get; set; }
        public int StateID { get; set; }
        [NotMapped]
        public IEnumerable<WorkTask> WorkTasks { get; set; }
    }
}
