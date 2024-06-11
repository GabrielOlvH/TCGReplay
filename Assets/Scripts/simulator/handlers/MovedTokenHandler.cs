using compiler;

namespace simulator.handlers
{
    public class MovedTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            var target = ctx.Next(ref ctx.Pos, ctx.PlayerToken);
            var n = ctx.Next(ref ctx.Pos, TokenType.Number);
            var location = ctx.Next(ref ctx.Pos, TokenType.Location);
                                
            ctx.ReadUntil(ref ctx.Pos, TokenType.FollowUp);
            ctx.Pos++;
            var cards = ctx.ReadUntilValid(ref ctx.Pos).Split(", ");
            foreach (var d in cards)
            {
                var cardName = d;
                if (cardName.EndsWith(".")) cardName = cardName[..^1];
                          
                ctx.GetPlayer(target).DrawCards(1)[0].SetCardMatcher(new CardMatcher(cardName));
                //player!.Move(card, CardHolder.Type.DECK, GetLocation(location.Value));
            }
            return $"{ctx.PlayerToken}'s "; // TF DOES THIS DO?
        }
    }
}