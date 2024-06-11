using compiler;
using UnityEngine;

namespace simulator.handlers
{
    public class AttachTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            ctx.Pos++;
            var attachment = ctx.ReadUntilValid(ref ctx.Pos);
            ctx.Pos++;
            var attachTo = ctx.ReadUntilValid(ref ctx.Pos);
            ctx.Pos++;
            var where = ctx.Next(ref ctx.Pos, TokenType.Location);
            var place = ctx.GetLocation(where.Value);

            var from = CardHolder.Type.HAND;
            foreach (var card in ctx.CardsPlayed)
            {

                var ruleTokens =
                    new Tokenizer("bluePlayer", "redPlayer").Tokenize(card.Matcher.Matches[0].Rules[0]);
                var ruleCtx = new SequenceContext
                {
                    Simulator = ctx.Simulator,
                    Sequence = ruleTokens,
                    Pos = 0
                };
                var tmp1 = ruleCtx.Next(ref ruleCtx.Pos, TokenType.Attach);
                ruleCtx.Next(ref ruleCtx.Pos, TokenType.Number);
                var tmp2 = ruleCtx.NextValid(ref ruleCtx.Pos);
                if (tmp1 != null && tmp2.TokenType == TokenType.From)
                {
                    from = ruleCtx.GetLocation(ruleCtx.Next(ref ruleCtx.Pos, TokenType.Location).Value);
                    
                    Debug.Log("Attaching from " + from + " using context ");
                }
            }

            var attachmentCard = ctx.Player!.Get(from).GetMatches(attachment)[0];
            var pokemonCard = ctx.Player!.Get(place).GetMatches(attachTo)[0];

            ctx.Player!.AttachTo(pokemonCard, attachmentCard, from, place);

            ctx.Player!.board.ShowPlayed(attachment + " was attached to " + attachTo);
            return $"{ctx.PlayerToken} attached {attachment} to {attachTo} in the {place} from their {from}";
        }
    }
}