var board;
var player;
var whoseMove;
var dotNetObjRef;
function registerGameComponentObject(dotNetObjRef_) {
    dotNetObjRef = dotNetObjRef_;
}
function init(fen, player_, lastMove, whoseMove_) {
    player = player_[0];
    whoseMove = whoseMove_[0];
    board = Chessboard('chessboard', {
        draggable: true,
        sparePieces: true,
        position: fen,
        onDrop: onDrop,
        onDragStart: onDragStart,
        orientation: player_
    });
    highlightSquares(lastMove.split('-')[0], lastMove.split('-')[1]);
}
function onDragStart(source, piece, position, orientation) {
    let from = translateSource(source, piece);
    let theseMoves = validMoves.filter(x => x.includes(`${from}-`));
    let destinations = theseMoves.map(x => x.split('-')[1]);
    destinations.forEach(x => {
        $(`.square-${x}`).addClass('destination');
        $(`.square-${x}:has(img)`).addClass('capture');
    });
    return (player == whoseMove && theseMoves.length > 0);
}
function onDrop(source, target, piece) {
    $('.square-55d63').removeClass('destination');
    $('.square-55d63').removeClass('capture');
    let from = translateSource(source, piece);
    let move = `${from} ${target}`;
    if (target != 'offboard') {
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
}
function setValidMoves(moves, player, summonables) {
    whoseMove = player[0];
    validMoves = moves;
    $('.spare-pieces-7492f img[data-piece]').addClass('locked');
    $(`.spare-pieces-7492f .piece-cost span`).addClass('locked');

    summonables.forEach(x => {
        $(`.spare-pieces-7492f img[data-piece="${x}"]`).removeClass('locked');
        $(`.spare-pieces-7492f .piece-cost-${x}`).removeClass('locked');
    });
}
function highlightSquares(from, to) {
    $('.square-55d63').removeClass('highlight');
    $(`.square-${from}`).addClass('highlight');
    $(`.square-${to}`).addClass('highlight');
}
function playSound(soundName) {
    $("#" + soundName)[0].play();
}
function updateMana(kvs) {
    Object.keys(kvs).forEach(key => {
        $(key).text(kvs[key]);
    });
}
function setOrientation(player_) {
    board.orientation(player_);
    player = player_[0];
}

function updateTitle(isNotifying) {
    document.title = (isNotifying ? '\u25CF ' : '') + 'Piecemaker' + $('.table-id').text();
}
