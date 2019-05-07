using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiellosAzureDashboard.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft;
using System.IO;
using System.Threading;

namespace CiellosAzureDashboard.Data
{
    public class CADContext : DbContext
    {
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Dashboard> Dashboards { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<VM> VMs { get; set; }
        public virtual DbSet<ActiveVM> ActiveVMs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "data.db" };
            connectionStringBuilder.Cache = SqliteCacheMode.Shared;
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            optionsBuilder.UseSqlite(connection);

        }

        public override int SaveChanges()
        {
            //if(AzureHelperService.AzureHelper != null)
            //while (AzureHelperService.AzureHelper.IsDBLocked)
            //{
            //    Thread.Sleep(1000);
            //}
            return base.SaveChanges();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure primary key
            modelBuilder.Entity<Application>().HasKey(s => s.AppId);
            modelBuilder.Entity<Log>().HasKey(s => s.logId);
            modelBuilder.Entity<Setting>().HasKey(s => s.settingId);
            modelBuilder.Entity<Link>().HasKey(s => s.linkId);
            modelBuilder.Entity<Dashboard>().HasKey(s => s.DashboardId);
            modelBuilder.Entity<User>().HasKey(s => s.UserId);
            modelBuilder.Entity<VM>().HasKey(s => s.Id);
            modelBuilder.Entity<ActiveVM>().HasKey(s => s.Id);

            modelBuilder.Entity<DashboardApplication>()
                .HasKey(bc => new { bc.DashboardId, bc.ApplicationId });
            modelBuilder.Entity<DashboardApplication>()
                .HasOne(bc => bc.Dashboard)
                .WithMany(b => b.DashboardApplications)
                .HasForeignKey(bc => bc.DashboardId);
            modelBuilder.Entity<DashboardApplication>()
                .HasOne(bc => bc.Application)
                .WithMany(c => c.DashboardApplications)
                .HasForeignKey(bc => bc.ApplicationId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
