using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace CFirst.Models
{
    public class CFirstModelsContext : DbContext
    {

        //static CFirstModelsContext()
        //{
        //    //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CFirstModelsContext>()); //Use after creating new database instance
        //    Database.SetInitializer(new DropCreateDatabaseAlways<CFirstModelsContext>());
        //}

        public CFirstModelsContext() : base("DefaultConnection")
        {
        }

        //Tagging Schema
        public DbSet<User> Users { get; set; }
        public User GetUser(string name)
        {
            return Users.First(x => x.Name == name);
        }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Url> Urls { get; set; }
        
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Changes the mapping names to more conventional ones.
            modelBuilder.Entity<User>().HasMany(u => u.Tags).WithMany(t => t.Users);
            modelBuilder.Entity<User>().HasMany(u => u.Urls).WithMany(t => t.Users);
            modelBuilder.Entity<Url>().HasMany(u => u.Tags).WithMany(t => t.Urls);
        
           
        }
    }
}