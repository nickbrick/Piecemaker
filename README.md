# Piecemaker <img src="Piecemaker.Blazor/wwwroot/favicon.ico" width="32">
## A summoner chess variant made in .NET Core & Blazor.
Piecemaker is a chess variant where your king uses magical energy to create his soldiers. This is a simple Blazor Server app made to showcase the variant to, and welcome feedback from, a wider chess audience.

Uses [ChessDotCore](https://github.com/NoNamePro0/Chess.Core) for the chess mechanics, extended to accommodate the new rules.

Frontend is based around [chessboard.js](https://github.com/oakmac/chessboardjs/).
# Rules
- Start with 2 Mana.
- After moving a piece that's already on the board, gain 2 Mana.
- Instead of moving an existing piece, you may Summon a new piece on an empty square, 8-adjacent to your king, if you have enough Mana.
- Base Mana costs for each piece are ♕:9 ♖:5 ♗:3 ♘:3 ♙:1
- After Summoning a piece, its Mana cost increases by its base Mana cost. For example, the first bishop Summoned costs 3 Mana, the second 6, the third 9 and so on.
- When capturing a piece, you gain Mana equal to its base cost (plus 2 for moving your piece).
- Capturing by Summoning is considered illegal.
- Checking the enemy king is illegal, unless it is also a mate, in which case the game is won.
- The starting position includes only the two kings.
- Pawn double advance is illegal (thus en passant does not apply). Pawn promotion is illegal. Pawns may exist on any rank.
- Castling does not apply.
- Threefold repetition and 50 move rule do not apply.
# Try it
https://piece-maker.herokuapp.com/