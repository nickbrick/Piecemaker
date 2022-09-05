using ChessDotCore;
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
        public MoveType Type { get; set; }
        public Player Player { get; set; }
        public MoveEventArgs(ChessGame game)
        {
            var move = game.Moves.Last();
            NewPositionFen = game.GetFen();
            NewPositionSimpleFen = game.GetFen().Split()[0];
            From = move.OriginalPosition.ToString();
            To = move.NewPosition.ToString();
            Player = move.Player;

            MoveType moveType = MoveType.Move;
            if (game.IsInCheck(~Player)) moveType |= MoveType.Check;
            if (move.IsSummon) moveType |= MoveType.Summon;
            if (move.IsCapture) moveType |= MoveType.Capture;
            Type = moveType;
        }
    }
    public class ActionEventArgs : EventArgs
    {
        public Player Actor { get; set; }
        public ActionState State { get; set; }
        public ActionEventArgs(ActionState state, Player actor)
        {
            State = state;
            Actor = actor;
        }
        public enum ActionState
        {
            Initiated,
            Cancelled,
            Completed
        }
    }
}
