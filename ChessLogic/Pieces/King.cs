namespace ChessLogic
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override Player Color { get; }

        public static Direction[] directions = new Direction[]
        {
            Direction.East,
            Direction.North,
            Direction.South,
            Direction.West,
            Direction.NorthEast,
            Direction.SouthEast,
            Direction.NorthWest,
            Direction.SouthWest
        };

        public King(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private IEnumerable<Position> MovePositions(Position fromPosition, Board board)
        {
            foreach (Direction dir in directions)
            {
                Position to = fromPosition + dir;

                if (!Board.IsInside(to))
                {
                    continue;
                }

                if (board.IsEmpty(to) || board[to].Color != Color)
                {
                    yield return to;
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            foreach (Position to in MovePositions(fromPosition, board))
            {
                yield return new NormalMove(fromPosition, to);
            }
        }
    }
}
