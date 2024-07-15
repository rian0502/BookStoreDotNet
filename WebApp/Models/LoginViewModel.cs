using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email Harus di isi")]
        public string email { get; set; }
        public string password { get; set; }
    }
}