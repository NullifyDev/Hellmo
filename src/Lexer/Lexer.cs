using System;
using System.Text;
using Hellmo;

namespace Hellmo
{
    public class Lexer
    {
        private int line = 1, curr = 0, start = 0;
        private bool isAtEnd => curr >= src.Length;
        private bool inCommentBlock = false;
        private Dictionary<string, TokenType> reserved = new Dictionary<string, TokenType>();
        private List<Token> tokens = new List<Token>();
        private string currLexeme => src[start..curr];
        private string src;
        Token token = new Token();

        public Lexer(string src)
        {
            this.src = src;
            this.reserved = new Dictionary<string, TokenType>
            {
                ["0x00"] = Hellmo.TokenType.Return,
                ["0x01"] = Hellmo.TokenType.PointerUp,
                ["0x02"] = Hellmo.TokenType.PointerDown,
                ["0x03"] = Hellmo.TokenType.JumpInScript,
                ["0x04"] = Hellmo.TokenType.JumpInStack,
                ["0x05"] = Hellmo.TokenType.IncrPointer,
                ["0x06"] = Hellmo.TokenType.DecrPointer,
                ["0x07"] = Hellmo.TokenType.SetValToAddr,
                ["0x08"] = Hellmo.TokenType.Print,
                ["0x09"] = Hellmo.TokenType.If,
                ["0x091"] = Hellmo.TokenType.Else,
                ["0x0A"] = Hellmo.TokenType.Switch,
                ["0x0A1"] = Hellmo.TokenType.Case,
                ["0x0B"] = Hellmo.TokenType.Exit,
                ["0x0C"] = Hellmo.TokenType.For,
                ["0x0D"] = Hellmo.TokenType.While,
                [":"] = Hellmo.TokenType._In,
                ["in"] = Hellmo.TokenType._In,
                ["0x0F"] = Hellmo.TokenType.Function
            };
        }

        #region Helper functions
        private char Next()
        {
            if (!isAtEnd)
                return src[curr++];
            return '\0';
        }

        private char PeekPrev(int behind = 1)
        {
            if (curr - behind >= src.Length)
                return '\0';
            return src[curr - behind];
        }

        private char Peek()
        {
            if (isAtEnd) return '\0';
            return src[curr];
        }

        private char PeekNext(int ahead = 1)
        {
            if (curr + ahead >= src.Length)
                return '\0';
            return src[curr + ahead];
        }

        private bool Match(char expected)
        {
            if (isAtEnd)
                return false;

            if (src[curr] != expected)
                return false;

            curr++;
            return true;
        }
        #endregion

        private void addToken(TokenType type, object? literal = null)
        {
            tokens.Add(token.Add(type, currLexeme, line, literal));
        }

        #region Scanners
        public List<Token> Scan()
        {
            while (!isAtEnd)
            {
                // We are at the beginning of the next lexeme
                start = curr;
                this.scanToken();
            }

            tokens.Add(token.Add(TokenType.EOF, "", line, null));

            // As a kind of workaround to 01, check if the first token is a Hellmo.TokenType.EOL and remove it

            if (tokens[0].Type == Hellmo.TokenType.EOL)
            {
                tokens.Remove(tokens[0]);
            }
            return tokens;
        }

