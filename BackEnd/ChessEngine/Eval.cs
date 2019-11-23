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
    public class Eval
    {
        public static int[] KING = new int[]
        {
             20,  30,  10,   0,   0,  10,  30,  20 ,
             20,  20,   0,   0,   0,   0,  20,  20 ,
             -10, -20, -20, -20, -20, -20, -20, -10 ,
             -10, -20, -20, -20, -20, -20, -20, -10 ,
             -20, -30, -30, -40, -40, -30, -30, -20 ,
             -30, -40, -40, -50, -50, -40, -40, -30 ,
             -30, -40, -40, -50, -50, -40, -40, -30 ,
             -30, -40, -40, -50, -50, -40, -40, -30 ,
             -30, -40, -40, -50, -50, -40, -40, -30 
        };


        public static int[] QUEEN = new int[]
        {
            -20, -10, -10, -5, -5, -10, -10, -20 ,
            -10,   0,   5,  0,  0,   0,   0, -10 ,
            -10,   5,   5,  5,  5,   5,   0, -10 ,
             0,   0,   5,  5,  5,   5,   0,  -5 ,
            -5,   0,   5,  5,  5,   5,   0,  -5 ,
            -10,   0,   5,  5,  5,   5,   0, -10 ,
            -10,   0,   0,  0,  0,   0,   0, -10 ,
            -20, -10, -10, -5, -5, -10, -10, -20
        };

        public static int[] ROOK = new int[]
        {
            0,  0,  0,  5,  5,  0,  0,  0 ,
            -5,  0,  0,  0,  0,  0,  0, -5 ,
            -5,  0,  0,  0,  0,  0,  0, -5 ,
            -5,  0,  0,  0,  0,  0,  0, -5 ,
            -5,  0,  0,  0,  0,  0,  0, -5 ,
            -5,  0,  0,  0,  0,  0,  0, -5 ,
            5, 10, 10, 10, 10, 10, 10,  5 ,
            0,  0,  0,  0,  0,  0,  0,  0
        };
        public static int[] KNIGHT = new int[]
        {
            -50, -40, -30, -30, -30, -30, -40, -50 ,
            -40, -20,   0,   5,   5,   0, -20, -40 ,
            -30,   5,  10,  15,  15,  10,   5, -30 ,
            -30,   0,  15,  20,  20,  15,   0, -30 ,
            -30,   5,  15,  20,  20,  15,   5, -30 ,
            -30,   0,  10,  15,  15,  10,   0, -30 ,
            -40, -20,   0,   0,   0,   0, -20, -40 ,
            -50, -40, -30, -30, -30, -30, -40, -50 
        };

        public static int[] BISHOP = new int[]
        {
            -20, -10, -10, -10, -10, -10, -10, -20 ,
            -10,   5,   0,   0,   0,   0,   5, -10 ,
            -10,  10,  10,  10,  10,  10,  10, -10 ,
            -10,   0,  10,  10,  10,  10,   0, -10 ,
            -10,   5,   5,  10,  10,   5,   5, -10 ,
            -10,   0,   5,  10,  10,   5,   0, -10 ,
            -10,   0,   0,   0,   0,   0,   0, -10 ,
            -20, -10, -10, -10, -10, -10, -10, -20
        };

        public static int[] PAWN = new int[]
        {
            0,  0,   0,   0,   0,   0,  0,  0 ,
            5, 10,  10, -20, -20,  10, 10,  5 ,
            5, -5, -10,   0,   0, -10, -5,  5 ,
            0,  0,   0,  20,  20,   0,  0,  0 ,
            5,  5,  10,  25,  25,  10,  5,  5 ,
            10, 10,  20,  30,  30,  20, 10, 10 ,
            50, 50,  50,  50,  50,  50, 50, 50 ,
            0,  0,   0,   0,   0,   0,  0,  0 
        };
        public static int PieceValueMG(Piece[] boardP)
        {
            int score = 0;
            for (int i = 0; i < 64; i++)
            {
                Piece piece = boardP[i];
                if (!piece.IsNoPiece())
                {
                    if (piece.GetPieceChar() == 'p')
                    {
                        score += -100;
                        score += -PAWN[(63-i)/8*8 + i%8];
                    }
                    else if (piece.GetPieceChar() == 'P')
                    {
                        score += 100;
                        score += PAWN[i];
                    }
                    else if (piece.GetPieceChar() == 'r')
                    {
                        score += -500;
                        score += -ROOK[(63 - i) / 8 * 8 + i % 8];
                    }
                    else if (piece.GetPieceChar() == 'R')
                    {
                        score += 500;
                        score += ROOK[i];
                    }
                    else if (piece.GetPieceChar() == 'n')
                    {
                        score += -300;
                        score += -KNIGHT[(63 - i) / 8 * 8 + i % 8];
                    }
                    else if (piece.GetPieceChar() == 'N')
                    {
                        score += 300;
                        score += KNIGHT[i];
                    }
                    else if (piece.GetPieceChar() == 'b')
                    {
                        score += -310;
                        score += -BISHOP[(63 - i) / 8 * 8 + i % 8];
                    }
                    else if (piece.GetPieceChar() == 'B')
                    {
                        score += 310;
                        score += BISHOP[i];
                    }
                    else if (piece.GetPieceChar() == 'k')
                    {
                        score += -28000;
                        score += -KING[(63 - i) / 8 * 8 + i % 8];
                    }
                    else if (piece.GetPieceChar() == 'K')
                    {
                        score += 28000;
                        score += KING[i];
                    }
                    else if (piece.GetPieceChar() == 'q')
                    {
                        score += -900;
                        score += -QUEEN[(63 - i) / 8 * 8 + i % 8];
                    }
                    else if (piece.GetPieceChar() == 'Q')
                    {
                        score += 900;
                        score += QUEEN[i];
                    }
                }
            }
            return score;
        }

        public static int MainEvaluation(Piece[] pieces)//mg = 0, eg = 1 at 53.33 ply moves or 26.66 full moves
        {
            return PieceValueMG(pieces);
        }
    }
}
