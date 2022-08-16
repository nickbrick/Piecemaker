var board;
var player;
var whoseMove;
var dotNetObjRef;
function registerGameComponentObject(dotNetObjRef_) {
    dotNetObjRef = dotNetObjRef_;
}
function init(fen, player_, lastMove) {
    player = player_;
    board = Chessboard('myBoard', {
        draggable: true,
        sparePieces: true,
        position: fen,
        onDrop: onDrop,
        onDragStart: onDragStart,
        orientation: player == 'b' ? 'black' : 'white'
    });
    highlightSquares(lastMove.split('-')[0], lastMove.split('-')[1]);
}
function onDragStart(source, piece, position, orientation) {
    let from = translateSource(source, piece);
    let theseMoves = validMoves.filter(x => x.includes(`${from}-`));
    let destinations = theseMoves.map(x => x.split('-')[1]);
    destinations.forEach(x => 
        $('.square-' + x).addClass('destination')
    );
    return (player == whoseMove && theseMoves.length > 0);
}
function onDrop(source, target, piece) {
    $('.square-55d63').removeClass('destination');
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
function setValidMoves(moves, player, summonables) {
    whoseMove = player;
    validMoves = moves;
    $('.spare-pieces-7492f img').addClass('locked');
    summonables.forEach(x => {
        $(`.spare-pieces-7492f img[data-piece="${x}"]`).removeClass('locked');
    });
}
function highlightSquares(from, to) {
    $('.square-55d63').removeClass('highlight');
    $('.square-55d63').removeClass('capture');
    $(`.square-${from}`).addClass('highlight');
    $(`.square-${to}`).addClass('highlight');
    $(`.square-${to}:has(img)`).addClass('capture');
}
function playSound(soundName) {
    $(`#${soundName}`)[0].play();
}