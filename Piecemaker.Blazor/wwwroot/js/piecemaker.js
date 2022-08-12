var board;
var player;
var whoseMove;
var dotNetObjRef;
function registerGameComponentObject(dotNetObjRef_) {
    dotNetObjRef = dotNetObjRef_;
}
function init(fen, player_) {
    player = player_;
    board = Chessboard('myBoard', {
        draggable: true,
        sparePieces: true,
        position: fen,
        onDrop: onDrop,
        onDragStart: onDragStart,
        orientation: player == 'b' ? 'black' : 'white'
    });
}
function onDragStart(source, piece, position, orientation) {
    console.log(source, piece, position, orientation);
    let from = translateSource(source, piece);
    console.log(from);
    return (player == whoseMove && validMoves.filter(x => x.includes(`${from}-`)).length > 0);
}
function onDrop(source, target, piece) {
    let from = translateSource(source, piece);
    let move = `${from} ${target}`;
    console.log(move);
    if (target != 'offboard') {
        console.log(validMoves.filter(x => x.includes(`${from}-${target}`)).length > 0);
        if (validMoves.filter(x => x.includes(`${from}-${target}`)).length > 0)
            dotNetObjRef.invokeMethodAsync('MakeMove', move);
        else return 'snapback';
    }
}
function translateSource(source, piece) {
    let from = source;
    if (source == 'spare') {
        from = piece[1]
        if (piece[0] == 'b')
            from = from.toLowerCase();
    }
    return from;
}
function setPosition(fen) {
    board.position(fen);
    return; // idk a task was cancelled
}
function setValidMoves(moves, player) {
    //console.log(moves);
    //console.log(player);
    whoseMove = player;
    validMoves = moves;
}
