using System;
using System.Collections.Generic;
using System.Linq;
using ChessDotCore;
using ChessDotCore.Variants;
namespace Piecemaker.Cli
{
    class Program
    {
        // Let's start by creating a chess game instance.

        static void Main(string[] args)
        {
            var game = new ChessGame();
            while (game.HasAnyValidMoves(game.WhoseTurn))
            {
                Helpers.PrintBoard(game);
                Console.WriteLine($"{game.WhoseTurn}'s move? (format {{from}} {{to}})");
                MoveType result;
                Move move;
                do
                {
                    string[] input = Console.ReadLine().Split();
                    move = new Move(input[0], input[1], game.WhoseTurn);
                    result = game.MakeMove(move, false);

                } while (result == MoveType.Invalid);
            }
            // Congratulations! You have learned about the most important methods of Chess.Core. Enjoy using the library :)
            Console.ReadKey();
        }
    }

    public static class Helpers
    {
        public static void PrintBoard(ChessGame game)
        {
            var board = game.GetBoard();
            var i = 8;
            Console.Clear();
            Console.Write("  + - - - - - - - - +\n");
            foreach (var rank in board)
            {
                Console.Write(i + " - ");
                foreach (var square in rank)
                {
                    Console.Write((square?.GetFenCharacter().ToString() ?? " ") + " ");
                }
                Console.Write("-\n");
                i--;
            }
            Console.Write("  + - - - - - - - - +\n");
            Console.Write("    a b c d e f g h\n");
        }
    }
}
