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
            Console.WriteLine("Commands:");
            Engine engine = new Engine();
            /*
            engine.game.SetFen("6k1/7p/8/5pp1/5P2/7q/8/4K3_w_-_g6_0_101");
            //engine.game.SetFen("3R4/1r4k1/4Bpp1/2pPp3/3nP3/6qP/P2Q2P1/6K1 w - - 5 34");
            engine.game.Position.SetCastle(true);
            engine.game.Position.SetCastle(false);
            */
            while (!engine.game.Position.IsMate())
            {
                //String key = $"0x{engine.game.State.Key:X}";
                String input = Console.ReadLine();
                String[] commands = input.Split(' ');
                if (commands[0].Equals("h"))
                {
                    engine.PerformMove(commands[1]);
                }
                if(commands[0].Equals("c"))
                {
                    int time = Int32.Parse(commands[1]);
                    bool white = commands[2].Equals("w") ? true : false;
                    Console.WriteLine(engine.PerformBestMove(time, white));
                }
                if(commands[0].Equals("suggest"))
                {
                    int time = Int32.Parse(commands[1]);
                    bool white = commands[2].Equals("w") ? true : false;
                    Console.WriteLine(engine.GetBestMove(time, white));
                }
                if(commands[0].Equals("w"))
                {
                    Console.WriteLine(engine.PerformBestMove(10000, true));
                }
                if (commands[0].Equals("b"))
                {
                    Console.WriteLine(engine.PerformBestMove(10000, false));
                }
                if (commands[0].Equals("auto"))
                {
                    int time = Int32.Parse(commands[1]);
                    while(!engine.game.Position.IsMate())
                    {
                        Console.WriteLine(engine.PerformBestMove(time, true));
                        Console.WriteLine(engine.game);
                        if (engine.game.Position.IsMate())
                        {
                            break;
                        }
                        if (engine.game.MoveNumber > 25)
                        {
                            Eval.SetEnd();
                        }
                        Console.WriteLine(engine.PerformBestMove(time, false));
                        Console.WriteLine(engine.game);
                        if (engine.game.MoveNumber > 25)
                        {
                            Eval.SetEnd();
                        }
                    }
                    
                }
                Console.WriteLine(engine.game);
                if (engine.game.MoveNumber > 25)
                {
                    Eval.SetEnd();
                }
            }
            Console.WriteLine("Checkmate");
            Console.ReadKey();
        }
    }
}
