/*
ChessLib, a chess data structure library

MIT License

Copyright (c) 2017-2019 Rudy Alex Kohn

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace Rudz.Chess
{
    using Enums;
    using Fen;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Types;

    public interface IGame : IEnumerable<Piece>
    {
        State State { get; }
        Action<Piece, Square> PieceUpdated { get; }
        int PositionIndex { get; }
        int PositionStart { get; }
        int MoveNumber { get; }
        BitBoard Occupied { get; }
        IPosition Position { get; }
        EGameEndType GameEndType { get; set; }

        /// <summary>
        /// Makes a chess move in the data structure
        /// </summary>
        /// <param name="move">The move to make</param>
        /// <returns>true if everything was fine, false if unable to progress - fx castleling position under attack</returns>
        bool MakeMove(Move move);

        void TakeMove();

        FenError NewGame(string fen = Fen.Fen.StartPositionFen);

        FenError SetFen(FenData fenData);

        /// <summary>
        /// Apply a FEN string board setup to the board structure.
        /// </summary>
        /// <param name="fenString">The string to set</param>
        /// <param name="validate">If true, the fen string is validated, otherwise not</param>
        /// <returns>
        /// 0 = all ok.
        /// -1 = Error in piece file layout parsing
        /// -2 = Error in piece rank layout parsing
        /// -3 = Unknown piece detected
        /// -4 = Error while parsing moving side
        /// -5 = Error while parsing castleling
        /// -6 = Error while parsing en-passant square
        /// -9 = FEN length exceeding maximum
        /// </returns>
        FenError SetFen(string fenString, bool validate = false);

        FenData GetFen();

        /// <summary>
        /// Converts a move data type to move notation string format which chess engines understand.
        /// e.g. "a2a4", "a7a8q"
        /// </summary>
        /// <param name="move">The move to convert</param>
        /// <param name="output">The string builder used to generate the string with</param>
        void MoveToString(Move move, StringBuilder output);

        void UpdateDrawTypes();

        string ToString();

        BitBoard OccupiedBySide(Player side);

        Player CurrentPlayer();

        ulong Perft(int depth);
    }
}