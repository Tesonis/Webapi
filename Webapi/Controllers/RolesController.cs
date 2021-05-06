using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using TOLC.ERP.Application;
using Webapi.Models;

namespace Webapi.Controllers
{
    public class RolesController : ApiController
    {
        private dbContext db = new dbContext();

        // GET: api/Roles
        public JsonResult GetRoles(string sid)
        {
            List<Models.Role> rolelist = new List<Models.Role>();
            ReturnValue rv = new ReturnValue();
            DataSet DBSetRoles = null;
            rv = new TOLC.ERP.Application.Role().List(sid, ref DBSetRoles);
            if (DBSetRoles != null)
            {
                foreach (DataTable table in DBSetRoles.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var role = new Models.Role
                        {
                            ID = int.Parse(row["ROWID"].ToString()),
                            Description = row["LONG_NAME"].ToString(),
                            hierarchyID = int.Parse(row["HIERARCHY_ID"].ToString())
                        };
                        rolelist.Add(role);
                    }
                }
            }
            var result = new JsonResult();
            result.Data = JsonConvert.SerializeObject(rolelist);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
        
    }
}