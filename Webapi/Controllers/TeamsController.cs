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
    public class TeamsController : ApiController
    {
        private dbContext db = new dbContext();

        // GET: api/Teams
        public JsonResult GetTeams(string sid, string hid)
        {
            int.TryParse(hid, out int hid_int);
            List<Models.Team> teamlist = new List<Models.Team>();
            List<int> teamtypeidlist = new List<int>();
            ReturnValue rv = new ReturnValue();
            DataSet DBSetTeams = null;
            DataSet DBSetTeamType = null;
            rv = new TOLC.ERP.Application.Hierarchy().ListTeamType(sid, ref DBSetTeamType);

            if (DBSetTeamType != null)
            {
                foreach (DataTable table in DBSetTeamType.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (int.Parse(row["Hierarchy_ID"].ToString()) == hid_int)
                        {
                            teamtypeidlist.Add(int.Parse(row["ROWID"].ToString()));
                        }

                    }
                }
            }

            TOLC.ERP.Application.Team teams = new TOLC.ERP.Application.Team();
            int.TryParse(hid, out int hierarchy);
            rv = teams.List(sid, ref DBSetTeams);

            if (DBSetTeams != null)
            {
                foreach (DataTable table in DBSetTeams.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (teamtypeidlist.Contains(int.Parse(row["TEAM_TYPE_ID"].ToString())))
                        {
                            var team = new Models.Team
                            {
                                Id = int.Parse(row["ROWID"].ToString()),
                                Name = row["NAME"].ToString(),
                                HierarchyId = row["TEAM_TYPE_ID"].ToString(),
                                ParentId = row["PARENT_TEAM_ID"].ToString(),
                                SystemRefCode = row["LEGACY_SYSTEM_REFERENCE"].ToString(),
                            };
                            teamlist.Add(team);
                        }
                    }
                }
            }
            var result = new JsonResult();
            result.Data = JsonConvert.SerializeObject(teamlist);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        // GET: api/Teams/5
        [ResponseType(typeof(Models.Team))]
        public JsonResult GetTeam(string sid, string id)
        {
            int.TryParse(id, out int teamid);
            List<TeamUser> teamdetails = new List<Models.TeamUser>();
            ReturnValue rv = new ReturnValue();
            DataSet DBSetTeamdetail = null;
            rv = new TOLC.ERP.Application.Team().Retrieve(sid, teamid, ref DBSetTeamdetail);

            if (DBSetTeamdetail != null)
            {
                foreach (DataTable table in DBSetTeamdetail.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var member = new Models.TeamUser
                        {
                            Username = row["USER_PROFILE"].ToString().Trim(' '),
                            RoleID = int.Parse(row["ROLE_ID"].ToString()),
                            TeamID = teamid
                        };
                        teamdetails.Add(member);

                    }
                }
            }

            
            var result = new JsonResult();
            result.Data = JsonConvert.SerializeObject(teamdetails);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        // POST: api/Teams
        [System.Web.Http.HttpPost]
        public JsonResult CreateTeam(string sid, string name, string hierarchyid, string parentid, string refcode)
        {
            var result = new JsonResult();
            ReturnValue rv = new ReturnValue();
            int newID = 0;
            try
            {
                int.TryParse(hierarchyid, out int hierarchyidnum);
                int.TryParse(parentid, out int parentidnum);

                rv = new TOLC.ERP.Application.Team().Create(sid, name, hierarchyidnum, parentidnum, refcode, ref newID);

                result.Data = rv.Message;
            }
            catch
            {
                result.Data = "Invalid Input Parameters";
            }
            return result;
        }
        [System.Web.Http.HttpPost]
        public JsonResult UpdateTeamHierarchy(string sid, int id, string hierarchyid, string parentid)
        {
            var result = new JsonResult();
            ReturnValue rv = new ReturnValue();
            try
            {
                int.TryParse(hierarchyid, out int hierarchyidnum);
                int.TryParse(parentid, out int parentidnum);

                rv = new TOLC.ERP.Application.Team().Update(sid,id, "", hierarchyidnum, parentidnum, "");

                result.Data = rv.Message;
            }
            catch
            {
                result.Data = rv.Message;
            }
            return result;
        }
        [System.Web.Http.HttpPost]
        public JsonResult EditTeam(string sid, int id, string name, string refcode)
        {
            var result = new JsonResult();
            ReturnValue rv = new ReturnValue();
            try
            {
                rv = new TOLC.ERP.Application.Team().Update(sid, id, name, 0, 0, refcode);

                result.Data = rv.Message;
            }
            catch
            {
                result.Data = rv.Message;
            }
            return result;
        }
        // DELETE: api/Teams/5
        [System.Web.Http.HttpPost]
        [ResponseType(typeof(Models.Team))]
        public JsonResult DeleteTeam(int id, string sid)
        {
            var result = new JsonResult();
            ReturnValue rv = new TOLC.ERP.Application.Team().Delete(sid, id);
            result.Data = rv.Message;
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeamExists(int id)
        {
            return db.Teams.Count(e => e.Id == id) > 0;
        }
    }
}