using System.Collections.Generic;
using compiler;

namespace simulator.handlers
{
    public abstract class TokenHandler
    {
        public static readonly Dictionary<TokenType, TokenHandler> Handlers = new()
        {
            { TokenType.RedPlayer, new PlayerTokenHandler() },
            { TokenType.BluePlayer, new PlayerTokenHandler() },
            { TokenType.Round, new RoundTokenHandler() },
            { TokenType.Drew, new DrewTokenHandler() },
            { TokenType.Played, new PlayedTokenHandler() },
            { TokenType.Used, new UsedTokenHandler() },
            { TokenType.Attach, new AttachTokenHandler() },
            { TokenType.Evolved, new EvolveTokenHandler() },
            { TokenType.Retreated, new RetreatTokenHandler() },
            { TokenType.Discarded, new DiscardedTokenHandler() },
            { TokenType.Moved, new MovedTokenHandler() },
            { TokenType.Switched, new SwitchedTokenHandler() },
            { TokenType.KnockedOut, new KnockedOutTokenHandler() },
            { TokenType.IsNow, new IsNowTokenHandler() },
            { TokenType.AddedPrize, new AddedPrizeTokenHandler() },
            { TokenType.Put, new PutTokenHandler() }
        };
        
        public abstract string Handle(DslToken token, SequenceContext ctx);
    }
}