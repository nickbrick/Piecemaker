using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Piecemaker.Engine
{
    public class MoveEventArgs : EventArgs
    {
        public string NewPositionFen { get; set; }
        public string NewPositionSimpleFen { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public MoveEventArgs(ChessDotCore.ChessGame game)
        {
            NewPositionFen = game.GetFen();
            NewPositionSimpleFen = game.GetFen().Split()[0];
            From = game.Moves.Last().OriginalPosition.ToString();
            To = game.Moves.Last().NewPosition.ToString();
        }
    }
}
