using ENETCareMVCApp.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Data
{
    public class DBContext : DbContext
    {
        public DBContext() : base("ENETCareAppConnection")
        {
            Configuration.LazyLoadingEnabled = true;
        }

        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<InterventionType> InterventionTypes { set; get; }
        public virtual DbSet<Intervention> Interventions { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}