using System;
using System.Collections.Generic;
using System.Linq;
using ChessDotCore;
namespace Piecemaker.Engine
{
    public class Table
    {
        public event EventHandler<MoveEventArgs> ValidMoveWasMade;
        public event EventHandler<Client> ClientJoined;
        public event EventHandler<Client> ClientDisconnected;
        public int Id { get; }
        public ChessGame Game { get; set; }
        private List<Client> Clients { get; set; }
        public Client WhiteClient => Clients.SingleOrDefault(client => client.Player == Player.White);
        public Client BlackClient => Clients.SingleOrDefault(client => client.Player == Player.Black);
        public Table(int id)
        {
            Id = id;
            Clients = new List<Client>();
            Game = new ChessGame("4k3/P7/8/8/8/8/p7/4K3 w - - 0 0");
        }
        private void WhiteClient_ValidMoveWasMade(object sender, MoveEventArgs e)
        {
            ValidMoveWasMade?.Invoke(this, e);
        }
        private void BlackClient_ValidMoveWasMade(object sender, MoveEventArgs e)
        {
            ValidMoveWasMade?.Invoke(this, e);
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
            return client;
        }
        public void Disconnect(Client client)
        {
            ClientDisconnected?.Invoke(this, client);
            Clients.Remove(client);
        }
    }
}
