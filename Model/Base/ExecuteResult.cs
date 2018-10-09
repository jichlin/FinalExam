using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Base
{
    public class ExecuteResult
    {
        [Key]
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public bool? Status { get; set; }
        [NotMapped]
        public dynamic ReturnVariable { get; set; }
        
    }
}
