namespace ChessLogic
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;
        public override Player Color { get; }

        public Knight(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Knight copy = new Knight(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private static IEnumerable<Position> PotentialToPositions(Position fromPosition)
        {
            foreach (Direction vDir in new Direction[] { Direction.North, Direction.South })
            {
                foreach (Direction hDir in new Direction[] { Direction.East, Direction.West })
                {
                    yield return fromPosition + 2 * vDir + hDir;
                    yield return fromPosition + 2 * hDir + vDir;
                }
            }
        }

        private IEnumerable<Position> MovePositions(Position fromPosition, Board board) {
            return PotentialToPositions(fromPosition).Where(pos => Board.IsInside(pos) && (board.IsEmpty(pos) || board[pos].Color != Color));
        }

        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board) {
            return MovePositions(fromPosition, board).Select(to => new NormalMove(fromPosition, to));
        }

        public override bool CanCaptureOpponentKing(Position fromPosition, Board board)
        {
            return MovePositions(fromPosition, board).Any(to =>
            {
                Piece piece = board[to];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
