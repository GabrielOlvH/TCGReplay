using compiler;
using UnityEngine;

namespace simulator.handlers
{
    public class UsedTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            ctx.Pos = 0;
            ctx.NextInvalid(ref ctx.Pos);
            var pokemon = ctx.ReadUntilValid(ref ctx.Pos);
            ctx.Pos++;
            var used = ctx.ReadUntilValid(ref ctx.Pos);
            ctx.Player!.board.ShowPlayed(pokemon + " used " + used);
            return $"{ctx.PlayerToken}'s {pokemon} used {used}";
        }
    }
}