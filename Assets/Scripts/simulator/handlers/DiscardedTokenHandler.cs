using compiler;
using Unity.VisualScripting;
using UnityEngine;

namespace simulator.handlers
{
    public class DiscardedTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            var tmp = ctx.Pos;
            if (ctx.Next(ref ctx.Pos, TokenType.From) != null)
            {
                Debug.Log("Discarded from!");
                return "";
            }
                                
            ctx.Pos = tmp;
            ctx.Pos++;
            var nToken = ctx.Sequence[ctx.Pos];
            string[] discards;
                                
            if (nToken.TokenType == TokenType.Number)
            {
                ctx.ReadUntil(ref ctx.Pos, TokenType.FollowUp);
                ctx.Pos++;
                discards = ctx.ReadUntilValid(ref ctx.Pos).Split(", ");
            }
            else
            {
                discards = new[] { ctx.ReadUntilValid(ref ctx.Pos) };
            }
                                
            foreach (var d in discards)
            {
                var discard = d;
                if (discard.EndsWith(".")) discard = discard[..^1];
                var card = ctx.Player!.hand.GetMatches(discard)[0];
                ctx.Player!.Move(card, CardHolder.Type.HAND, CardHolder.Type.DISCARD);
            }

            return $"{ctx.PlayerToken} discarded {discards.ToCommaSeparatedString()}";
        }
    }
}