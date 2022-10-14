using Newtonsoft.Json;
using System;
using Velox;

using static Velox.Utils;
using static Velox.Hex;

namespace Velox
{
    public class Parser
    {
        private  int verbosity = 1;
        private  List<Token> tokens = new();
        private  List<Error> errors = new();
        private  int current = 0;
        private  int pointer = 0;
        // Data data = new Data();

        public Parser(List<Token> tokens = null) {
            if (tokens != null) this.tokens = tokens;
        }

        public void Parse(List<Token> tokens) {
            if (errors.Count > 0) {
                this.PrintErrors();
            }
            if (check(Peek().Type)) {
                Data data = new Data();
                switch(Peek().Lexeme.ToString().ToLower()) {
                    case "0x00": // return [X]
                        this.VerboseLog($"Returning {PeekNext().Lexeme.ToString()}");
                        break;
                    case "0x01":
                        this.VerboseLog($"Moving Pointer Up. [Now: {pointer}, Target {pointer+1}]", 1);
                        pointer++;
                        break;
                    case "0x02":
                        this.VerboseLog($"Moving Pointer Down. [Now: {pointer}, Target {pointer-1}]", 1);
                        pointer--;
                        break;
                    case "0x03": // Jump to pointer [X] in the Stack [Y]. If first argument is 0x07 (function), Call function by ID.
                        this.VerboseLog($"Jumping To Target Address {pointer-1}", 1);
                        break;
                    case "0x04":
                        this.VerboseLog($"Incrementing Address {pointer} by 1", 1);
                        break;
                    case "0x05": //
                        this.VerboseLog($"Decrementing Address {pointer} by 1", 1);
                        break;
                    case "0x06": 
                        this.VerboseLog($"Changing Value of Address {pointer} by {PeekNext().Lexeme.ToString()}", 1);
                        break;
                    case "0x07": // Set [X] Address to [Y] Value
                        this.VerboseLog($"Setting Value {PeekNext().Lexeme.ToString()} To Address {pointer}", 1);
                        Next();
                        if (Peek().Lexeme.ToString() == "" || Peek().Lexeme.ToString() == null) {
                            
                        }
                        SetAt(Data.GetStack(PeekNext().Lexeme.ToString()), Hex.ToNumber(PeekNext(2).Lexeme.ToString()), Hex.ToNumber(PeekNext(3).Lexeme.ToString()));
                        break;
                    case "0x08": 
                    case "0x09": // Print what ever's after it. Include everything after '+'
                    case "0x0a":
                    case "0x0b": // Exit application with code [X]
                        Next();
                        this.VerboseLog($"Quitting Application...", 1);
                        Environment.Exit(Hex.ToNumber(Peek().Lexeme.ToString()));
                        break;
                    case "0x0c":
                    case "0x0d":
                    case "0x0e":
                        break;
                    case "0x0f":
                        if (Hex.IsConvertableToNumber(PeekNext().Lexeme.ToString())) {
                            Next();
                            this.VerboseLog($"Making Function With ID of {Hex.ToNumber(Peek().Lexeme.ToString())}", 1);
                            Next();
                            if (Peek().Type.ToString()  == "LeftBracket") {
                                this.VerboseLog($"Checking for arguments", 1);
                                this.CheckParams(data);
                            }
                        }
                        break;
                }
            }
        }

        public void CheckParams(Data data) {
            Next();
            string name = Peek().Lexeme.ToString();
            Outputln($"[VERBOSE MODE - PARSER] Variable Name: {name}");
            Next();


            Output($"[VERBOSE MODE - PARSER]: Identifying type of {name} argument:");
            if (Peek().Type.ToString() == "Identifier") {
                Next();
                PeekNext().Type.ToString();
                switch(Peek().Type.ToString()) {
                    case "Dot":
                        Next();
                        Outputln($"stack.{Peek().Lexeme.ToString()}");
                        // Outputln($"[VERBOSE MODE - PARSER] Adding new stack [Name: {name}, Type: stack.{Peek().Lexeme.ToString()}] to Stack Dictionary as function argument");
                        data.NewStack(name, Peek().Lexeme.ToString(), new List<string> { "isFunctionArg" } );
                        Next();
                        if (Peek().Type.ToString() == "Comma") CheckParams(data);
                        else if (PeekNext().Type.ToString() == "Equals") { Next(2);  }
                        break;
                    case "Comma": CheckParams(data); break;
                    case "RightBracket": return;
                    default:
                        Outputln($"Something went wrong... Here's what we see: [{Peek().Type.ToString()}]: {Peek().Lexeme.ToString()}");
                        break;
                }
            }
        }

        public  void PrintErrors(bool exit = false) {
            this.VerboseLog($"Printing Errors...", 1);
            for (int i = 0; i < errors.Count; i++) {
                Errors.PrintError($"{errors[i].Type} At Line {errors[i].Line}: {errors[i].Message}");
            }
            if (exit) Environment.Exit(1);
        }

        public  void VerboseLog(string message, int level = 1, string cmd = "") { 
            if (verbosity > 0) {
                if (level <= verbosity) { 
                    Outputln("[VERBOSE MODE - PARSER]: " + message);
                    switch(cmd) {
                        case "exit":
                            this.PrintErrors();
                            Environment.Exit(0);
                            break;
                    }
                }
            } 
        }
        public void AddError(string type, string message) => Errors.Add(type, Peek().Line, message);

        #region Helper functions
        private  bool isAtEnd() => Peek().Type == TokenType.EOF;

        private Token Peek()
        {
            return tokens[current];
        }

        private  Token PeekPrev(int prev = 1)
        {
            return tokens[current - prev];
        }

        private  Token PeekNext(int next = 1)
        {
            return tokens[current + next];
        }

        private  Token? consume(TokenType type, string message)
        {
            if (check(type))
                return Next();


            return null;
        }

        private Token Next(int next = 1)
        {
            if (!isAtEnd())
                current += next;
            return Peek();
        }

        private  bool check(TokenType type)
        {
            if (isAtEnd()) return false;
            return Peek().Type == type;
        }

        private  bool matches(params TokenType[] types)
        {
            foreach (var t in types)
            {
                if (check(t))
                {
                    Next();
                    return true;
                }
            }

            return false;
        }
        #endregion

    }
}