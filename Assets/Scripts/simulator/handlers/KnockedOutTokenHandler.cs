using compiler;

namespace simulator.handlers
{
    public class KnockedOutTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            ctx.Previous(ref ctx.Pos);
            ctx.ReadUntil(ref ctx.Pos, ctx.PlayerToken);
            ctx.Pos++;
            var knockedOutName = ctx.ReadUntilValid(ref ctx.Pos);
            var match = ctx.Player!.active.GetMatches(knockedOutName)[0];
            ctx.Player.Move(match, CardHolder.Type.BENCH, CardHolder.Type.DISCARD);
            return $"{ctx.PlayerToken}'s was knocked out";
        }
    }
}