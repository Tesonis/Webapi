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
    public class AuthsController : ApiController
    {
        private dbContext db = new dbContext();

        // GET: api/Auths
        public IQueryable<Auth> GetAuths()
        {
            return db.Auths;
        }

        // GET: api/Auths?json
        [ResponseType(typeof(Auth))]
        public JsonResult GetAuth(string json)
        {
            string decodedjson = Base64Decode(json);
            Auth auth = JsonConvert.DeserializeObject<Auth>(decodedjson);
            Session session = null;
            ReturnValue rv = new Security().Logon(auth.Username, auth.Password, ref session, false);

            var result = new JsonResult();
            result.Data = JsonConvert.SerializeObject(session);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
        private string Base64Decode(string password)
        {
            var passwordstr = System.Convert.FromBase64String(password);
            return System.Text.Encoding.UTF8.GetString(passwordstr);
        }
        // PUT: api/Auths/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAuth(string id, Auth auth)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != auth.Username)
            {
                return BadRequest();
            }

            db.Entry(auth).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthExists(id))
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

        // POST: api/Auths
        [ResponseType(typeof(Auth))]
        public async Task<IHttpActionResult> PostAuth(Auth auth)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Auths.Add(auth);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AuthExists(auth.Username))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = auth.Username }, auth);
        }

        // DELETE: api/Auths/5
        [ResponseType(typeof(Auth))]
        public async Task<IHttpActionResult> DeleteAuth(string id)
        {
            Auth auth = await db.Auths.FindAsync(id);
            if (auth == null)
            {
                return NotFound();
            }

            db.Auths.Remove(auth);
            await db.SaveChangesAsync();

            return Ok(auth);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AuthExists(string id)
        {
            return db.Auths.Count(e => e.Username == id) > 0;
        }
    }
}