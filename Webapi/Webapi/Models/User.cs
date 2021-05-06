using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webapi.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}