﻿namespace ChessLogic
{
    public class DoublePawn : Move
    {
        public override MoveType Type => MoveType.DoublePawn;
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }

        private readonly Position skippedPosition;

        public DoublePawn(Position fromPosition, Position to)
        {
            FromPosition = fromPosition;
            ToPosition = to;

            skippedPosition = new Position((fromPosition.Row + to.Row) / 2, fromPosition.Column);
        }

        public override void Execute(Board board)
        {
            Player player = board[FromPosition].Color;
            board.SetPawnSkipPosition(player, skippedPosition);

            new NormalMove(FromPosition, ToPosition).Execute(board);
        }
    }
}
