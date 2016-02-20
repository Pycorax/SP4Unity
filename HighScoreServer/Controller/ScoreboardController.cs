using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HighScoreServer.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HighScoreServer.Controllers
{
    public class ScoreboardController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Submit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Submit(ScoreEntry entry)
        {
            // Check if the result is valid
            if (!ModelState.IsValid)
            {
                return View(entry);
            }

            // Save the entry asynchronously
            var db = new ScoreDataContext();
            db.Add(entry);
            await db.SaveChangesAsync();

            // Redirect to the Entry Action with an Anonymous Parameter that Entry() needs
            return RedirectToAction("Entry", new { id = entry.Id });
        }

        public IActionResult Entry(int id)
        {
            // Look in the Database
            var db = new ScoreDataContext();

            // Retrieve the score entry using the ID (via LINQ lambda syntax)
            ScoreEntry score = db.Entries.SingleOrDefault(x => id == x.Id);

            return View(score);
        }
    }
}
