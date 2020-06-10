using System;
using System.Collections.Generic;
using System.Text;
using static ChessEngine.Conversion.Pieces;

namespace ChessEngine
{
    public class Legality
    {
        public static int EnpassantSquare(int beforeSquare, int afterSquare, int piece)
        {
            if(piece == (int)WHITE_PAWN || piece == (int)BLACK_PAWN)
            {
                if(Math.Abs(beforeSquare/10-afterSquare/10) == 2)
                {
                    return (beforeSquare / 10 + afterSquare / 10)*5 + beforeSquare % 10;
                }
            }
            return 0;
        }
    }
}
