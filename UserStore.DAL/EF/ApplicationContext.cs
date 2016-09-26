using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using UserStore.DAL.Entities;

namespace UserStore.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer(new MyInitializator());
        }

        public DbSet<ClientProfile> ClientProfiles { get; set; }

        public DbSet<AbstractContent> Contents { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Translator> Transletors { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Scenarist> Writers { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Check> Checks { get; set; }
        public DbSet<UpDown> UpDowns { get; set; }
        public DbSet<Vote> Votes { get; set; }
        
    }
    class MyInitializator : DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            
            Check newCheck = new Check {Id = 1, Name = "Not Checked" };
            Check newCheck2 = new Check { Id = 2, Name = "Checked" };
            Check newCheck3 = new Check { Id = 3, Name = "Delete" };
            UpDown up = new UpDown { Id = 1, Vote = 1 };
            UpDown down = new UpDown { Id = 2, Vote = -1 };
            
            context.UpDowns.Add(up);
            context.UpDowns.Add(down);
            context.Checks.Add(newCheck);
            context.Checks.Add(newCheck2);
            context.Checks.Add(newCheck3);
            base.Seed(context);
        }
        
    }
}
