using FinalExamModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamModel.FinalExamModels.View_Model
{
    public class AuthorizeUser
    {
        public User User { get; set; }
        public int ProjectID { get; set; }
        public bool Authorize { get; set; }

    }
}
