using compiler;

namespace simulator.handlers
{
    public class SwitchedTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            ctx.Previous(ref ctx.Pos);
            ctx.ReadUntil(ref ctx.Pos, ctx.PlayerToken);
            ctx.Pos++;
            var newActiveName = ctx.ReadUntilValid(ref ctx.Pos);
            ctx.ReadUntil(ref ctx.Pos, ctx.PlayerToken);
            ctx.Pos++;
            var switchedName = ctx.ReadUntilValid(ref ctx.Pos);
                                
            ctx.Pos += 5;

            var switched = ctx.Player!.active.GetMatches(switchedName)[0];
            var newActive = ctx.Player!.bench.GetMatches(newActiveName)[0];

            ctx.Player!.Move(switched, CardHolder.Type.ACTIVE, CardHolder.Type.BENCH);
            ctx.Player!.Move(newActive, CardHolder.Type.BENCH, CardHolder.Type.ACTIVE);
            ctx.Player!.board.ShowPlayed(switchedName + " was switched with " + newActiveName);
            return $"{ctx.PlayerToken}'s {switchedName} was switched with {newActive}";
        }
    }
}