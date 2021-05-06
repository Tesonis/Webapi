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
    public class BrandItemsController : ApiController
    {
        private dbContext db = new dbContext();

        // GET: api/BrandItems
        public JsonResult GetBrandItems( string brand)
        {
            List<ItemPayload> itemidlist = new List<ItemPayload>();
            List<BrandItem> cmGroupItemList = new List<BrandItem>();
            DataSet DBSetBranditems = null;
            ReturnValue rv = new ReturnValue();

            Session session = null;
            rv = new Security().Logon("FSPRC_TEST", "viking", ref session, false);
            var sid = session.securityIdentifier;

            try
            {
                rv = new Item().ListbyBrandandTeam(sid, brand, ref DBSetBranditems);

                if (DBSetBranditems != null)
                {
                    foreach (DataTable table in DBSetBranditems.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            BrandItem item = new BrandItem
                            {
                                //refer to rowid in akitm minus 1
                                itemID = row["ITEM"].ToString(),
                                itemdescription = row["ITEM_DESCRIPTION"].ToString(),
                                unitsize = row["ITEM_SIZE"].ToString(),
                                unitspercase = Int32.Parse(row["UNIT_PER_CASE"].ToString()),
                                movingaveragecost = Decimal.Parse(row["MAC"].ToString()),
                                warehousecat = row["WAREHOUSE_CATEGORY"].ToString(),
                                shelflife = Int32.Parse(row["SHELF_LIFE"].ToString()),
                                purchaseprice = 0.00m,
                                wholesaleprice = 0.00m,
                                dutypct = Decimal.Parse(row["DUTY_PCT"].ToString())
                            };
                            ItemPayload i = new ItemPayload
                            {
                                ItemNumber = row["ITEM"].ToString()
                            };
                            cmGroupItemList.Add(item);
                            itemidlist.Add(i);
                        }
                    }
                }
                ////Retrive Avg purchase price for each item
                //rv = new Item().RetrieveAveragePurchasePricePerCase(sid, ref itemidlist, vendor);
                //rv = new Item().RetrievePrice(sid, ref itemidlist);
                //rv = new Item().RetrieveSalesHistory(sid, ref itemidlist);
                //for (int i = 0; i < cmGroupItemList.Count; i++)
                //{
                //    cmGroupItemList[i].purchaseprice = Math.Round(itemidlist[i].AveragePurchasePricePerCase, 2);
                //    //Add wholesale and DSD price based on zone value
                //    if (zone == "3")
                //    {
                //        cmGroupItemList[i].wholesaleprice = Math.Round(itemidlist[i].Zone3WholesalePrice, 2);
                //        cmGroupItemList[i].dsdprice = Math.Round(itemidlist[i].Zone3DSDPrice, 2);
                //        cmGroupItemList[i].retailprice = Math.Round(itemidlist[i].Zone3RetailPrice, 2);

                //    }
                //    else
                //    {
                //        cmGroupItemList[i].wholesaleprice = Math.Round(itemidlist[i].Zone1WholesalePrice, 2);
                //        cmGroupItemList[i].dsdprice = Math.Round(itemidlist[i].Zone1DSDPrice, 2);
                //        cmGroupItemList[i].retailprice = Math.Round(itemidlist[i].Zone1RetailPrice, 2);
                //    }


                //    cmGroupItemList[i].totalsales = Math.Round(itemidlist[i].SalesHistoryGrossDollars, 2);
                //    cmGroupItemList[i].unitssold = itemidlist[i].SalesHistoryCases;

                //    //Warehouse Category Expanded
                //    if (cmGroupItemList[i].warehousecat == "F")
                //    {
                //        cmGroupItemList[i].warehousecat = "Frozen";
                //    }
                //    else if (cmGroupItemList[i].warehousecat == "P")
                //    {
                //        cmGroupItemList[i].warehousecat = "Perishable";
                //    }
                //    else
                //    {
                //        cmGroupItemList[i].warehousecat = "Dry";
                //    }
                //}
                
            }
            catch
            {

            }
            var result = new JsonResult();
            result.Data = JsonConvert.SerializeObject(cmGroupItemList);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
        
    }
}