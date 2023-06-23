using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.DbModels;

namespace TestData.Data
{
    public class DBContext : DbContext
    {
        public DBContext() { }
        public DBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("MY_DATABASE_CONNECTIONSTRING", EnvironmentVariableTarget.Machine));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .Property(e => e.Price)
                .IsRequired()
                .HasAnnotation("Range", new RangeAttribute(1.0, 3000.0).ToString());

            modelBuilder.Entity<User>()
                .HasIndex(e => e.UserName)
                .IsUnique();
        }
    }
}
