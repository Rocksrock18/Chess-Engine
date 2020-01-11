using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BaseChessEngine;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        static Engine engine = new Engine();


        // GET api/values
        [HttpGet]
        public string Get([FromQuery] String fen)
        {
            if (fen == null)
            {
                return "njull";
            }
            engine.game.SetFen(fen);
            String move = engine.GetBestMove(1000, true);
            return move;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public ActionResult<string> Post([FromBody] string value)
        {
            engine.PerformMove(value);
            String move = engine.PerformBestMove(5000, true);
            return move;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
