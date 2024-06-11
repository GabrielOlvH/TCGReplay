using compiler;

namespace simulator.handlers
{
    public class PlayerTokenHandler : TokenHandler
    {
        public override string Handle(DslToken token, SequenceContext ctx)
        {
            ctx.PlayerToken = token.TokenType;
            ctx.Player = ctx.GetPlayer(token);
            return "";
        }
    }
}