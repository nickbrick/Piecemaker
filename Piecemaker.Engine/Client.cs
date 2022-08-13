using ChessDotCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Piecemaker.Engine
{
    public class Client
    {
        public event EventHandler<MoveEventArgs> ValidMoveWasMade;
        public Guid Id;
        public Player Player { get; set; }
        private Table Table { get; }
        public List<string> ValidMoves => Table.Game.GetValidMoves(Player).Select(move => move.ToString()).ToList();
        public Client(Table table)
        {
            Id = Guid.NewGuid();
            Table = table;
        }
        public bool MakeMove(string movestring)
        {
            try
            {
                Move move = new Move(movestring.Split()[0], movestring.Split()[1], Player);
                var result = Table.Game.MakeMove(move, alreadyValidated: false);
                System.Diagnostics.Debug.WriteLine("{0} tried move {1}, result was {2}", new string[] { Player.ToString(), movestring, result.ToString() });
                if (result.HasFlag(MoveType.Move))
                    ValidMoveWasMade?.Invoke(this, new MoveEventArgs(Table.Game));
                return result.HasFlag(MoveType.Move);
            }
            catch
            {
                return false;
            }
        }
        public Client GetOpponentClient()
        {
            if (Player == Player.White)
                return Table.BlackClient;
            else if (Player == Player.Black)
                return Table.WhiteClient;
            else return null;
        }
    }
}