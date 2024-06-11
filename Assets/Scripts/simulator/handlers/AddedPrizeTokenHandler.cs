using compiler;
using UnityEngine;

namespace simulator.handlers
{
    public class AddedPrizeTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            ctx.Pos = 0;
            var prizeName = ctx.ReadUntilValid(ref ctx.Pos);
            var t = ctx.NextValid(ref ctx.Pos);
            Debug.Log(t + " bought " + prizeName);
            ctx.Player = ctx.GetPlayer(t);
            if (prizeName != "A card")
            {
                var match = ctx.Player!.prize.GetMatches(prizeName)[0];
                ctx.Player!.Move(match, CardHolder.Type.PRIZE, CardHolder.Type.HAND);
                return $"{ctx.PlayerToken} bought {prizeName} from a prize";
            }
            else
            {
                ctx.Player!.Move(ctx.Player!.prize.cards[0], CardHolder.Type.PRIZE, CardHolder.Type.HAND);
                return $"{ctx.PlayerToken} bought an unknown prize";
            }
        }
    }
}