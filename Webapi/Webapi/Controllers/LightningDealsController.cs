using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using TOLC.ERP.Application;
using Webapi.Models;

namespace Webapi.Controllers
{
    public class LightningDealsController : ApiController
    {
        // GET api/<controller>
        public JsonResult Get()
        {
            List<LightningDeal> lightningDealsList = new List<LightningDeal>();
            ReturnValue rv = new ReturnValue();
            DataSet DBSetTeams = null;
            TOLC.ERP.Application.Clearance clearance = new TOLC.ERP.Application.Clearance();
            Session session = null;
            rv = new Security().Logon("FSPRC_TEST", "viking", ref session, false);
            var sec = session.securityIdentifier;

            clearance.List(sec, ref DBSetTeams);
            if (DBSetTeams != null)
            {
                foreach (DataTable table in DBSetTeams.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var lightningDealItem = new Models.LightningDeal
                        {
                            BranchNumber = int.Parse(row["BRANCH"].ToString()),
                            BrandName = row["BRAND_NAME"].ToString(),
                            ItemNumber = row["ITEM"].ToString(),
                            ItemDescription = row["ITEM_DECRIPTION"].ToString(),
                            Category = row["STATUS"].ToString(),
                            BestBeforeDate = Convert.ToDateTime(row["BEST_BEFORE_DATE"], new CultureInfo("en-US")),
                            TagNumber = row["TAG"].ToString(),
                            RegularPrice = decimal.Parse(row["REGULAR_PRICE"].ToString()),
                            DiscountPct = decimal.Parse(row["DISCOUNT_PCT"].ToString()),
                            NetPrice = decimal.Parse(row["NET_PRICE"].ToString()),
                        };
                        lightningDealsList.Add(lightningDealItem);
                    }
                }
            }
            var result = new JsonResult();
            result.Data = JsonConvert.SerializeObject(lightningDealsList);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        // GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}