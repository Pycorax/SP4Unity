using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HighScoreServer.Models
{
    public class ScoreEntry
    {
        // Provide the ID for EntityFramework LocalDB to use as the primary key
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public int Score { get; set; }
    }
}
