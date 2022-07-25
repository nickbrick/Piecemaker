using System;
using System.Collections.Generic;
using System.Linq;
using ChessDotCore;

namespace Piecemaker.Console
{
    class Program
    {
        // Let's start by creating a chess game instance.

        static void Main(string[] args)
        {
            var game = new ChessGame();
            while (true || game.HasAnyValidMoves(game.WhoseTurn))
            {
                Helpers.PrintBoard(game);
                System.Console.WriteLine($"{game.WhoseTurn}'s move? (format {{from}} {{to}})");
                MoveType result;
                Move move;
                do
                {
                    try
                    {
                        string[] input = System.Console.ReadLine().Split();
                        move = new Move(input[0], input[1], game.WhoseTurn);
                        result = game.MakeMove(move, false);
                    }
                    catch
                    {
                        result = MoveType.Invalid;
                    }
                } while (result == MoveType.Invalid);
            }
            // Congratulations! You have learned about the most important methods of Chess.Core. Enjoy using the library :)
            System.Console.ReadKey();
        }
    }

    public static class Helpers
    {
        public static void PrintBoard(ChessGame game)
        {
            var board = game.GetBoard();
            System.Console.Clear();
            for (int rank = 8; rank > 0; rank--)
            {
                System.Console.Write(" "+rank + " ");
                for (int file = 0; file < 8; file++)
                {
                    Piece square = board[8-rank][file];
                    string blank = rank % 2 == 0 ^ file % 2 == 0 ? " " : "░";
                    System.Console.Write((square?.GetFenCharacter().ToString() ?? blank) + blank);
                }
                if (rank == 8)
                    System.Console.Write(" q{0} r{1} b{2} n{3} p{4}", game.GetAllCosts(Player.Black));
                if (rank == 7)
                    System.Console.Write(" Mana: {0}", game.BlackMana);
                if (rank == 2)
                    System.Console.Write(" Mana: {0}", game.WhiteMana);
                if (rank == 1)
                    System.Console.Write(" Q{0} R{1} B{2} N{3} P{4}", game.GetAllCosts(Player.White));
                System.Console.Write("\n");
            }
            System.Console.Write("   a b c d e f g h\n");
        }
    }
}
