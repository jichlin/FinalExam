using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Base
{
    public class StoredProcedure
    {
        [Key]
        public string SPName { get; set; }
        public SqlParameter[] SQLParam { get; set; }

        public string SQLParamString { get; set; }
    }
}
