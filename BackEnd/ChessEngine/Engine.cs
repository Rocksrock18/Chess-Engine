using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rudz.Chess;
using Rudz.Chess.Enums;
using Rudz.Chess.Types;
using Rudz.Chess.Factories;

namespace BaseChessEngine
{
    public class Engine
    {
        private System.Timers.Timer timer;
        bool end;
        Move bestMove;
        public Game game;
        public Engine()
        {
            var position = new Position();
            game = (Game)GameFactory.Create(position);
            game.NewGame();
            end = false;
        }
        public String PerformBestMove(int timeLimit, bool white) //cplay 1000 w or cplay 1500 b
        {
            Move move = CalculateBestMove(timeLimit, white);
            //Console.WriteLine("Move Performed: " + move);
            game.MakeMove(move);
            //Console.WriteLine(game);
            end = false;
            timer.Stop();
            return move.ToString();
        }
        public bool PerformMove(String mv) //hplay e2e4
        {
            var moveList = new MoveGenerator(game.Position).Moves;
            foreach (Move m in moveList)
            {
                if (m.ToString().Equals(mv))
                {
                    game.MakeMove(m);
                    return true;
                }
            }
            return false;
        }
        public String GetBestMove(int timeLimit, bool white) //suggest 1000 w or suggest 5000 b
        {
            Move move = CalculateBestMove(timeLimit, white);
            //Console.WriteLine("Move Evaluated: " + move);
            end = false;
            timer.Stop();
            return move.ToString();
        }
        private Move CalculateBestMove(int timeLimit, bool white)
        {
            setTimer(timeLimit);
            Move oldMove = new Move();
            int maxDepth = 1;
            int score = 0;
            String fen = game.GetFen().Fen;
            while (!end)
            {
                if (white)
                {
                    score = AlphaBetaMax(maxDepth, maxDepth, int.MinValue, int.MaxValue);
                }
                else
                {
                    score = AlphaBetaMin(maxDepth, maxDepth, int.MinValue, int.MaxValue);
                }
                if (!end)
                {
                    oldMove = bestMove;
                    maxDepth++;
                }
                game.SetFen(fen);
            }
            //Console.WriteLine("Max depth: " + maxDepth);
            return oldMove;
        }
        private int AlphaBetaMax(int currentDepth, int initialDepth, int alpha, int beta)
        {
            if (currentDepth == 0)
            {
                return QuiescenceMax(alpha, beta);
            }
            int score = 0;
            var moveList = new MoveGenerator(game.Position).Moves;
            if (moveList.Count == 0)
            {
                return int.MinValue;
            }
            foreach (Move m in moveList)
            {
                if (end)
                {
                    break;
                }
                game.MakeMove(m);
                score = AlphaBetaMin(currentDepth - 1, initialDepth, alpha, beta);
                if (score >= beta)
                {
                    if (currentDepth == initialDepth)
                    {
                        bestMove = m;
                    }
                    game.TakeMove();
                    return beta;
                }
                if (score > alpha)
                {
                    if (currentDepth == initialDepth)
                    {
                        bestMove = m;
                    }
                    alpha = score;
                }
                game.TakeMove();
            }
            return alpha;
        }
        private int AlphaBetaMin(int currentDepth, int initialDepth, int alpha, int beta)
        {
            if (currentDepth == 0)
            {
                return QuiescenceMin(alpha, beta);
            }
            int score = 0;
            var moveList = new MoveGenerator(game.Position).Moves;
            if (moveList.Count == 0)
            {
                return int.MaxValue;
            }
            foreach (Move m in moveList)
            {
                if (end)
                {
                    break;
                }
                game.MakeMove(m);
                score = AlphaBetaMax(currentDepth - 1, initialDepth, alpha, beta);
                if (score <= alpha)
                {
                    if (currentDepth == initialDepth)
                    {
                        bestMove = m;
                    }
                    game.TakeMove();
                    return alpha;
                }
                if (score < beta)
                {
                    if (currentDepth == initialDepth)
                    {
                        bestMove = m;
                    }
                    beta = score;
                }
                game.TakeMove();
            }
            return beta;
        }
        private int QuiescenceMax(int alpha, int beta)
        {
            int standPat = Eval.MainEvaluation(game.Position.BoardLayout);
            if (standPat >= beta)
            {
                return beta;
            }
            if (standPat > alpha)
            {
                alpha = standPat;
            }
            int score = 0;
            var moveList = new MoveGenerator(game.Position, true, true).Moves;
            foreach (Move m in moveList)
            {
                if (end)
                {
                    break;
                }
                game.MakeMove(m);
                score = QuiescenceMin(alpha, beta);
                if (score >= beta)
                {
                    game.TakeMove();
                    return beta;
                }
                if (score > alpha)
                {
                    alpha = score;
                }
                game.TakeMove();
            }
            return alpha;
        }
        private int QuiescenceMin(int alpha, int beta)
        {
            int standPat = Eval.MainEvaluation(game.Position.BoardLayout);
            if (standPat <= alpha)
            {
                return alpha;
            }
            if (standPat < beta)
            {
                beta = standPat;
            }
            int score = 0;
            var moveList = new MoveGenerator(game.Position, true, true).Moves;
            foreach (Move m in moveList)
            {
                if (end)
                {
                    break;
                }
                game.MakeMove(m);
                score = QuiescenceMax(alpha, beta);
                if (score <= alpha)
                {
                    game.TakeMove();
                    return alpha;
                }
                if (score < beta)
                {
                    beta = score;
                }
                game.TakeMove();
            }
            return beta;
        }
        private void setTimer(int timeLimit)
        {
            timer = new System.Timers.Timer(timeLimit);
            timer.Elapsed += Terminate;
            timer.Enabled = true;
        }
        private void Terminate(Object source, System.Timers.ElapsedEventArgs e)
        {
            end = true;
        }
    }
}
