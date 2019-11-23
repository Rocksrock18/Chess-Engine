using System;
using Rudz.Chess;
using Rudz.Chess.Factories;
using System.Collections.Generic;
using Rudz.Chess.Enums;
using Rudz.Chess.Types;
using BaseChessEngine;

namespace BaseChessEngine
{
    class ChessRunner
    {
        static void Main(string[] args)
        {
            Engine engine = new Engine();
            Console.WriteLine(engine.game);
            Console.WriteLine("Commands:");
            while (true)
            {
                String input = Console.ReadLine();
                String[] commands = input.Split(' ');
                if (commands[0].Equals("hplay"))
                {
                    engine.PerformMove(commands[1]);
                }
                if (commands[0].Equals("cplay"))
                {
                    int time = Int32.Parse(commands[1]);
                    bool white = commands[2].Equals("w") ? true : false;
                    Console.WriteLine(engine.PerformBestMove(time, white));
                }
                if (commands[0].Equals("suggest"))
                {
                    int time = Int32.Parse(commands[1]);
                    bool white = commands[2].Equals("w") ? true : false;
                    Console.WriteLine(engine.GetBestMove(time, white));
                }
                Console.WriteLine(engine.game);
            }
        }
    }
}
