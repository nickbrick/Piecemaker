using System;
using System.Collections.Generic;
using System.Text;

namespace ChessDotCore
{
	public partial class ChessGame
	{
		public const int StartingMana = 2;
		public const int ManaGain = 2;
		public const string StartingFen = "4k3/8/8/8/8/8/4q3/4K3 w - - 0 0";
		public static PieceCosts InitialPieceCosts = new PieceCosts
		{
			Queen = 9,
			Rook = 5,
			Bishop = 3,
			Knight = 3,
			Pawn = 1
		};

		public int WhiteMana { get; protected set; } = StartingMana;
		public int BlackMana { get; protected set; } = StartingMana;
		public PieceCosts WhitePieceCosts = InitialPieceCosts;
		public PieceCosts BlackPieceCosts = InitialPieceCosts;
		public System.Collections.ObjectModel.ReadOnlyCollection<Move> ValidMoves => GetValidMoves(WhoseTurn);
		public int GetMana(Player player)
        {
			return player == Player.White ? WhiteMana : BlackMana;
        }
		public void AddMana(Player player, int mana)
		{
			if (player == Player.White) WhiteMana += mana;
			else BlackMana += mana;
		}
		public int GetCost(Player player, Piece piece)
        {
			PieceCosts costs;
			if (player == Player.White) costs = WhitePieceCosts;
			else if (player == Player.Black) costs = BlackPieceCosts;
			else costs = InitialPieceCosts;

			if (piece is Pieces.Queen)
				return costs.Queen;
			if (piece is Pieces.Rook)
				return costs.Rook;
			if (piece is Pieces.Bishop)
				return costs.Bishop;
			if (piece is Pieces.Knight)
				return costs.Knight;
			if (piece is Pieces.Pawn)
				return costs.Pawn;

			throw new ArgumentException("Invalid piece.");
		}
		public int GetCost<T>(Player player) where T : Piece
		{
			PieceCosts costs;
			if (player == Player.White) costs = WhitePieceCosts;
			else costs = BlackPieceCosts;

			if (typeof(T) == typeof(Pieces.Queen))
				return costs.Queen;
			if (typeof(T) == typeof(Pieces.Rook))
				return costs.Rook;
			if (typeof(T) == typeof(Pieces.Bishop))
				return costs.Bishop;
			if (typeof(T) == typeof(Pieces.Knight))
				return costs.Knight;
			if (typeof(T) == typeof(Pieces.Pawn))
				return costs.Pawn;

			throw new ArgumentException("Invalid piece.");
		}
		public object[] GetAllCosts(Player player)
        {
			PieceCosts costs = player == Player.White ? WhitePieceCosts : BlackPieceCosts;
			return new object[]{ costs.Queen, costs.Rook, costs.Bishop, costs.Knight, costs.Pawn };
        }
		public void AddCost(Player player, Piece piece)
        {
			if (player == Player.White)
            {
				if (piece is Pieces.Queen)
					WhitePieceCosts.Queen+=InitialPieceCosts.Queen;
				if (piece is Pieces.Rook)
					WhitePieceCosts.Rook+=InitialPieceCosts.Rook;
				if (piece is Pieces.Bishop)
					WhitePieceCosts.Bishop+=InitialPieceCosts.Bishop;
				if (piece is Pieces.Knight)
					WhitePieceCosts.Knight+=InitialPieceCosts.Knight;
				if (piece is Pieces.Pawn)
					WhitePieceCosts.Pawn+=InitialPieceCosts.Pawn;
			}
            else
            {
				if (piece is Pieces.Queen)
					BlackPieceCosts.Queen += InitialPieceCosts.Queen;
				if (piece is Pieces.Rook)
					BlackPieceCosts.Rook += InitialPieceCosts.Rook;
				if (piece is Pieces.Bishop)
					BlackPieceCosts.Bishop += InitialPieceCosts.Bishop;
				if (piece is Pieces.Knight)
					BlackPieceCosts.Knight += InitialPieceCosts.Knight;
				if (piece is Pieces.Pawn)
					BlackPieceCosts.Pawn += InitialPieceCosts.Pawn;
			}
        }

		private bool IsPlayersKingAdjacentTo(Player player, Position position)
		{
			for (int file = (int)position.File - 1; file <= (int)position.File + 1; file++)
			{
				for (int rank = position.Rank - 1; rank <= position.Rank + 1; rank++)
				{
					if (file == (int)position.File && rank == position.Rank) continue;
					if (file < 0 || file > 7 || rank < 1 || rank > 8) continue;
					Piece neighbor = GetPieceAt((File)file, rank);
					if (neighbor is Pieces.King && neighbor.Owner == player) return true;
				}
			}
			return false;
		}
		internal bool CanAffordSummon(Piece summon)
		{
			int mana;
			PieceCosts costs;
			if (summon.Owner == Player.White)
			{
				mana = WhiteMana;
				costs = WhitePieceCosts;
			}
			else
			{
				mana = BlackMana;
				costs = BlackPieceCosts;
			}

			if (summon is Pieces.Queen)
				return mana >= costs.Queen;
			if (summon is Pieces.Rook)
				return mana >= costs.Rook;
			if (summon is Pieces.Bishop)
				return mana >= costs.Bishop;
			if (summon is Pieces.Knight)
				return mana >= costs.Knight;
			if (summon is Pieces.Pawn)
				return mana >= costs.Pawn;
			return false;
		}
		private bool IsSummonValid(Move move)
        {
			Position pos = move.NewPosition;
			if (!move.IsSummon) return false;
			Piece summon = move.OriginalPosition.Summon;

			return IsPlayersKingAdjacentTo(move.Player, pos) && CanAffordSummon(summon);
        }
	}
}
