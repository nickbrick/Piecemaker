﻿using ChessDotCore.Pieces;

namespace ChessDotCore.Variants.Antichess.Pieces
{
    public class AntichessKing : King
    {
        public AntichessKing(Player owner) : base(owner, false) { }

        public override Piece AsPromotion()
        {
            var copy = new AntichessKing(Owner);
            copy.IsPromotionResult = true;
            return copy;
        }

        public override Piece GetWithInvertedOwner()
        {
            return new AntichessKing(ChessUtilities.GetOpponentOf(Owner));
        }
    }
}
