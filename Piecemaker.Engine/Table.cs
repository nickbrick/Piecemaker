using System;
using System.Collections.Generic;
using System.Linq;
using ChessDotCore;
namespace Piecemaker.Engine
{
    public class Table
    {
        public event EventHandler<MoveEventArgs> ValidMoveWasMade;
        public event EventHandler<Player> PlayerWonByCheckmate;
        public event EventHandler<Client> ClientJoined;
        public event EventHandler<Client> ClientDisconnected;
        public event EventHandler SideSwapActionHandled;
        public event EventHandler<Status> StatusChanged;
        public int Id { get; }
        public ChessGame Game { get; set; }
        public Status Status { get; set; }
        public DateTime LastActivityAt { get; private set; }
        private List<Client> Clients { get; set; }
        public int PlayingClientsCount => Clients.Count(client => client.Player != Player.None);
        public Player SideSwapInitiator = Player.None;
        public Client WhiteClient => Clients.SingleOrDefault(client => client.Player == Player.White);
        public Client BlackClient => Clients.SingleOrDefault(client => client.Player == Player.Black);
        public Table(int id)
        {
            Id = id;
            Clients = new List<Client>();
            Game = new ChessGame(ChessGame.StartingFen);
            Status = Status.Empty;
            LastActivityAt = DateTime.UtcNow;
        }
        private void WhiteClient_ValidMoveWasMade(object sender, MoveEventArgs e)
        {
            AnyClient_ValidMoveWasMade(this, e);
        }
        private void BlackClient_ValidMoveWasMade(object sender, MoveEventArgs e)
        {
            AnyClient_ValidMoveWasMade(this, e);
        }
        private void AnyClient_ValidMoveWasMade(object sender, MoveEventArgs e)
        {
            Status = Status.Playing;
            ValidMoveWasMade?.Invoke(this, e);
            if (Game.IsCheckmated(Game.WhoseTurn))
            {
                PlayerWonByCheckmate?.Invoke(this, e.Player);
                Status = Status.Finished;
            }
            LastActivityAt = DateTime.UtcNow;
        }
        public Client Join()
        {
            var client = new Client(this);
            if (!Clients.Any(client => client.Player == Player.White))
            {
                client.Player = Player.White;
                client.ValidMoveWasMade += WhiteClient_ValidMoveWasMade;
            }
            else if (!Clients.Any(client => client.Player == Player.Black))
            {
                client.Player = Player.Black;
                client.ValidMoveWasMade += BlackClient_ValidMoveWasMade;
            }
            else client.Player = Player.None;
            Clients.Add(client);

            switch (Status)
            {
                case Status.Empty:
                    Status = Status.Open;
                    break;
                case Status.Open:
                    Status = Status.Ready;
                    break;
                case Status.Paused:
                    Status = PlayingClientsCount == 2 ? Status.Playing : Status.Paused;
                    break;
            }

            ClientJoined?.Invoke(this, client);
            LastActivityAt = DateTime.UtcNow;
            return client;
        }
        public void Disconnect(Client client)
        {
            Clients.Remove(client);
            if (client.Player != Player.None)
            {
                switch (Status)
                {
                    case Status.Playing:
                        Status = Status.Paused;
                        break;
                    case Status.Ready:
                        Status = Status.Open;
                        break;
                    case Status.Open:
                        Status = Status.Empty;
                        break;
                }
            }
            LastActivityAt = DateTime.UtcNow;
            ClientDisconnected?.Invoke(this, client);
        }
        public void OpenClose()
        {
            Status = Status == Status.Closed ? Status.Open : Status.Closed;
            StatusChanged?.Invoke(this, Status);
        }
        public void HandleSideSwapAction(Player clicker)
        {
            if (PlayingClientsCount == 1)
            {
                Clients.ForEach(client => client.Player = ~client.Player);
            }
            else
            {
                if (SideSwapInitiator == Player.None)
                    SideSwapInitiator = clicker;
                else
                {
                    if (SideSwapInitiator == ~clicker)
                        Clients.ForEach(client => client.Player = ~client.Player);
                    SideSwapInitiator = Player.None;
                }
            }
            SideSwapActionHandled?.Invoke(this, EventArgs.Empty);
        }
    }
    public enum Status
    {
        Empty,          // Should be disposed
        Open,           // One player in starting position, use for finding game
        Closed,         // One player in starting position, need url to join
        Ready,          // Two players in starting position
        Playing,        // Two players and not in starting position, leave it alone
        Paused,         // One player and not in starting position, need url to join
        Finished        // Checkmate happened
    }
}
