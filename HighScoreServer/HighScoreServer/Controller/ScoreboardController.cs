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
        private readonly ScoreDataContext database;

        public ScoreboardController(ScoreDataContext context)
        {
            // Save reference to the database
            database = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(database.Entries.ToArray());
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
            database.Add(entry);
            await database.SaveChangesAsync();

            // Redirect to the Entry Action with an Anonymous Parameter that Entry() needs
            return RedirectToAction("Entry", new { id = entry.Id });
        }

        public IActionResult Entry(int id)
        {
            // Retrieve the score entry using the ID (via LINQ lambda syntax)
            ScoreEntry score = database.Entries.SingleOrDefault(x => id == x.Id);

            return View(score);
        }

        public IActionResult Entry(ScoreEntry entry)
        {
            return View(entry);
        }
    }
}