        private void scanHex()
        {
            if ((int)Peek() == 48)
            {
                while (Char.IsLetterOrDigit(Peek()))
                {
                    int n = PeekNext();
                    if ((n <= 48 && n >= 57) || (n <= 65 && n >= 70) || (n <= 97 && n >= 100) || n == 120 || n == 88)
                    {
                        Next();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            while (Char.IsLetterOrDigit(Peek()) || Match('_'))
            {
                Next();
            }
            var hex = src.Substring(start, curr - start);
            if (hex.Contains("x") || hex.Contains("X"))
            {
                addToken(Hellmo.TokenType.Hex, hex);
            }
            else
            {
                addToken(Hellmo.TokenType.NumberLit, hex);
            }
            // if (Char.IsDigit(PeekNext())) {
            //     Console.WriteLine("Number Found");
            // }
        }

        private void scanIdentifier()
        {
            while (Char.IsLetterOrDigit(Peek()) || Match('_'))
                Next();
            var identifier = src.Substring(start, curr - start);

            // Check if reserved keyword exists
            if (reserved.ContainsKey(identifier))
            {
                var tokenType = reserved[identifier];
                addToken(tokenType);
            }
            else
            {
                addToken(Hellmo.TokenType.Identifier, identifier);
            }
        }

        private void scanNumber()
        {
            while (Char.IsDigit(Peek()))
                Next();

            if (Peek() == '.' && Char.IsDigit(PeekNext()))
            {
                Next();

                while (Char.IsDigit(Peek()))
                    Next();
            }

            var value = src.Substring(start, curr - start);
            addToken(TokenType.NumberLit, Double.Parse(value));
        }
        private void scanChar()
        {
            while (Peek() != '\'' && !isAtEnd)
            {
                if (Peek() == '\n')
                    line++;

                Next();
            }

            if (isAtEnd)
            {
                Utils.Error($"Line {line}: Unterminated character literal");
                return;
            }

            Next();

            string substring = src.Substring(start + 1, curr - start - 2);
            if (substring.Length > 1)
            {
                Utils.Error($"Line {line}: Too many characters in character literal");
                return;
            }

            char value = char.Parse(substring);
            addToken(Hellmo.TokenType.CharLit, value);
        }

        private void scanString()
        {
            // TODO: add string interpolation
            while (Peek() != '"' && !isAtEnd)
            {
                if (Peek() == '\n')
                    line++;

                Next();
            }

            if (isAtEnd)
            {
                Utils.Error($"Line {line}: Unterminated string literal");
                return;
            }

            // Closing quote
            Next();

            // Remove the surrounding quotes
            string value = src.Substring(start + 1, curr - start - 2);
            addToken(Hellmo.TokenType.StringLit, value);
        }

        private void scanToken()
        {
            char c = Next();
            switch (c)
            {
                case '[': addToken(Hellmo.TokenType.LeftBracket); break;
                case ']': addToken(Hellmo.TokenType.RightBracket); break;
                case ',': addToken(Hellmo.TokenType.Comma); break;
                case '.': addToken(Hellmo.TokenType.Dot); break;
                case '+': addToken(Hellmo.TokenType.Plus); break;
                case '*': addToken(Hellmo.TokenType.Star); break;
                case '=': addToken(Hellmo.TokenType.Equals); break;
                case '"': scanString(); break;
                case '\'': scanChar(); break;
                case '\\': // line continuation
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace
                    break;
                case '\n':
                    if (PeekNext(ahead: -2) != '\\')
                    {
                        // FIXME: 01: Kinda works but it still appends Hellmo.TokenType.EOL even if the line is actually empty
                        if (Peek() != ' ' && tokens.Count == 0 || Peek() != '\0' || tokens[tokens.Count - 1].Type != Hellmo.TokenType.EOL)
                        {
                            addToken(Hellmo.TokenType.EOL);
                        }
                        line++;
                    }
                    break;
                case '|':
                    inCommentBlock = false;
                    break;
                case '#':
                    if (Match('>'))
                    {
                        // Multiline comment block
                        inCommentBlock = true;
                    }

                    if (!inCommentBlock)
                    {
                        while (Peek() != '\n' && !isAtEnd) Next();
                        if (Peek() == '\n') Next();
                    }

                    break;
                default:
                    if (!inCommentBlock)
                    {
                        if (Char.IsDigit(c))
                        {
                            scanHex();
                        }
                        else if (Char.IsLetter(c) || c == '_')
                        {
                            scanIdentifier();
                        }
                        else
                        {
                            Utils.Error($"Line {line}: Unexpected character '{c}'");
                        }
                    }
                    else
                    {
                        // Inside a comment block, we ignore everything
                        while (Peek() != '<' && PeekNext() != '#' && !isAtEnd)
                            Next();
                    }
                    break;
            }
        }
        #endregion
    }
}