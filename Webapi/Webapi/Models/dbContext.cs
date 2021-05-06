using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Webapi.Models
{
    public class dbContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
        //private IBM.Data.DB2.iSeries.iDB2Connection cn = null;
        public dbContext() : base("name=dbContext")
        {
        }

        //private IBM.Data.DB2.iSeries.iDB2Connection OpenConnection(string connectionString)
        //{
        //    cn = new IBM.Data.DB2.iSeries.iDB2Connection()
        //    {
        //        ConnectionString = connectionString
        //    };

        //    cn.Open();

        //    return cn;
        //}

        public System.Data.Entity.DbSet<Webapi.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<Webapi.Models.BrandRecord> Brands { get; set; }

        public System.Data.Entity.DbSet<Webapi.Models.Team> Teams { get; set; }

    }
}
