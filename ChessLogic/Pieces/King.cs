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

        private static bool IsUnmovedRook(Position position, Board board)
        {
            if (board.IsEmpty(position))
            {
                return false;
            }

            Piece piece = board[position];

            return piece.Type == PieceType.Rook && !piece.HasMoved;
        }

        private static bool AllEmpty(IEnumerable<Position> positions, Board board)
        {
            return positions.All(pos => board.IsEmpty(pos));
        }

        private bool CanCastleKingSide(Position fromPosition, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            Position rookPos = new Position(fromPosition.Row, 7);
            Position[] betweenPositions = new Position[] { new(fromPosition.Row, 5), new(fromPosition.Row, 6) };

            return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
        }

        private bool CanCastleQueenSide(Position fromPosition, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            Position rookPos = new Position(fromPosition.Row, 0);
            Position[] betweenPositions = new Position[] { new(fromPosition.Row, 1), new(fromPosition.Row, 2), new(fromPosition.Row, 3) };

            return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
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

            if (CanCastleKingSide(fromPosition, board))
            {
                yield return new Castle(MoveType.CastleKS, fromPosition);
            }

            if (CanCastleQueenSide(fromPosition, board))
            {
                yield return new Castle(MoveType.CastleQS, fromPosition);
            }
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
