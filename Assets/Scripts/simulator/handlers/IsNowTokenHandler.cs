using compiler;
using UnityEngine;

namespace simulator.handlers
{
    public class IsNowTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            if (ctx.PreviousAction == TokenType.Switched) return "";
            Debug.Log(ctx.PreviousAction);
            ctx.Previous(ref ctx.Pos);
            ctx.ReadUntil(ref ctx.Pos, ctx.PlayerToken);
            ctx.Pos++;
            var newActive = ctx.ReadUntilValid(ref ctx.Pos);

            var place = ctx.GetLocation(ctx.Next(ref ctx.Pos, TokenType.Location).Value);
            var match = ctx.Player!.bench.GetMatches(newActive)[0];
            ctx.Player!.Move(match, CardHolder.Type.BENCH, place);

            return $"{ctx.PlayerToken}'s {newActive} was switched to {place} with {match.Name}";
        }
    }
}