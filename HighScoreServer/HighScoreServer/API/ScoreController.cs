using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HighScoreServer.Models;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HighScoreServer.API
{
    [Route("api/[controller]")]
    public class ScoreController : Controller
    {
        private readonly ScoreDataContext database;

        public ScoreController(HighScoreServer.Models.ScoreDataContext db)
        {
            database = db;
        }

        // GET: api/values
        [HttpGet]
        public string Get()
        {
            string easyResult = "";

            // Loop through each entry and produce an easy to process string
            foreach (var e in database.OrderedEntries)
            {
                easyResult += e.Name + "-" + e.Score + ",";
            }

            return easyResult;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<string> Post(ScoreEntry value)
        {
            if (value != null)
            {
                database.Add(value);
                await database.SaveChangesAsync();

                return "Success";
            }
            else
            {
                return "Failed";
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
