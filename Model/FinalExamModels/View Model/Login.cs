using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalExamModels;
using System.ComponentModel.DataAnnotations;

namespace FinalExamModel.FinalExamModels.View_Model
{
    public class Login
    {
        public int UserID { get; set; }
        [Required(ErrorMessage = "Wajib diisi")]
        public string Username { get; set; }
        [Required(ErrorMessage ="Wajib diisi")]
        public string Password { get; set; }
    }
}
