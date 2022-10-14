namespace Velox
{
    public enum TokenType
    {
        StringLit, NumberLit, Identifier, CharLit,

        LeftParen, RightParen, LeftBrace, RightBrace, LeftBracket, RightBracket,
        Comma, Plus, Star, Slash, Colon, Semicolon, Bang, Equals, Dot,

        PointerUp, PointerDown, IncrPointer, DecrPointer,
        JumpInStack, JumpInScript, SetValToAddr, Print, If,
        Else, Switch, Case, Return, For, While, _In, Of, Function,

        Hex,

        Exit,

        EOL, EOF
    }

    public class Token
    {
        public TokenType Type;
        public int Line;
        public object Lexeme = new Object();
        public object? Literal;

        public Token Add(TokenType type, object lexeme, int line, object? literal = null)
        {
            Token token = new();
            token.Type = type;
            token.Lexeme = lexeme;
            token.Line = line;
            token.Literal = literal;
            return token;
        }
    }
}