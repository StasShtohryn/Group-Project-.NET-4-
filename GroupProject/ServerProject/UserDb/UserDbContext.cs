using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModel;

namespace ServerProject.UserDb
{
    internal class UserDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserDbContext() 
        {
        }

        public UserDbContext(DbContextOptions<UserDbContext> options):
            base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseLazyLoadingProxies()
                .UseSqlServer(config.GetConnectionString("SqlClient"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
        }
    }
}
