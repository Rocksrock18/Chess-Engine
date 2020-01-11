using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseChessEngine;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        static Engine engine = new Engine();
        [HttpGet]
        public string Get([FromQuery]string fen)
        {
            if (fen == null)
            {
                return "null from test";
            }
            engine.PerformMove(fen);
            /*
            int counter = 0;
            while (fen[counter] != ' ')
            {
                counter++;
            }
            String move = "error";
            if (fen[counter + 1] == 'b')
            {
                move = engine.GetBestMove(10000, false);
            }
            if(fen[counter + 1] == 'w')
            {
                move = engine.GetBestMove(10000, true);
            }
            */

            String move = engine.PerformBestMove(8000, false);

            return move;
        }
    }
}