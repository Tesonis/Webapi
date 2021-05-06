using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webapi.Models
{
    public class Teamtype
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HierarchyID { get; set; }
    }
}