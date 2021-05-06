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
    public class CustomersController : ApiController
    {
        private dbContext db = new dbContext();

        // GET: api/Customers
        public JsonResult GetCustomers(string sid)
        {
            List<Customer> customerlist = new List<Customer>();
            ReturnValue rv = new ReturnValue();
            DataSet DBSetKA = null;
            KeyAccount KA = new KeyAccount();

            KA.List(sid,0, ref DBSetKA);
            if (DBSetKA != null)
            {
                foreach (DataTable table in DBSetKA.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var c = new Customer
                        {
                            Id = row["KACKAC"].ToString().Trim(' '),
                            CustomerName = row["KACNAM"].ToString()
                        };
                        customerlist.Add(c);
                    }
                }
            }
            var result = new JsonResult();
            result.Data = JsonConvert.SerializeObject(customerlist);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
        
    }
}