using Microsoft.EntityFrameworkCore;
using Mp3Updater.DAL;



namespace MP3FileUpdater.DAL.SQL
{
    public class SQLDatabaseContext : DbContext
    {

        public SQLDatabaseContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<Session> Session { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Mp3Updater.SQL;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>().HasKey(c => new { c.Id });
            modelBuilder.Entity<Session>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();
            modelBuilder.Entity<Mp3FileInfo>().HasKey(c => new { c.Id });

        }
    }
}

