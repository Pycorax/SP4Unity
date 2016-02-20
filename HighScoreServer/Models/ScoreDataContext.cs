using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HighScoreServer.Models
{
    public class ScoreDataContext : DbContext
    {
        public DbSet<ScoreEntry> Entries { get; set; }

        public ScoreDataContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Set the database to connect to
            var connectionString = @"Server=(LocalDb)\MSSQLLocalDb;Database=HighScoreDB";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Force the server to update the ID
            modelBuilder.UseSqlServerIdentityColumns();
        }
    }
}
