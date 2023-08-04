using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.DbModels;

namespace TestData.Data
{
    public class DBContext : DbContext
    {

        public DBContext()
        {

        }
        public DBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionString = Environment.GetEnvironmentVariable("MY_DATABASE_CONNECTIONSTRING", EnvironmentVariableTarget.Machine);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }

            optionsBuilder.UseSqlServer(connectionString, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });

#if DEBUG
            optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
#endif

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .Property(e => e.Price)
                .IsRequired()
                .HasAnnotation("Range", new RangeAttribute(1.0, 3000.0).ToString());

            modelBuilder.Entity<Game>()
                .Property(e => e.Rate)
                .HasAnnotation("Range", new RangeAttribute(0.0, 5.0).ToString());

            modelBuilder.Entity<User>()
                .Property(e => e.Balance)
                .HasAnnotation("Range", new RangeAttribute(0, 50000).ToString());

            modelBuilder.Entity<User>()
                .HasIndex(e => e.UserName)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(e => e.Name)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(e => e.IsEmailConfirmed)
                .IsRequired()
                .HasDefaultValue(false)
                .ValueGeneratedNever();

            modelBuilder.Entity<User>()
                .HasIndex(e => e.ConfirmationToken)
                .IsUnique();

            modelBuilder.Entity<User>()
            .HasMany(u => u.Games)
            .WithOne(g => g.User)
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
