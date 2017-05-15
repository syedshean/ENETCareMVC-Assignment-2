using ENETCareMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.DAL
{
    public class DBContext : DbContext
    {
        public DBContext() : base("ENETCareAppConnection")
        {
        }

        public DbSet<District> Districts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<InterventionType> InterventionTypes { set; get; }
        public DbSet<Intervention> Interventions { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}