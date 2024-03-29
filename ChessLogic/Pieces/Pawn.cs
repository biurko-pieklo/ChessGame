﻿namespace ChessLogic
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;
        public override Player Color { get; }

        private readonly Direction forward;

        public Pawn(Player color) {
            Color = color;

            if (color == Player.White)
            {
                forward = Direction.North;
            }
            else
            {
                forward = Direction.South;
            }
        }

        public override Piece Copy()
        {
            Pawn copy = new Pawn(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private static bool CanMoveTo(Position position, Board board)
        {
            return Board.IsInside(position) && board.IsEmpty(position);
        }

        private bool CanCaptureAt(Position position, Board board)
        {
            if (!Board.IsInside(position) || board.IsEmpty(position)) {
                return false;
            }

            return board[position].Color != Color;
        }

        private static IEnumerable<Move> PromotionMoves(Position fromPosition, Position to)
        {
            yield return new PawnPromotion(fromPosition, to, PieceType.Knight);
            yield return new PawnPromotion(fromPosition, to, PieceType.Bishop);
            yield return new PawnPromotion(fromPosition, to, PieceType.Rook);
            yield return new PawnPromotion(fromPosition, to, PieceType.Queen);
        }

        private IEnumerable<Move> ForwardMoves(Position fromPosition, Board board)
        {
            Position oneMovePosition = fromPosition + forward;

            if (CanMoveTo(oneMovePosition, board))
            {
                if (oneMovePosition.Row == 0 || oneMovePosition.Row == 7)
                {
                    foreach (Move promMove in PromotionMoves(fromPosition, oneMovePosition))
                    {
                        yield return promMove;
                    }
                }
                else
                {
                    yield return new NormalMove(fromPosition, oneMovePosition);
                }

                Position twoMovesPosition = oneMovePosition + forward;

                if (!HasMoved && CanMoveTo(twoMovesPosition, board))
                {
                    yield return new DoublePawn(fromPosition, twoMovesPosition);
                }
            }
        }

        private IEnumerable<Move> DiagonalMoves(Position fromPosition, Board board)
        {
            foreach (Direction direction in new Direction[] { Direction.East, Direction.West })
            {
                Position to = fromPosition + direction + forward;

                if (to == board.GetPawnSkipPosition(Color.Opponent()))
                {
                    yield return new EnPassant(fromPosition, to);
                }
                else if (CanCaptureAt(to, board))
                {
                    if (to.Row == 0 || to.Row == 7)
                    {
                        foreach (Move promMove in PromotionMoves(fromPosition, to))
                        {
                            yield return promMove;
                        }
                    }
                    else
                    {
                        yield return new NormalMove(fromPosition, to);
                    }
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            return ForwardMoves(fromPosition, board).Concat(DiagonalMoves(fromPosition, board));
        }

        public override bool CanCaptureOpponentKing(Position fromPosition, Board board)
        {
            return DiagonalMoves(fromPosition, board).Any(move =>
            {
                Piece piece = board[move.ToPosition];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
