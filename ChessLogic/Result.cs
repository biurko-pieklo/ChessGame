namespace ChessLogic
{
    public class Result
    {
        public Player Winner { get; }
        public EndReason EndReason { get; }

        public Result(Player winner, EndReason reason)
        {
            Winner = winner;
            EndReason = reason;
        }

        public static Result Win(Player winner)
        {
            return new Result(winner, EndReason.Checkmate);
        }

        public static Result Draw(EndReason reason)
        {
            return new Result(Player.None, reason);
        }
    }
}
