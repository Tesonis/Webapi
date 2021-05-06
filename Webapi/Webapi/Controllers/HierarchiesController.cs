using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using TOLC.ERP.Application;
using Webapi.Models;

namespace Webapi.Controllers
{
    public class HierarchiesController : ApiController
    {
        private dbContext db = new dbContext();
        
        // GET: api/Hierarchies
        public JsonResult GetHierarchy(string sid)
        {
            List<Models.Hierarchy> hierarchylist = new List<Models.Hierarchy>();
            ReturnValue rv = new ReturnValue();
            DataSet DBSetHierarchy = null;
            TOLC.ERP.Application.Hierarchy hierarchies = new TOLC.ERP.Application.Hierarchy();
            rv = hierarchies.List(sid, ref DBSetHierarchy);
            if (DBSetHierarchy != null)
            {
                foreach (DataTable table in DBSetHierarchy.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var hierarchy = new Models.Hierarchy
                        {
                            Id = int.Parse(row["ROWID"].ToString()),
                            Name = row["NAME"].ToString(),
                        };
                        hierarchylist.Add(hierarchy);
                    }
                }
            }
            var result = new JsonResult();
            result.Data = JsonConvert.SerializeObject(hierarchylist);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
        // GET: api/Hierarchies
        public JsonResult GetHierarchyTeamType(string sid, string hid)
        {
            List<Models.Teamtype> teamtypelist = new List<Models.Teamtype>();
            ReturnValue rv = new ReturnValue();
            DataSet DBSetTeamType = null;
            TOLC.ERP.Application.Hierarchy teamtypes = new TOLC.ERP.Application.Hierarchy();
            rv = teamtypes.ListTeamType(sid, ref DBSetTeamType);
            int.TryParse(hid, out int hid_int);
            if (DBSetTeamType != null)
            {
                foreach (DataTable table in DBSetTeamType.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (int.Parse(row["Hierarchy_ID"].ToString()) == hid_int)
                        {
                            var teamtype = new Models.Teamtype
                            {
                                Id = int.Parse(row["ROWID"].ToString()),
                                Name = row["NAME"].ToString(),
                                HierarchyID = int.Parse(row["Hierarchy_ID"].ToString())
                            };
                            teamtypelist.Add(teamtype);
                        }
                        
                    }
                }
            }
            var result = new JsonResult();
            result.Data = JsonConvert.SerializeObject(teamtypelist);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
        // GET: api/Hierarchies/5
        public async Task<IHttpActionResult> GetHierarchy(int id)
        {

            return null;
        }
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HierarchyExists(int id)
        {
            return db.Hierarchies.Count(e => e.Id == id) > 0;
        }
    }
}