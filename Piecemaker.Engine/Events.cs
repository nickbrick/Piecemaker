using System;
using System.Collections.Generic;
using System.Text;

namespace Piecemaker.Engine
{
    public class MoveEventArgs : EventArgs
    {
        public string NewPositionFen { get; set; }
        public string NewPositionSimpleFen { get; set; }
        public MoveEventArgs(ChessDotCore.ChessGame game)
        {
            NewPositionFen = game.GetFen();
            NewPositionSimpleFen = game.GetFen().Split()[0];
        }
    }
}
