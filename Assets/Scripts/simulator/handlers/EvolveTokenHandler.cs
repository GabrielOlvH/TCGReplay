using compiler;

namespace simulator.handlers
{
    public class EvolveTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            ctx.Pos++;
            var preEvolve = ctx.ReadUntilValid(ref ctx.Pos);
            ctx.Pos++;
            var evolved = ctx.ReadUntilValid(ref ctx.Pos);
            ctx.Pos++;
            var place = ctx.GetLocation(ctx.Next(ref ctx.Pos, TokenType.Location).Value);

                              
            var preEvolveCard = ctx.Player!.Get(place).GetMatches(preEvolve)[0];
            var evolvedCard = ctx.Player!.hand.GetMatches(evolved)[0];

            ctx.Player!.AttachTo(preEvolveCard, evolvedCard, CardHolder.Type.HAND, place); // TODO handle Evolution from other than Hand
            ctx.Player!.board.ShowPlayed(preEvolve + " evolved into " + evolved);
            return $"{ctx.PlayerToken} evolved {preEvolve} in {place} into {evolved} from HAND";
        }
    }
}