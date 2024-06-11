using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace compiler
{
    public class Tokenizer
    {
        private readonly List<TokenDefinition> tokenDefinitions;

        public Tokenizer(string bluePlayerName, string redPlayerName)
        {
            tokenDefinitions = new List<TokenDefinition>
            {
                
                new(TokenType.BluePlayer, @$"^{bluePlayerName}('s)?"),
                new(TokenType.RedPlayer, @$"^{redPlayerName}('s)?"),
                
                new(TokenType.Drew, @"^drew"),
                new(TokenType.Played, @"^played"),
                new(TokenType.Attach, @"^attach(ed)?"),
                new(TokenType.Shuffled, @"^shuffled"),
                new(TokenType.Put, @"^put"),
                new(TokenType.Discarded, @"^discarded"),
                new(TokenType.Evolved, @"^evolved"),
                new(TokenType.Retreated, @"^retreated"),
                new(TokenType.KnockedOut, @"^was knocked out"),
                new(TokenType.AddedPrize, @"^was added to"),
                new(TokenType.Switched, @"^was switched with"),
                new(TokenType.Used, @"^used"),
                new(TokenType.Moved, @"^moved"),
                
                new(TokenType.Location, @"^(Bench|Active Spot|deck|bottom of their deck|hand\.|discard pile)"),
                
                new(TokenType.FollowUp, @"^(\- |   • )"),
                new(TokenType.And, @"^and "),
                new(TokenType.Into, @"^into "),
                new(TokenType.To, @"^to "),
                new(TokenType.From, @"^from "),
                new(TokenType.IsNow, @"^is now "),
                
                new(TokenType.Ignored, @"^(on|in) "),
                new(TokenType.NewLine, "\n+"),
                new(TokenType.Round, "^Turn #"),
                
                new(TokenType.Number, @"^( a |[0-9]+)")
            };
        }
        public List<DslToken> Tokenize(string lqlText)
        {
            var tokens = new List<DslToken>();

            var lines = lqlText.Split("\n");
            foreach (var line in lines)
            {
                if (line.Length == 1) continue;
                var remainingText = line;

                while (!string.IsNullOrWhiteSpace(remainingText))
                {
                    var match = FindMatch(remainingText);
                    if (match.IsMatch)
                    {
                        tokens.Add(new DslToken(match.TokenType, match.Value));
                        remainingText = match.RemainingText;
                    }
                    else
                    {
                        if (IsWhitespace(remainingText))
                        {
                            remainingText = remainingText.Substring(1);
                        }
                        else
                        {

                            var invalidTokenMatch = CreateInvalidTokenMatch(remainingText);
                            tokens.Add(new DslToken(invalidTokenMatch.TokenType, invalidTokenMatch.Value));
                            remainingText = invalidTokenMatch.RemainingText;
                        }
                    }
                }
                tokens.Add(new DslToken(TokenType.SequenceTerminator, ""));

            }
            
            return tokens;
        }

        private TokenMatch FindMatch(string lqlText)
        {
            foreach (var tokenDefinition in tokenDefinitions)
            {
                var match = tokenDefinition.Match(lqlText);
                if (match.IsMatch)
                    return match;
            }

            return new TokenMatch() {  IsMatch = false };
        }

        private bool IsWhitespace(string lqlText)
        {
            return Regex.IsMatch(lqlText, "^\\s+");
        }

        private TokenMatch CreateInvalidTokenMatch(string lqlText)
        {
            var match = Regex.Match(lqlText, "(^\\S+\\s)|^\\S+");
            if (match.Success)
            {
                return new TokenMatch()
                {
                    IsMatch = true,
                    RemainingText = lqlText.Substring(match.Length),
                    TokenType = TokenType.Invalid,
                    Value = match.Value.Trim()
                };
            }

            throw new Exception("Failed to generate invalid token");
        }
    }
}