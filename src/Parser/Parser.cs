using Newtonsoft.Json;
using System;
using Hellmo;

using static Hellmo.Utils;
using static Hellmo.Hex;

namespace Hellmo
{
    public class Parser
    {
        private int verbosity = 0;
        private List<Token> tokens = new();
        private List<Error> errors = new();
        private Dictionary<string, Stack> tempFuncStack = new();
        private Dictionary<string, Var> tempFuncVar = new();
        private int current = 0;
        private int pointer = 0;
        private int attempts = 0;
        private bool done = false;
        // Data data = new Data();

        public Parser(List<Token> tokens = null)
        {
            if (tokens != null) this.tokens = tokens;
        }

        public void Parse(List<Token> tokens)
        {
            if (errors.Count > 0)
            {
                this.PrintErrors();
            }
            if (check(Peek().Type))
            {
                Data data = new Data();
                if (Peek().Type.ToString() == "Hex")
                {
                    switch (Peek().Lexeme.ToString().ToLower())
                    {
                        case "0x00": // return [X]
                            this.VerboseLog($"Returning {PeekNext().Lexeme.ToString()}");
                            break;
                        case "0x01":
                            this.VerboseLog($"Moving Pointer Up. [Now: {pointer}, Target {pointer + 1}]", 1);
                            pointer++;
                            break;
                        case "0x02":
                            this.VerboseLog($"Moving Pointer Down. [Now: {pointer}, Target {pointer - 1}]", 1);
                            pointer--;
                            break;
                        case "0x03": // Jump to pointer [X] in the Stack [Y]. If first argument is 0x07 (function), Call function by ID.
                            this.VerboseLog($"Jumping To Target Address {pointer - 1}", 1);
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
                            if (Peek().Lexeme.ToString() == "" || Peek().Lexeme.ToString() == null)
                            {

                            }
                            SetAt(Data.GetStack(PeekNext().Lexeme.ToString()), Hex.ToNumber(PeekNext(2).Lexeme.ToString()), Hex.ToNumber(PeekNext(3).Lexeme.ToString()));
                            break;
                        case "0x08":
                            while (PeekNext().Type != TokenType.EOL) {
                                Next();
                                Output(Peek().Type switch
                                {
                                    TokenType.StringLit => ((Func<string>)(() => Peek().Lexeme.ToString().Remove(0,1).Remove(Peek().Lexeme.ToString().Remove(0,1).Length-1, 1).ToString() ))(),
                                    TokenType.NumberLit => ((Func<string>)(() => Peek().Lexeme.ToString() ))(),
                                    // TokenType.Dot => ((Func<string>)(() => { 
                                    //     Next();
                                    //     if (PeekNext().Lexeme.ToString() == "ToNumber") {
                                    //         return this.ToNumber(PeekPrev()).ToString();
                                    //     }
                                    //     AddError("TypeError", $"No such thing as {PeekNext().Lexeme}");
                                    //     Environment.Exit(0);
                                    //     return "";
                                    // } ))(),
                                    TokenType.Hex => ((Func<string>)(() => Hex.ToNumber(PeekPrev().Lexeme.ToString()).ToString() ))(),
                                    _ => ((Func<string?>)(() => { 
                                        if (PeekNext().Type == TokenType.Dot) { 
                                            Next();
                                            if (PeekNext().Lexeme.ToString() == "ToNumber") {
                                                return this.ToNumber(PeekPrev()).ToString();
                                            }
                                            AddError("TypeError", $"No such thing as {PeekNext().Lexeme}");
                                            Environment.Exit(0);
                                            return "";
                                        }
                                        if (Peek().Lexeme.ToString() == "0" || Peek().Lexeme.ToString() == "null") return "0";
                                        if (Data.stacks.ContainsKey(Peek().Lexeme.ToString())) {
                                            var key = Data.stacks.Where(stack => stack.Key == Peek().Lexeme.ToString()).Select(pair => pair.Value).FirstOrDefault();
                                            List<Stack> x = Data.stacks.Values.ToList();

                                            // x[key].Content;
                                            return "";
                                        }
                                        return "";
                                    } ))(),
                                });
                            }
                            Output("\n");
                            break;
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
                            if (Hex.IsConvertableToNumber(PeekNext().Lexeme.ToString()))
                            {
                                Function func = new();
                                Next();
                                func.id = Hex.ToNumber(Peek().Lexeme.ToString());
                                this.VerboseLog($"Making Function With ID of {Hex.ToNumber(Peek().Lexeme.ToString())}", 1);
                                Next();

                                if (Peek().Type.ToString() == "LeftBracket") { 
                                    this.VerboseLog($"Checking for arguments", 1);
                                    this.CheckParams(func.id, data); 
                                } else if (Peek().Type.ToString() == "Colon") {
                                    while ((Peek().Lexeme.ToString() == "0x00") && (Hex.IsConvertableToNumber(PeekNext().Lexeme.ToString()))) 
                                        func.block.Add(Peek());
                                } else {
                                    Parser parser = new();
                                    parser.AddError("SyntaxError", "Function doesnt have an argument");
                                }
                            } 
                            Data.GetVars();
                            tempFuncStack = new();
                            tempFuncVar = new();
                            break;
                    }
                } else if (Peek().Type.ToString() == "Identifier") {
                    if (Peek().Lexeme.ToString() == "stack") {
                        if (PeekNext(3).Lexeme.ToString() == "=") Data.NewStack(PeekPrev().Lexeme.ToString(), Data.StringToStackDataType($"{Peek().Lexeme}{PeekNext().Lexeme}{PeekNext(2).Lexeme}"), new List<int> { int.Parse(PeekNext(3).Lexeme.ToString()) } );
                        else Data.NewStack(PeekPrev().Lexeme.ToString(), Data.StringToStackDataType($"{Peek().Lexeme}{PeekNext().Lexeme}{PeekNext(2).Lexeme}"));
                    }
                    else if (Data.IsDataType(Peek().Lexeme.ToString()))
                    {
                        if (PeekNext().Lexeme.ToString() == "=") Data.NewVar(PeekPrev().Lexeme.ToString(), Data.StringToVarDataType($"{Peek().Lexeme}"), PeekNext().Lexeme.ToString());
                        else Data.NewVar(PeekPrev().Lexeme.ToString(), Data.StringToVarDataType($"{Peek().Lexeme}"));
                    }
                }
            }
        }
        public int? ToNumber(Token token) {
            if (token.Type == TokenType.Hex) {
                return Hex.ToNumber(token.Lexeme.ToString());   
            } else if (token.Type == TokenType.NumberLit) {

                foreach(int x in token.Lexeme.ToString()) {
                    if (x == 0 || x == 1) continue;
                    else { this.AddError("TypeError", $"The value {token.Lexeme} is a number and not a binary value. Did you mean to use Hex (e.g.: 0x00)?"); Environment.Exit(0); return null; }
                }
                return Binary.ToNumber(int.Parse(token.Lexeme.ToString())); 
            } else {
                return null;    
            }
        }
        public void CheckParams(int id, Data data, string name = "", Data.DataType type = Data.DataType.NST, string value = "0", int next = 1)
        {
            // Outputln($"FUNCTION PARAMETERS: Name: {name} | Type: {type} | Value: {value}");
            // if (Peek().Lexeme.ToString() == ",") Next()/;
            if (done) return;
            Next(next);
            switch (Peek().Lexeme.ToString())
            {
                case ".":
                    if (Data.IsDataType($"{PeekPrev().Lexeme}{Peek().Lexeme}{PeekNext().Lexeme}")) CheckParams(id, data, name, Data.StringToStackDataType($"{PeekPrev().Lexeme}{Peek().Lexeme}{PeekNext().Lexeme}"), value, 2);
                    break;
                case ",":
                    if (Data.IsStack(type.ToString())) {
                        Data.NewStack(name, type, new List<int> { 0 });
                    }
                    else {
                        Data.NewVar(name, type, value);
                    }
                    // CheckParams(id, data, "", Data.DataType.NST, "0");
                    // CheckParams(id, data, "", Data.DataType.NST, "0");
                    break;
                case "]": done = true; return;
                case "=":
                    // Outputln($"[=] Name: {name}, Type: {type}, Value: {PeekNext().Lexeme.ToString()}");
                    if (Data.IsStack(type.ToString())) {
                        Data.NewStack(name, type);
                    }
                    else {
                        Data.NewVar(name, type, value);
                    }
                    CheckParams(id, data, "", Data.DataType.NST, "0", 2);
                    break;

                default:
                    // Outputln($"[Identifier]: CURRENT TOKEN: {Peek().Lexeme.ToString()}");
                    if (Data.IdentifyDataType(Peek().Lexeme.ToString()) == "stack")
                    {
                        if (Data.IsDataType($"{Peek().Lexeme}{PeekNext().Lexeme}{PeekNext(2).Lexeme}", 1)) CheckParams(id, data, name, Data.StringToStackDataType($"{PeekPrev().Lexeme}{Peek().Lexeme}{PeekNext().Lexeme}"), value, 2);
                        CheckParams(id, data, name, type, value);
                    }
                    else if (Data.IsDataType(Peek().Lexeme.ToString())) CheckParams(id, data, PeekPrev().Lexeme.ToString(), Data.StringToVarDataType(Peek().Lexeme.ToString()), value);
                    else if (Peek().Lexeme.ToString() == ",")
                    {
                        Next();
                        Output("[Identifier]: NextPeek(): " + Peek().Lexeme.ToString());
                        CheckParams(id, data, PeekNext().Lexeme.ToString(), type, value);
                    }
                    else
                    {
                        if (Peek().Lexeme.ToString() == "stack") CheckParams(id, data, name, type, value);
                        CheckParams(id, data, Peek().Lexeme.ToString(), type, value);
                        break;
                    }
                    break;
            }
        }
        #region Logging
        public void PrintErrors(bool exit = false)
        {
            this.VerboseLog($"Printing Errors...", 1);
            for (int i = 0; i < errors.Count; i++)
            {   
                Errors.PrintError($"{errors[i].Type} At Line {errors[i].Line}: {errors[i].Message}");
            }
            if (exit) Environment.Exit(1);
        }
        public void VerboseLog(string message, int level = 1, string cmd = "")
        {
            if (verbosity > 0)
            {
                if (level <= verbosity)
                {
                    Outputln("[VERBOSE MODE - PARSER]: " + message);
                    switch (cmd)
                    {
                        case "exit":
                            this.PrintErrors();
                            Environment.Exit(0);
                            return;
                    }
                }
            }
        }
        public void AddError(string type, string message) { Errors.Add(type, /* Peek().Line,*/ message); }
        #endregion
        #region Helper functions
        private bool isAtEnd() => Peek().Type == TokenType.EOF;
        private Token Peek() => tokens[current];
        private Token PeekPrev(int prev = 1) => tokens[current - prev];
        private Token PeekNext(int next = 1)
        {
            return tokens[current + next];
        }

        private Token? consume(TokenType type, string message)
        {
            if (check(type))
                return Next();
            return null;
        }

        private Token Next(int next = 1)
        {
            // Outputln($"Current: {Peek().Type} Next: {tokens[current + next].Lexeme}");
            if (!isAtEnd())
                current += next;
            return Peek();
        }

        private bool check(TokenType type)
        {
            if (isAtEnd()) return false;
            return Peek().Type == type;
        }

        private bool matches(params TokenType[] types)
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