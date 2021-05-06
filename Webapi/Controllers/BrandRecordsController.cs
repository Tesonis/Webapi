using IBM.Data.DB2.iSeries;
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
using Newtonsoft.Json;

namespace Webapi.Controllers
{
    public class BrandRecordsController : ApiController
    {
        private dbContext db = new dbContext();

        // GET: api/BrandRecords
        public JsonResult GetBrands(string sid)
        {
            List<BrandRecord> brandlist = new List<BrandRecord>();
            ReturnValue rv = new ReturnValue();
            DataSet DBSetBrands = null;
            Brand Brands = new Brand();

            Brands.List(sid, ref DBSetBrands);
            if (DBSetBrands != null)
            {
                foreach (DataTable table in DBSetBrands.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var brand = new BrandRecord
                        {
                            Id = row["PRNPRN"].ToString().Trim(' '),
                            BrandName = row["PRNNAM"].ToString()
                        };
                        brandlist.Add(brand);
                    }
                }
            }
            var result = new JsonResult();
            result.Data = JsonConvert.SerializeObject(brandlist);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        // GET: api/BrandRecords/5
        [ResponseType(typeof(BrandRecord))]
        public IHttpActionResult GetBrandRecord(int id)
        {
            BrandRecord brandRecord = db.Brands.Find(id);
            if (brandRecord == null)
            {
                return NotFound();
            }

            return Ok(brandRecord);
        }

        // PUT: api/BrandRecords/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBrandRecord(string id, BrandRecord brandRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != brandRecord.Id)
            {
                return BadRequest();
            }

            db.Entry(brandRecord).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (id == "" || id == null)
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

        // POST: api/BrandRecords
        [ResponseType(typeof(BrandRecord))]
        public IHttpActionResult PostBrandRecord(BrandRecord brandRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Brands.Add(brandRecord);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = brandRecord.Id }, brandRecord);
        }

        // DELETE: api/BrandRecords/5
        [ResponseType(typeof(BrandRecord))]
        public IHttpActionResult DeleteBrandRecord(int id)
        {
            BrandRecord brandRecord = db.Brands.Find(id);
            if (brandRecord == null)
            {
                return NotFound();
            }

            db.Brands.Remove(brandRecord);
            db.SaveChanges();

            return Ok(brandRecord);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}