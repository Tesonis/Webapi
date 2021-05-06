using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webapi.Models
{
    public class Team
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string HierarchyId { get; set; }
        public string ParentId { get; set; }
        public string SystemRefCode { get; set; }
    }
}