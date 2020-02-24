using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rudz.Chess;
using Rudz.Chess.Enums;
using Rudz.Chess.Types;
using Rudz.Chess.Factories;
using HtmlAgilityPack;

namespace BaseChessEngine
{
    public class Engine
    {
        private System.Timers.Timer timer;
        public Dictionary<string, int> transpositionTableA;
        public Dictionary<string, int> transpositionTableB;
        public Dictionary<string, MoveList> transpositionTableM;
        public Dictionary<string, Move> transpositionTablePV;
        private bool a;
        bool end;
        Move bestMove;
        public Game game;
        public Engine()
        {
            var position = new Position();
            game = (Game)GameFactory.Create(position);
            game.NewGame();
            end = false;
            a = true;
            transpositionTableA = new Dictionary<string, int>();
            transpositionTableB = new Dictionary<string, int>();
            transpositionTableM = new Dictionary<string, MoveList>();
            transpositionTablePV = new Dictionary<string, Move>();
        }
        public String PerformBestMove(int timeLimit, bool white) //cplay 1000 w or cplay 1500 b
        {
            if (game.Occupied.Count < 8)
            {
                String mv = EndGameTableBase();
                PerformMove(mv);
                return mv;
            }
            Move move = CalculateBestMove(timeLimit, white);
            if (move.IsCastlelingMove())
            {
                game.Position.SetCastle(white);
            }
            //Console.WriteLine("Move Performed: " + move);
            game.MakeMove(move);
            Console.WriteLine(Eval.end);
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
                if (m.ToString().Substring(0,4).Equals(mv.Substring(0,4)))
                {
                    if (m.IsCastlelingMove())
                    {
                        game.Position.SetCastle(game.State.SideToMove.IsWhite());
                    }
                    if(m.ToString().Length == 5)
                    {
                        if(m.ToString()[4] != 'q')
                        {
                            continue;
                        }
                    }
                    game.MakeMove(m);
                    return true;
                }
            }
            return false;
        }
        public String GetBestMove(int timeLimit, bool white) //suggest 1000 w or suggest 5000 b
        {
            //game.Position.SetCastle();
            Move move = CalculateBestMove(timeLimit, white);
            //Console.WriteLine("Move Evaluated: " + move);
            end = false;
            timer.Stop();
            return move.ToString();
        }
        private void UpdateTranspositionTable(string key, int eval)
        {
            if (a && !transpositionTableA.ContainsKey(key))
            {
                transpositionTableA.Add(key, eval);
            }
            else if (!a && !transpositionTableB.ContainsKey(key))
            {
                transpositionTableB.Add(key, eval);
            }
        }
        private Move CalculateBestMove(int timeLimit, bool white)
        {
            setTimer(timeLimit);
            Move oldMove = new Move();
            int maxDepth = 1;
            int score = 0;
            int newScore = 0;
            String fen = game.GetFen().Fen;
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            while (!end)
            {
                if (white)
                {
                    score = AlphaBetaMax(maxDepth, maxDepth, alpha, beta);
                }
                else
                {
                    score = AlphaBetaMin(maxDepth, maxDepth, alpha, beta);
                }
                oldMove = bestMove;
                newScore = score;
                Console.WriteLine(oldMove + " " + score + " " + maxDepth);
                maxDepth++;
                game.SetFen(fen);
                if (newScore == int.MaxValue || newScore == int.MinValue)
                {
                    break;
                }
            }
            if (a)
            {
                transpositionTableB = new Dictionary<string, int>();
            }
            else
            {
                transpositionTableA = new Dictionary<string, int>();
            }
            transpositionTableM = new Dictionary<string, MoveList>();
            transpositionTablePV = new Dictionary<string, Move>();
            a = !a;
            Console.WriteLine("Max depth: " + (maxDepth - 1));
            Console.WriteLine("Evaluation: " + newScore);
            Console.WriteLine("TTA Count: " + transpositionTableA.Count);
            Console.WriteLine("TTB Count: " + transpositionTableB.Count);
            return oldMove;
        }
        private int AlphaBetaMax(int currentDepth, int initialDepth, int alpha, int beta)
        {
            if (game.Position.IsMate())
            {
                return int.MinValue;
            }
            String key = $"0x{game.State.Key:X}";
            Move nodeBestMove = new Move();
            int nodeBestScore = int.MinValue;
            if (currentDepth == 0)
            {
                if (a)
                {
                    if (transpositionTableB.ContainsKey(key))
                    {
                        int value = transpositionTableB[key];
                        if (!transpositionTableA.ContainsKey(key))
                        {
                            transpositionTableA.Add(key, value);
                        }
                        return value;
                    }
                    else if (transpositionTableA.ContainsKey(key))
                    {
                        return transpositionTableA[key];
                    }
                    int eval = QuiescenceMax(alpha, beta, 0);
                    transpositionTableA.Add(key, eval);
                    return eval;
                }
                else
                {
                    if (transpositionTableA.ContainsKey(key))
                    {
                        int value = transpositionTableA[key];
                        if (!transpositionTableB.ContainsKey(key))
                        {
                            transpositionTableB.Add(key, value);
                        }
                        return value;
                    }
                    else if (transpositionTableB.ContainsKey(key))
                    {
                        return transpositionTableB[key];
                    }
                    int eval = QuiescenceMax(alpha, beta, 0);
                    transpositionTableB.Add(key, eval);
                    return eval;
                }
            }
            int score = 0;
            MoveList moveList;
            Move PVMove = new Move();
            if (transpositionTableM.ContainsKey(key))
            {
                moveList = transpositionTableM[key];
                if (transpositionTablePV.ContainsKey(key))
                {
                    PVMove = transpositionTablePV[key];
                    //
                    game.MakeMove(PVMove);
                    score = AlphaBetaMin(currentDepth - 1, initialDepth, alpha, beta);
                    nodeBestScore = score;
                    nodeBestMove = PVMove;
                    if (score >= beta)
                    {
                        if (currentDepth == initialDepth)
                        {
                            bestMove = PVMove;
                        }
                        game.TakeMove();
                        if (score == int.MaxValue)
                        {
                            return score;
                        }
                        return beta;
                    }
                    if (score > alpha)
                    {
                        if (currentDepth == initialDepth)
                        {
                            bestMove = PVMove;
                        }
                        alpha = score;
                    }
                    game.TakeMove();
                    //
                }
            }
            else
            {
                moveList = new MoveGenerator(game.Position).Moves;
                transpositionTableM.Add(key, moveList);
            }
            foreach (Move m in moveList)
            {
                /*
                if (end)
                {
                    break;
                }
                */
                if (m.Equals(PVMove))
                {
                    continue;
                }
                game.MakeMove(m);
                score = AlphaBetaMin(currentDepth - 1, initialDepth, alpha, beta);
                if (score > nodeBestScore)
                {
                    nodeBestScore = score;
                    nodeBestMove = m;
                }
                if (score >= beta)
                {
                    if (currentDepth == initialDepth)
                    {
                        bestMove = m;
                    }
                    game.TakeMove();
                    if (transpositionTablePV.ContainsKey(key))
                    {
                        transpositionTablePV[key] = nodeBestMove;
                    }
                    else
                    {
                        transpositionTablePV.Add(key, nodeBestMove);
                    }
                    if (score == int.MaxValue)
                    {
                        return score;
                    }
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
            if (transpositionTablePV.ContainsKey(key))
            {
                transpositionTablePV[key] = nodeBestMove;
            }
            else
            {
                transpositionTablePV.Add(key, nodeBestMove);
            }
            return alpha;
        }
        private int AlphaBetaMin(int currentDepth, int initialDepth, int alpha, int beta)
        {
            if (game.Position.IsMate())
            {
                return int.MaxValue;
            }
            String key = $"0x{game.State.Key:X}";
            Move nodeBestMove = new Move();
            int nodeBestScore = int.MaxValue;
            if (currentDepth == 0)
            {
                if (a)
                {
                    if (transpositionTableB.ContainsKey(key))
                    {
                        int value = transpositionTableB[key];
                        if (!transpositionTableA.ContainsKey(key))
                        {
                            transpositionTableA.Add(key, value);
                        }
                        return value;
                    }
                    else if (transpositionTableA.ContainsKey(key))
                    {
                        return transpositionTableA[key];
                    }
                    int eval = QuiescenceMin(alpha, beta, 0);
                    transpositionTableA.Add(key, eval);
                    return eval;
                }
                else
                {
                    if (transpositionTableA.ContainsKey(key))
                    {
                        int value = transpositionTableA[key];
                        if (!transpositionTableB.ContainsKey(key))
                        {
                            transpositionTableB.Add(key, value);
                        }
                        return value;
                    }
                    else if (transpositionTableB.ContainsKey(key))
                    {
                        return transpositionTableB[key];
                    }
                    int eval = QuiescenceMin(alpha, beta, 0);
                    transpositionTableB.Add(key, eval);
                    return eval;
                }
            }
            int score = 0;
            MoveList moveList;
            Move PVMove = new Move();
            if (transpositionTableM.ContainsKey(key))
            {
                moveList = transpositionTableM[key];
                if (transpositionTablePV.ContainsKey(key))
                {
                    PVMove = transpositionTablePV[key];
                    //
                    game.MakeMove(PVMove);
                    score = AlphaBetaMax(currentDepth - 1, initialDepth, alpha, beta);
                    nodeBestMove = PVMove;
                    nodeBestScore = score;
                    if (score <= alpha)
                    {
                        if (currentDepth == initialDepth)
                        {
                            bestMove = PVMove;
                        }
                        game.TakeMove();
                        if (score == int.MinValue)
                        {
                            return score;
                        }
                        return alpha;
                    }
                    if (score < beta)
                    {
                        if (currentDepth == initialDepth)
                        {
                            bestMove = PVMove;
                        }
                        beta = score;
                    }
                    game.TakeMove();
                    //
                }
            }
            else
            {
                moveList = new MoveGenerator(game.Position).Moves;
                transpositionTableM.Add(key, moveList);
            }
            foreach (Move m in moveList)
            {
                /*
                if (end)
                {
                    break;
                }
                */
                if (m.Equals(PVMove))
                {
                    continue;
                }
                game.MakeMove(m);
                score = AlphaBetaMax(currentDepth - 1, initialDepth, alpha, beta);
                if (score < nodeBestScore)
                {
                    nodeBestScore = score;
                    nodeBestMove = m;
                }
                if (score <= alpha)
                {
                    if (currentDepth == initialDepth)
                    {
                        bestMove = m;
                    }
                    game.TakeMove();
                    if (transpositionTablePV.ContainsKey(key))
                    {
                        transpositionTablePV[key] = nodeBestMove;
                    }
                    else
                    {
                        transpositionTablePV.Add(key, nodeBestMove);
                    }
                    if (score == int.MinValue)
                    {
                        return score;
                    }
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
            if (transpositionTablePV.ContainsKey(key))
            {
                transpositionTablePV[key] = nodeBestMove;
            }
            else
            {
                transpositionTablePV.Add(key, nodeBestMove);
            }
            return beta;
        }
        private int QuiescenceMax(int alpha, int beta, int extraDepth)
        {
            int standPat = Eval.MainEvaluation(game.Position.BoardLayout, game, 1);
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
            if (extraDepth <= 4)
            {
                moveList.Concat(game.Position.GenerateMoves(Emgf.QuietChecks));
            }
            foreach (Move m in moveList)
            {
                /*
                if (end)
                {
                    break;
                }
                */
                game.MakeMove(m);
                score = QuiescenceMin(alpha, beta, extraDepth + 1);
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
        private int QuiescenceMin(int alpha, int beta, int extraDepth)
        {
            int standPat = Eval.MainEvaluation(game.Position.BoardLayout, game, -1);
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
            if (extraDepth <= 4)
            {
                moveList.Concat(game.Position.GenerateMoves(Emgf.QuietChecks));
            }
            foreach (Move m in moveList)
            {
                /*
                if (end)
                {
                    break;
                }
                */
                game.MakeMove(m);
                score = QuiescenceMax(alpha, beta, extraDepth + 1);
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
        //very basic
        public string EndGameTableBase()
        {
            string fen = ParseFen();
            var html = @"https://syzygy-tables.info/?fen=";
            html += fen;
            Console.WriteLine(html);
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);
            var node = htmlDoc.DocumentNode.SelectSingleNode("//body/div[@class='right-side']/div/section");

            //Console.WriteLine("Node Name: " + node.Name + "\n" + node.OuterHtml);

            string moves = node.OuterHtml;
            string search = "data-uci=\"";

            moves = moves.Substring(moves.IndexOf(search) + search.Length);
            //Console.WriteLine(moves);

            string move = moves.Substring(0, 5);
            if (move[4] == '"')
            {
                move = move.Substring(0, 4);
            }
            //Console.WriteLine(move);

            return move;
        }
        public string ParseFen()
        {
            string fen = game.GetFen().ToString();
            fen = fen.Replace(' ', '_');
            int underscore = fen.IndexOf('_') + 3;
            String last;
            int length = fen.Length;
            int counter = 1;
            int numUnderscores = 0;
            while (numUnderscores < 2)
            {
                if (fen[length - counter] == '_')
                {
                    numUnderscores++;
                }
                counter++;
            }
            if (fen[length - counter] == '-')
            {
                last = "_-_0_1";
            }
            else
            {
                last = fen.Substring(length - counter - 2, 3);
                last += "_0_1";
            }
            String newFen = fen.Substring(0, underscore);
            newFen = newFen + "-" + last;
            return newFen;
        }
    }
}
