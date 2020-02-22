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

        public static bool end = false;

        public static int[] KING = new int[]
        {
             20,  50,  30,   0,   0,  10,  50,  20 ,
             20,  20,   0,   0,   0,   0,  20,  20 ,
             -10, -20, -20, -20, -20, -20, -20, -10 ,
             -10, -20, -20, -20, -20, -20, -20, -10 ,
             -20, -30, -30, -40, -40, -30, -30, -20 ,
             -30, -40, -40, -50, -50, -40, -40, -30 ,
             -30, -40, -40, -50, -50, -40, -40, -30 ,
             -30, -40, -40, -50, -50, -40, -40, -30 ,
             -30, -40, -40, -50, -50, -40, -40, -30
        };

        public static int[] KING_END = new int[]
        {
            -20, -10, -10, -5, -5, -10, -10, -20 ,
            -10,   0,   0,  0,  0,   0,   0, -10 ,
            -10,   0,   5,  5,  5,   5,   0, -10 ,
             0,   0,   5,  5,  5,   5,   0,  -5 ,
            -5,   0,   5,  5,  5,   5,   0,  -5 ,
            -10,   0,   5,  5,  5,   5,   0, -10 ,
            -10,   0,   0,  0,  0,   0,   0, -10 ,
            -20, -10, -10, -5, -5, -10, -10, -20
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
            0,   0,  0,  10, 10, 5,  0,  0 ,
            -5,  0,  0,  0,  0,  0,  0, -5 ,
            -5,  0,  0,  0,  0,  0,  0, -5 ,
            -5,  0,  0,  0,  0,  0,  0, -5 ,
            -5,  0,  0,  0,  0,  0,  0, -5 ,
            -5,  0,  0,  0,  0,  0,  0, -5 ,
            5,  15, 15, 15, 15, 15, 15,  5 ,
            0,   0,  0,  0,  0,  0,  0,  0
        };

        public static int[] KNIGHT = new int[]
        {
            -50, -10, -30, -30, -30, -30, -10, -50 ,
            -40, -20,   0,   5,   5,   0, -20, -40 ,
            -30,   5,  10,  10,  10,  10,   5, -30 ,
            -30,   0,  10,  15,  15,  10,   0, -30 ,
            -30,   5,  10,  10,  10,  10,   5, -30 ,
            -30,   0,   5,   5,   5,   5,   0, -30 ,
            -40, -20,   0,   0,   0,   0, -20, -40 ,
            -50, -40, -30, -30, -30, -30, -40, -50
        };

        //public static int[] knight = new int[]
        //{
        //    -50, -20, -30, -30, -30, -30, -20, -50 ,
        //    -40, -20,   0,   5,   5,   0, -20, -40 ,
        //    -30,   5,  10,  15,  15,  12,   5, -30 ,
        //    -30,   0,  15,  20,  20,  15,   0, -30 ,
        //    -30,   5,  15,  20,  20,  15,   5, -30 ,
        //    -30,   0,  10,  15,  15,  10,   0, -30 ,
        //    -40, -20,   0,   0,   0,   0, -20, -40 ,
        //    -50, -40, -30, -30, -30, -30, -40, -50
        //};


        /* jasen's bishop */
        /*
        public static int[] BISHOP = new int[]
{
            -20, -10, -10, -10, -10, -10, -10, -20 ,
            -10,  12,   0,   5,   5,   0,  14, -10 ,
            -10,  10,  10,  12,  12,  10,  10, -10 ,
            -10,   0,  15,  15,  15,  15,   0, -10 ,
            -10,   5,   5,  10,  10,   5,   5, -10 ,
            -10,   0,   5,  10,  10,   5,   0, -10 ,
            -10,   0,   0,   0,   0,   0,   0, -10 ,
            -20, -10, -10, -10, -10, -10, -10, -20
        };
        */

        public static int[] BISHOP = new int[]
        {
            -20, -10, -10, -10, -10, -10, -10, -20 ,
            -10,  12,   0,   5,   5,   0,  12, -10 ,
            -10,  10,  10,  12,  12,  10,  10, -10 ,
            -10,   0,  15,  10,  10,  15,   0, -10 ,
            -10,   5,   5,  10,  10,   5,   5, -10 ,
            -10,   0,   5,  10,  10,   5,   0, -10 ,
            -10,   0,   0,   0,   0,   0,   0, -10 ,
            -20, -10, -10, -10, -10, -10, -10, -20
        };
        /*
         * public static int[] BISHOP = new int[]
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
        */
        //jacob's pawn
        public static int[] PAWN = new int[]
        {
            0,  0,   0,   0,   0,   0,   0,  0 ,
            0,  25,  5,  -20, -20,  25,  25, 0 ,
            8,  10,  10,  30,  30,  10,  10, 8 ,
            10, 10,  20,  40,  40,  20,  10, 10 ,
            15, 20,  40,  50,  50,  35,  20, 15 ,
            40, 40,  40,  60,  60,  40,  40, 40 ,
            80, 80,  80,  80,  80,  80,  80, 80 ,
            0,  0,   0,   0,   0,   0,   0,  0
        };

        //        /* jasen's pawn */
        //        public static int[] PAWN = new int[]
        //{
        //            0,  0,   0,   0,   0,   0,  0,  0 ,
        //            5,  5,   5, -20, -20,  5,  5,  5 ,
        //            5,  5,  10,  10,  10, -10,  6,  5 ,
        //            5,  0,  17,  20,  20,  12,  0,  5 ,
        //            5,  5,  10,  25,  25,  10,  5,  5 ,
        //           20, 10,  20,  50,  50,  20, 10, 20 ,
        //           45, 50,  75,  75,  75,  75, 50, 45 ,
        //            0,  0,   0,   0,   0,   0,  0,  0
        //};
        /* original
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
        */
        public static int PieceValueMG(Piece[] boardP, bool end)
        {
            int score = 0;
            for (int i = 0; i < 64; i++)
            {
                Piece piece = boardP[i];
                if (!piece.IsNoPiece())
                {
                    switch(piece.GetPieceChar())
                    {
                        case 'p':
                            score += -100;
                            score += -PAWN[(63 - i) / 8 * 8 + i % 8];
                            break;
                        case 'P':
                            score += 100;
                            score += PAWN[i];
                            break;
                        case 'r':
                            score += -500;
                            score += -ROOK[(63 - i) / 8 * 8 + i % 8];
                            break;
                        case 'R':
                            score += 500;
                            score += ROOK[i];
                            break;
                        case 'n':
                            score += -300;
                            score += -KNIGHT[(63 - i) / 8 * 8 + i % 8];
                            break;
                        case 'N':
                            score += 300;
                            score += KNIGHT[i];
                            break;
                        case 'b':
                            score += -320;
                            score += -BISHOP[(63 - i) / 8 * 8 + i % 8];
                            break;
                        case 'B':
                            score += 320;
                            score += BISHOP[i];
                            break;
                        case 'k':
                            //score += -28000;
                            if(end)
                            {
                                score += -KING_END[(63 - i) / 8 * 8 + i % 8];
                            }
                            else
                            {
                                score += -KING[(63 - i) / 8 * 8 + i % 8];
                            }
                            break;
                        case 'K':
                            //score += 28000;
                            if (end)
                            {
                                score += KING_END[i];
                            }
                            else
                            {
                                score += KING[i];
                            }
                            break;
                        case 'q':
                            score += -900;
                            score += -QUEEN[(63 - i) / 8 * 8 + i % 8];
                            break;
                        case 'Q':
                            score += 900;
                            score += QUEEN[i];
                            break;
                    }
                }
            }
            return score;
        }

        public static int MainEvaluation(Piece[] pieces, Game engine, int type)//mg = 0, eg = 1 at 53.33 ply moves or 26.66 full moves
        {
            int score = PieceValueMG(pieces, end);
            var moveList = engine.Position.GenerateMoves(Emgf.Quiet);
            score += (int)(moveList.Count * (Math.PI - Math.E) * type);
            return score;
        }
        public static void SetEnd()
        {
            end = true;
        }
    }
}
