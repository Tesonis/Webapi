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
        public JsonResult GetTeams()
        {
            List<Models.Team> teamlist = new List<Models.Team>();
            ReturnValue rv = new ReturnValue();
            DataSet DBSetTeams = null;
            TOLC.ERP.Application.Team teams = new TOLC.ERP.Application.Team();
            Session session = null;
            rv = new Security().Logon("FSPRC_TEST", "viking", ref session, false);
            var sec = session.securityIdentifier;

            teams.List(sec, ref DBSetTeams);
            if (DBSetTeams != null)
            {
                foreach (DataTable table in DBSetTeams.Tables)
                {
                    foreach (DataRow row in table.Rows)
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
            var result = new JsonResult();
            result.Data = JsonConvert.SerializeObject(teamlist);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        // GET: api/Teams/5
        [ResponseType(typeof(Models.Team))]
        public async Task<IHttpActionResult> GetTeam(int id)
        {
            Models.Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        // PUT: api/Teams/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTeam(int id, Models.Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != team.Id)
            {
                return BadRequest();
            }

            db.Entry(team).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Teams
        [ResponseType(typeof(Models.Team))]
        public async Task<IHttpActionResult> PostTeam(Models.Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teams.Add(team);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = team.Id }, team);
        }

        // DELETE: api/Teams/5
        [ResponseType(typeof(Models.Team))]
        public async Task<IHttpActionResult> DeleteTeam(int id)
        {
            Models.Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            db.Teams.Remove(team);
            await db.SaveChangesAsync();

            return Ok(team);
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