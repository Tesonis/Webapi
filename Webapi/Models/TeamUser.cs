using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Webapi.Models
{
    public class TeamUser
    {
        [Key, Column(Order = 0)]
        public int TeamID { get; set; }
        [Key, Column(Order = 1)]
        public string Username { get; set; }
        public int RoleID { get; set; }
        
    }
}