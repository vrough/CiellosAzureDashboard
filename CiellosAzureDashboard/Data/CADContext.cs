using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiellosAzureDashboard.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace CiellosAzureDashboard.Data
{
    public class CADContext : DbContext
    {
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Dashboard> Dashboards { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "data.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            optionsBuilder.UseSqlite(connection);
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
