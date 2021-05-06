using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webapi.Models
{
    public class Auth
    {
        [Key]
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}