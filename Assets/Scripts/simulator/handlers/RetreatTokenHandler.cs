using compiler;
using UnityEngine;

namespace simulator.handlers
{
    public class RetreatTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            var pokemon = ctx.ReadUntilValid(ref ctx.Pos);
            var loc = ctx.Next(ref ctx.Pos, TokenType.Location);
            ctx.ReadUntil(ref ctx.Pos, TokenType.SequenceTerminator);
            if (ctx.Next(ref ctx.Pos, TokenType.FollowUp) != null)
            {
                ctx.Pos++;
                var discarded = ctx.ReadUntilValid(ref ctx.Pos);
                                    
            }

            var match = ctx.Player!.active.GetMatches(pokemon)[0];
            ctx.Player!.Move(match, CardHolder.Type.ACTIVE, ctx.GetLocation(loc.Value));
            return $"{ctx.PlayerToken} retreated their {pokemon} to {loc.Value}";
        }
    }
}