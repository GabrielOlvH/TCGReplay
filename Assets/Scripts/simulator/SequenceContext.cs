using System;
using System.Collections.Generic;
using compiler;
using UnityEngine.Assertions;

namespace simulator
{
    public class SequenceContext
    {
        public Simulator Simulator;
        public int CurrentRound;
        public List<DslToken> Sequence;
        public int Pos;
        public List<Card> CardsPlayed;
        public TokenType PreviousAction;
        public TokenType PlayerToken;
        public CardController Player;
          public CardHolder.Type GetLocation(string name)
        {
            if (name == "Bench")
            {
                return CardHolder.Type.BENCH;
            }
            else if (name == "Active Spot")
            {
                return CardHolder.Type.ACTIVE;
            }
            else if (name == "bottom of their deck")
            {
                return CardHolder.Type.DECK;
            }
            else if (name == "discard pile")
            {
                return CardHolder.Type.DISCARD;
            }
            else
            {
                throw new Exception("Not a location " + name);
            }
        }

        public void Previous(ref int pos)
        {
            var dslToken = Sequence[--pos];
            while (dslToken.TokenType != TokenType.SequenceTerminator)
            {

                if (pos < 1) break;
                dslToken = Sequence[--pos];
            }
        }

        public DslToken Next(ref int pos, TokenType find)
        {
            if (Sequence[pos].TokenType == TokenType.SequenceTerminator) return null;
            var dslToken = Sequence[++pos];
            while (dslToken.TokenType != find)
            {
                if (Sequence.Count - 1 <= pos) return null;
                dslToken = Sequence[++pos];
            }

            return dslToken;
        }

        public DslToken NextValid(ref int pos)
        {
            if (Sequence[pos].TokenType == TokenType.SequenceTerminator) return null;
            var dslToken = Sequence[++pos];
            while (dslToken.TokenType == TokenType.Invalid)
            {
                dslToken = Sequence[++pos];
            }

            return dslToken;
        }

        public DslToken NextInvalid(ref int pos)
        {
            if (Sequence[pos].TokenType == TokenType.SequenceTerminator) return null;
            var dslToken = Sequence[++pos];
            while (dslToken.TokenType != TokenType.Invalid)
            {
                dslToken = Sequence[++pos];
            }

            return dslToken;
        }

        public string ReadUntilValid(ref int start)
        {
            var value = "";
            while (Sequence[start].TokenType == TokenType.Invalid)
            {
                value += Sequence[start].Value.Trim() + " ";
                start++;
            }

            return value.Trim();
        }

        public int GetNumber(DslToken token)
        {
            Assert.IsTrue(token.TokenType == TokenType.Number);

            return token.Value.Equals(" a ") ? 1 : int.Parse(token.Value);
        }

        public string ReadUntil(ref int start, TokenType type)
        {
            var value = "";
            while (Sequence[start].TokenType != type)
            {
                value += Sequence[start].Value.Trim() + " ";
                start++;
            }

            return value.Trim();
        }
        
        public CardController GetPlayer(DslToken token)
        {
            Assert.IsTrue(token.TokenType is TokenType.RedPlayer or TokenType.BluePlayer);
            return GetBoard(token.Value.Replace(@"'s", ""));
        }

        public CardController GetBoard(string player)
        {
            if (player.Replace(@"'s", "") == Simulator.redPlayer.Trim())
            {
                return Simulator.RedController;
            }
            else
            {
                return Simulator.BlueController;
            }
        }
    }
}