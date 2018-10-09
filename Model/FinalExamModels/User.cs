using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamModels
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public int Roles { get; set; }
        public bool isActive { get; set; }
        public string Picture { get; set; }
    }
}
