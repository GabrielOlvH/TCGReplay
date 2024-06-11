using compiler;

namespace simulator.handlers
{
    public class PlayedTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            ctx.Pos++;
            var playedCard = ctx.ReadUntilValid(ref ctx.Pos);
            if (playedCard.EndsWith(".")) playedCard = playedCard[..^1];

            var loc = ctx.Next(ref ctx.Pos, TokenType.Location);
            var match = ctx.Player!.hand.GetMatches(playedCard)[0];
            if (loc == null)
            {
                ctx.Player!.Move(match, CardHolder.Type.HAND, CardHolder.Type.DISCARD);
                ctx.Player!.board.ShowPlayed("Played " + playedCard);
                ctx.CardsPlayed.Add(match);
                return $"{ctx.PlayerToken} played {playedCard}";
            }
            else
            { 
                ctx.Player!.Move(match, CardHolder.Type.HAND, ctx.GetLocation(loc.Value));
                return $"{ctx.PlayerToken} played {playedCard} to ${loc.Value}";
            }
        }
    }
}