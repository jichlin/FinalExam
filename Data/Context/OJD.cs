using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Data.Context
{
    public class OJD : DbContext
    {
        public OJD() : base("OJD")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<OJD>(null);
            base.OnModelCreating(modelBuilder);
        }


        public virtual void Commit()
        {
            base.SaveChanges();
        }

        public System.Data.Entity.DbSet<FinalExamModels.User> Users { get; set; }

        public System.Data.Entity.DbSet<FinalExamModels.Projects> Projects { get; set; }

        public System.Data.Entity.DbSet<FinalExamModels.Sprint> Sprints { get; set; }

        public System.Data.Entity.DbSet<FinalExamModels.WorkItem> WorkItems { get; set; }
    }
}
