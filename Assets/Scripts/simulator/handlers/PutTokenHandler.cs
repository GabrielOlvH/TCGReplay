using System.Linq;
using compiler;
using Unity.VisualScripting;
using UnityEngine;

namespace simulator.handlers
{
    public class PutTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            Debug.Log("PUT!!!!");
            var targetName = ctx.Sequence[ctx.Pos - 1].Value;
            Debug.Log("TARGET = " +  targetName);
            var target = ctx.GetBoard(targetName);
            var number = ctx.GetNumber(ctx.Next(ref ctx.Pos, TokenType.Number));
            var loc = ctx.GetLocation(ctx.Next(ref ctx.Pos, TokenType.Location).Value);
            ctx.ReadUntil(ref ctx.Pos, TokenType.FollowUp);
            ctx.Pos++;
            var cards = ctx.ReadUntilValid(ref ctx.Pos).Split(", ");
            Debug.Log($"{targetName} put {number} cards to {loc}:  {cards.ToSeparatedString(", ")}");
            if (number != cards.Length)
            {
                for (var i = 0; i < number; i++)
                {
                    
                    var matches = target.hand.cards;
                    target.Move(matches[0], CardHolder.Type.HAND, loc);
                }

                return $"{ctx.PlayerToken} put {number} cards to {loc}";
            }
            else
            {
                foreach (var d in cards)
                {
                    var cardName = d;
                    if (cardName.EndsWith(".")) cardName = cardName[..^1];

                    var matches = target.hand.GetMatches(cardName);
                    target.Move(matches[0], CardHolder.Type.HAND, loc);
                    Debug.Log("Moved " + d + " to " + loc);
                    //player!.Move(card, CardHolder.Type.DECK, GetLocation(location.Value));
                }
                return $"{ctx.PlayerToken} put {cards.ToCommaSeparatedString()} cards to {loc}";
                
            }
        }
    }
}