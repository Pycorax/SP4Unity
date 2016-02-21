using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HighScoreServer.Models
{
    public class ScoreDataContext : DbContext
    {
        // Controls the amount of high scores the server can store
        private static int maxHighScores = 10;
        
        public DbSet<ScoreEntry> Entries { get; set; }
        private List<ScoreEntry> orderedEntries;
        public List<ScoreEntry> OrderedEntries
        {
            get
            {
                // If wasn't initialized
                if (orderedEntries == null)
                {
                    // Build from the DB
                    updateOrderedList();
                }
                return orderedEntries;
            }
        }
        public int MaxHighScores { get { return maxHighScores; } }

        public ScoreDataContext()
        {
            Database.EnsureCreated();
        }
        
        public void Add(ScoreEntry entry)
        {
            Entries.Add(entry);
            updateOrderedList();
        }

        public void SetMaxScores(int score)
        {
            maxHighScores = score;
            updateOrderedList();
        }

        private void updateOrderedList()
        {
            // Update the sorted list
            orderedEntries = (from e in Entries orderby e.Score descending select e).Take(maxHighScores).ToList();
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
            modelBuilder.ForSqlServerUseIdentityColumns();
        }
    }
}
