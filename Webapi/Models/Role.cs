﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webapi.Models
{
    public class Role
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int hierarchyID { get; set; }
    }
}