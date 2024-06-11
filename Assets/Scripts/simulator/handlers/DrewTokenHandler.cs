using System.Collections.Generic;
using compiler;
using Unity.VisualScripting;
using UnityEngine;

namespace simulator.handlers
{
    public class DrewTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            var message = "";
            Debug.Log("DREW!");
            var initialPos = ctx.Pos;
            var next = ctx.Sequence[ctx.Pos + 1];
            List<Card> drawn;
            if (next.TokenType == TokenType.Number)
            {
                drawn = ctx.Player!.DrawCards(ctx.GetNumber(next));

                if (ctx.CurrentRound < 1) // Ignore "7 drawn cards." from initial hand draw.
                {
                    ctx.Next(ref ctx.Pos, TokenType.FollowUp);
                    ctx.Pos++;
                }

                if (ctx.PlayerToken == TokenType.BluePlayer && ctx.Next(ref ctx.Pos, TokenType.FollowUp) != null) // Check if the drawn cards were specified
                {
                    ctx.Pos++;
                    var split = ctx.ReadUntil(ref ctx.Pos, TokenType.SequenceTerminator).Split(", ");
                    for (var index = 0; index < split.Length; index++)
                    {
                        drawn[index].SetCardMatcher(new CardMatcher(split[index].Trim()));
                    }

                    message = $"{ctx.PlayerToken} drew {split.ToCommaSeparatedString()}";
                }
                else
                {
                    message = $"{ctx.PlayerToken} drew {drawn.Count} cards";
                }
            }
            else
            {
                drawn = ctx.Player!.DrawCards(1);
                ctx.Pos++;
                var cardName = ctx.ReadUntilValid(ref ctx.Pos);
                if (cardName.EndsWith(".")) cardName = cardName[..^1];
                drawn[0].SetCardMatcher(new CardMatcher(cardName));
            }

            ctx.Pos = initialPos;

            if (ctx.Next(ref ctx.Pos, TokenType.And) != null) // Compound action
            {
                if (ctx.Sequence[++ctx.Pos].TokenType == TokenType.Played)
                {
                    var location = ctx.Next(ref ctx.Pos, TokenType.Location);
                    ctx.Pos++;
                    foreach (var card in drawn)
                    {
                        ctx.Player!.Move(card, CardHolder.Type.HAND, ctx.GetLocation(location.Value));
                    }

                    message += $" and played to {location}";
                }
            }

            return message;
        }
    }
}