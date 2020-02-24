using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BaseChessEngine;
using Rudz.Chess.Types;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        static Engine engine;


        // GET api/values
        [HttpGet]
        public string Get([FromQuery] String fen, String move)
        {
            bool white = true;
            if (move == null)
            {
                return "njull";
            }
            if(fen == "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
            {
                engine = new Engine();
                engine.PerformMove(move);
                white = false;
            }
            else
            {
                engine.PerformMove(move);
                int counter = 0;
                while (fen[counter] != ' ')
                {
                    counter++;
                }
                if (fen[counter + 1] == 'w')
                {
                    white = false;
                }
            }
            String res = engine.PerformBestMove(5000, white);
            return res;
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
