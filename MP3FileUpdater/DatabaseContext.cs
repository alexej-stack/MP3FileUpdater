using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Text;

namespace MP3FileUpdater.Core
{
    public class DatabaseContext:DbContext
    {

        public DatabaseContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<ProgressFiles> ProgressFiles { get; set; }
       


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Mp3Updater;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            modelBuilder.Entity<ProgressFiles>().HasKey(c=>new {c.Id});
            modelBuilder.Entity<ProgressFiles>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();
            modelBuilder.Entity<Mp3File>().HasKey(c=>new { c.Name });

        }
    }
}
