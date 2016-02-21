using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HighScoreServer.Models;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HighScoreServer.Controllers
{
    public class AdminController : Controller
    {
        private readonly ScoreDataContext database;

        public AdminController(ScoreDataContext context)
        {
            database = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            ScoreConfig c = new ScoreConfig();
            c.MaxScores = database.MaxHighScores;

            return View("Index", c);
        }

        [HttpPost]
        public IActionResult Index(ScoreConfig config)
        {
            database.SetMaxScores(config.MaxScores);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}
