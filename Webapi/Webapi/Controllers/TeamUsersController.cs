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
    public class TeamUsersController : ApiController
    {
        private dbContext db = new dbContext();

        // GET: api/Users
        public JsonResult GetUserTeams(string sid, string username)
        {
            var result = new JsonResult();
            DataSet ds = new DataSet();
            ReturnValue rv = new ReturnValue();
            List<TeamUser> userteamlist = new List<TeamUser>();
            try
            {
                rv = new TOLC.ERP.Application.User().ListTeams(sid, username, ref ds);
                if (rv.Number != 0)
                {
                    result.Data = rv.Message;
                    return result;
                }
                else
                {
                    if (ds != null)
                    {
                        foreach (DataTable table in ds.Tables)
                        {
                            foreach (DataRow row in table.Rows)
                            {

                                var team = new TeamUser
                                {
                                    Username = username,
                                    RoleID = int.Parse(row["ROLE_ID"].ToString()),
                                    TeamID = int.Parse(row["TEAM_ID"].ToString())
                                };
                                userteamlist.Add(team);
                            }
                        }
                    }
                }
                result.Data = JsonConvert.SerializeObject(userteamlist);
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            catch
            {
                result.Data = "Invalid Parameters";
            }
            return result;
        }
        // POST: api/TeamUsers
        [ResponseType(typeof(TeamUser))]
        public JsonResult ChangeTeamUser(string sid, int id, string username, string role, string action)
        {
            var result = new JsonResult();
            ReturnValue rv = new ReturnValue();
            try
            {
                int.TryParse(role, out int roleid);
                if (action.ToLower() == "create" || action.ToLower() == "update")
                {
                    rv = new TOLC.ERP.Application.Team().ChangeMember(sid, id, username, roleid, action);
                    result.Data = rv.Message;
                }
                else
                {
                    result.Data = "Invalid Action";
                }

                
            }
            catch
            {
                result.Data = "Invalid Parameters";
            }
            return result;
        }
        [System.Web.Http.HttpPost]
        // DELETE: api/TeamUsers/5
        [ResponseType(typeof(TeamUser))]
        public JsonResult DeleteTeamUser(string sid, int id, string username)
        {
            var result = new JsonResult();
            ReturnValue rv = new ReturnValue();
            try
            {
                    rv = new TOLC.ERP.Application.Team().DeleteMember(sid, id, username);
                    result.Data = rv.Message;
            }
            catch
            {
                result.Data = "Invalid Parameters";
            }
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

        private bool TeamUserExists(int id)
        {
            return db.TeamUsers.Count(e => e.TeamID == id) > 0;
        }
    }
}