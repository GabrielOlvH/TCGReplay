using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using compiler;
using simulator.handlers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

namespace simulator
{
    public class Simulator
    {
        public static bool isPaused;
        public bool Simulation;
        public List<DslToken> Tokens;
        public int StartToken = 0;
        
        public string bluePlayer;
        public string redPlayer;
        
        public CardController BlueController;
        public CardController RedController;
        public int currentRound = 0;
        public int delay;
        
        private List<RoundData> game = new();
        private RoundData roundBuilder;
        
        public void Parse()
        {
            BlueController.simulation = Simulation;
            RedController.simulation = Simulation;
            roundBuilder = new RoundData(0);
            for (var i = StartToken; i < Tokens.Count; i++)
            {
                var sequence = new List<DslToken>();

                while (Tokens.Count > i)
                {
                    sequence.Add(Tokens[i]);
                    i++;
                    if (Tokens.Count - 1 > i && Tokens[i].TokenType == TokenType.SequenceTerminator &&
                        Tokens[i + 1].TokenType != TokenType.FollowUp)
                    {
                        break;
                    }
                }

                if (Tokens.Count > i) sequence.Add(Tokens[i]);
                if (sequence.First().TokenType == TokenType.Round)
                {
                    game.Add(roundBuilder);
                    var number = GetNumber(sequence[1]);
                    roundBuilder = new RoundData(number);
                }

                //sequence.RemoveAll(s => s.TokenType == TokenType.SequenceTerminator);
                //sequence.Add(new DslToken(TokenType.SequenceTerminator));

                Do(() =>
                {
                    var ctx = new SequenceContext
                    {
                        Simulator = this,
                        CurrentRound = currentRound,
                        Sequence = sequence,
                        Pos = 0,
                        CardsPlayed = new(),
                        PreviousAction = TokenType.And,
                        PlayerToken = TokenType.BluePlayer,
                        Player = null
                    };

                    for (var seqPos = 0; seqPos < sequence.Count; seqPos++)
                    {
                        var token = sequence[seqPos];
                        ctx.Pos = seqPos;
                        try
                        {
                            if (TokenHandler.Handlers.TryGetValue(token.TokenType, out var handler))
                            {
                                var desc = handler.Handle(token, ctx);
                                if (desc.Length != 0) 
                                    BlueController.board.Log.text += "\n\n" + desc;
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"Error on sequence: {sequence.ToSeparatedString("\n")}");
                            Debug.LogError("At position " + seqPos + " (" + token.TokenType + " = " + token.Value + ")");
                            Debug.LogException(e);
                        }

                        seqPos = ctx.Pos;
                        ctx.PreviousAction = token.TokenType;
                    }
                });
            }

            game.Add(roundBuilder);
        }

        public void AlternatePath(Action action, int startToken)
        {
            Debug.Log("Starting alternate path");
            var path = new Simulator
            {
                Simulation = Simulation,
                Tokens = Tokens,
                StartToken = startToken,
                bluePlayer = bluePlayer,
                redPlayer = redPlayer,
                BlueController = BlueController,
                RedController = RedController,
            };
            path.game.AddRange(game.Select(round => round.Copy()));
            path.roundBuilder = roundBuilder.Copy();


            action.Invoke();
            path.Parse();
        }

        public bool Step()
        {
            if (isPaused) return false;
            if (--delay > 0) return true;
            if (currentRound >= game.Count) return false;
            var roundData = game[currentRound];
            if (!roundData.HasNext())
            {
                currentRound++;
            }
            Debug.Log("Current Round " + currentRound + "/" + game.Count + " with " + game[currentRound].Actions.Count);

            roundData.Next().Invoke();
            if (!Simulation) 
                delay = RoundData.DelayBetweenActions;
            return true;
        }


        private void Do(Action a)
        {
            roundBuilder.Actions.Add(a);
        }

        public int GetNumber(DslToken token)
        {
            Assert.IsTrue(token.TokenType == TokenType.Number);

            return token.Value.Equals(" a ") ? 1 : int.Parse(token.Value);
        }
    }
}