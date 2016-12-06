using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace CFirst.Models
{
    public class DbInitialization
        // : DropAndCreateTables
        // : DropCreateDatabaseAlways<CFirstModelsContext> 
        // : IDatabaseInitializer<CFirstModelsContext>
        // : MigrateDatabaseToLatestVersion<CFirstModelsContext, Migrations.CFirstModelsContext> 
         : DropCreateDatabaseIfModelChanges<CFirstModelsContext> //Use after creating new database instance
    {
        protected override void Seed(CFirstModelsContext context)
        {
            var Db = new CFirstModelsContext();
            //Tags

            Db.Tags.Add(new Tag() { Name = "Like" });
            Db.Tags.Add(new Tag() { Name = "DisLike" });
            Db.Tags.Add(new Tag() { Name = "Important" });
            Db.Tags.Add(new Tag() { Name = "News" });
            Db.Tags.Add(new Tag() { Name = "Competitor" });
            Db.SaveChanges();
            //Users
            Db.Users.Add(new User() { Name = "Jane" });
            Db.Users.Add(new User() { Name = "Tom" });
            Db.SaveChanges();
            //Urls
            Db.Urls.Add(new Url() { Address = "http://localhost:49558/Url" });
            Db.SaveChanges();
          
        }

        

    }
}
