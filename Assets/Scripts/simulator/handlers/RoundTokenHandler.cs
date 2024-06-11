using compiler;

namespace simulator.handlers
{
    public class RoundTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            var number = ctx.GetNumber(ctx.Sequence[1]);
            ctx.GetBoard(ctx.Simulator.bluePlayer).board.RoundInfoText.SetText($"Turn #{number}");
            return $"Round #{number}";
        }
    }
}