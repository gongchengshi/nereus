using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Nereus.Models
{
   public class NereusDb : DbContext
   {
      public NereusDb() : base("DefaultConnection")
      {
         //Configuration.LazyLoadingEnabled = true;
      }

      // Account Schema
      public DbSet<User> Users { get; set; }

      public User GetUser(string name)
      {
         return Users.First(x => x.UserName == name);
      }
      //Settings Schema
      public DbSet<UserUISettings> UserUISettings { get; set; }
      // Search Schema
      public DbSet<ProjectUrl> ProjectUrls { get; set; }
      public DbSet<UserProjectUrl> UserProjectUrls { get; set; }
      public DbSet<ProjectQuery> ProjectQueries { get; set; }
      public DbSet<SearchProfile> SearchProfiles { get; set; }
      public DbSet<UserSearchProfile> UserSearchProfiles { get; set; }
      public DbSet<UserSearchSettings> UserSearchSettings { get; set; }
      public DbSet<SearchProvider> SearchProviders { get; set; }

      public DbSet<ProjectUrlPattern> ProjectUrlPatterns { get; set; }

      // Browsing Schema
      public DbSet<Url> Urls { get; set; }
      public DbSet<UserUrl> UserUrls { get; set; }

      // Project Schema
      public DbSet<Project> Projects { get; set; }
      public IEnumerable<Project> GetUserProjects(User user)
      {
         return from x in Projects
                where (x.Users.Count(y => y.Id == user.Id) > 0)
                select x;
      }

      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);
         modelBuilder.Entity<Project>().HasMany(p => p.Users).
                      WithMany(u => u.Projects);

         modelBuilder.Entity<User>().HasMany(u => u.Projects).WithMany(p => p.Users);
      }
   }
}
