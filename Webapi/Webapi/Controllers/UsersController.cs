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
    public class UsersController : ApiController
    {
        private dbContext db = new dbContext();

        // GET: api/Users
        public IQueryable<Models.User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(Models.User))]
        public JsonResult GetUser(int hid)
        {
            List<Models.User> userlist = new List<Models.User>();
            ReturnValue rv = new ReturnValue();
            DataSet DBSetUsers = null;
            TOLC.ERP.Application.Team teams = new TOLC.ERP.Application.Team();
            Session session = null;
            rv = new Security().Logon("FSPRC_TEST", "viking", ref session, false);
            var sec = session.securityIdentifier;

            rv = new TOLC.ERP.Application.User().ListbyHierarchy(sec, hid, ref DBSetUsers);
            if (DBSetUsers != null)
            {
                foreach (DataTable table in DBSetUsers.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var user = new Models.User
                        {
                            Username = row["USRUSR"].ToString(),
                            Fullname = row["USRNAM"].ToString(),
                        };
                        userlist.Add(user);
                    }
                }
            }
            var result = new JsonResult();
            result.Data = JsonConvert.SerializeObject(userlist);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
        
        private bool UserExists(string uname)
        {
            return db.Users.Count(e => e.Username == uname) > 0;
        }
    }
}