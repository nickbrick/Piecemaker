using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ChessDotCore.Variants.Piecemaker
{
    public class PiecemakerChessGame : ChessGame
    {
        public const int StartingMana = 2;
        public int WhiteMana { get; protected set; } = StartingMana;
        public int BlackMana { get; protected set; } = StartingMana;
        public static PieceCosts InitialPieceCost = new PieceCosts
        {
            Queen = 9,
            Rook = 5,
            Bishop = 3,
            Knight = 3,
            Pawn = 1
        };
        public PieceCosts WhitePieceCost = InitialPieceCost;
        public PieceCosts BlackPieceCost = InitialPieceCost;
        
        public PiecemakerChessGame() : base() { }
        public PiecemakerChessGame(Piece[][] board, Player whoseTurn) : base(board, whoseTurn) { }
        public PiecemakerChessGame(GameCreationData data) : base(data) { }
        public PiecemakerChessGame(string fen) : base(fen) { }
        public PiecemakerChessGame(IEnumerable<Move> moves, bool movesAreValidated) : base(moves, movesAreValidated) { }

        protected override MoveType ApplyMove(Move move, bool alreadyValidated, out Piece captured, out CastlingType castlingType)
        {
            MoveType ret = base.ApplyMove(move, alreadyValidated, out captured, out castlingType);
            if (ret == MoveType.Invalid)
            {
                return ret;
            }

            return ret;
        }

        public override bool Undo()
        {
            throw new NotImplementedException("Undo not implemented yet.");
        }
        public struct PieceCosts
        {
            public int Queen;
            public int Rook;
            public int Bishop;
            public int Knight;
            public int Pawn;
        }
    }
}
