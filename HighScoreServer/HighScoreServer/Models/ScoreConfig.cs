using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HighScoreServer.Models
{
    public class ScoreConfig
    {
        [Required]
        public int MaxScores { get; set; }
    }
}
