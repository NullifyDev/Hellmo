using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static Hellmo.Parser;

namespace Hellmo
{
    public class Data {
        #region Initializer
        Token token = new();
        public enum DataType {
            Hex, Binary, Bit, Ascii,
            StackHex, StackBinary,
            StackBit, StackAscii, NST
        };
        public DataType Type;
        public string Name = "";
        public object? Value = null;
        Parser parser = new Parser();
        #endregion
        #region Data Storage
        public static Dictionary<int, List<Hellmo.Function>> Functions = new();
        public static Dictionary<string, Stack> stacks = new();
        public static Dictionary<string, Var> variables = new();
        #endregion
        private static void VerboseLog(string msg, string cmd = "") {
            Parser parser = new();
            parser.VerboseLog(msg, 2, cmd);
        }
        public static void GetVars() {
            Utils.Outputln("[VARIABLE]");
            foreach (var x in variables.Select((Entry, Index) => new { Entry, Index }))
            {
                Utils.Outputln($"[{x.Index}] Name: {x.Entry.Key}, Type: {x.Entry.Value.Type}, Value: {x.Entry.Value.content}");
            }

            Utils.Outputln("\n[STACKS]");
            foreach (var x in stacks.Select((Entry, Index) => new { Entry, Index }))
            {
                Utils.Output($"[{x.Index}] Name: {x.Entry.Key}, Type: {x.Entry.Value.Type}, Value:");
                Utils.Output("[");
                foreach (var y in x.Entry.Value.Content) Utils.Output($"{y.content}");
                Utils.Outputln("]");
            }
        }
        public static void NewStack(string name, DataType type, List<int> value = null, List<string>? props = null)
        {
            if (props == null) { props = new(); }
            if (value == null) new List<int>() { 0 };
            if (props.Count() > 0)
            {
                Data.VerboseLog("Looping through 'properties' for invalid strings");
            }
            Data.VerboseLog($"[Creating New Stack] Name: {name}, Type: {type}, Other: {props}");
            Stack stack = new();
            stack.name = name;
            stack.Type = type switch
            {
                Data.DataType.Bit => Data.DataType.Bit,
                Data.DataType.Binary => Data.DataType.Binary,
                Data.DataType.Ascii => Data.DataType.Ascii,
                Data.DataType.Hex => Data.DataType.Hex,
                _ => Data.DataType.NST,
            };
            Data.VerboseLog($"Checking Stack Type...");
            if (stack.Type == DataType.NST) { /* parser.AddError("TypeError", $"No such type (NST) {type.ToString()}"); */ Data.VerboseLog($"BAD TYPE [{type.ToString()}]!", "exit"); }
            Data.stacks.Add(name, stack);
        }
        public static string IdentifyDataType(string val)
        {
            return val switch
            {
                "Stack" => "Stack",

                "Bit" => "Bit",
                "Hex" => "Hex",
                "Binary" => "Binary",
                "Ascii" => "Ascii",

                "StackBit" => "StackBit",
                "StackHex" => "StackHex",
                "StackBinary" => "StackBinary",
                "StackAscii" => "StackAscii",

                _ => "NST",
            };
        }
        public static void NewVar(string name, Hellmo.Data.DataType type, string? value = null)
        {
            Data Data = new();
            Parser parser = new();

            Var vari = new();
            vari.Type = type switch
            {
                Hellmo.Data.DataType.Bit => Hellmo.Data.DataType.Bit,
                Hellmo.Data.DataType.Binary => Hellmo.Data.DataType.Binary,
                Hellmo.Data.DataType.Ascii => Hellmo.Data.DataType.Ascii,
                Hellmo.Data.DataType.Hex => Hellmo.Data.DataType.Hex,
                _ => Hellmo.Data.DataType.NST,
            };
            Data.VerboseLog($"Checking Variable Type...");
            if (vari.Type == Hellmo.Data.DataType.NST) { parser.AddError("TypeError", $"No such type (NST) {type.ToString()}"); Data.VerboseLog($"BAD TYPE [{type}]!", "exit"); }
            Data.variables.Add(name, vari);
        }
        public static Stack GetStack(string name) => stacks.ContainsKey(name) ? stacks[name] : null;
        public static string GetDataType(Hellmo.Data.DataType obj) => obj.ToString();
        #region Condition Functions
        public static bool IsDataType(string val, int yes = 0)
        { 
            if (yes == 1) Utils.Outputln(val);
            return val.ToLower() switch
            {
                "bit" => true,
                "hex" => true,
                "bin" => true,
                "ascii" => true,

                "stackbit" => true,
                "stackhex" => true,
                "stackbinary" => true,
                "stackascii" => true,

                "stack.bit" => true,
                "stack.hex" => true,
                "stack.bin" => true,
                "stack.ascii" => true,

                _ => false,
            };
        }
        public static bool IsStack(string type) {
            if(type.ToLower().StartsWith("stack".ToLower())) return true;
            return false;
        }
        #endregion
        #region Conversion Functions
        public static Hellmo.Data.DataType StringToVarDataType(string val)
        {
            return val switch
            {
                "bit" => Hellmo.Data.DataType.Bit,
                "bin" => Hellmo.Data.DataType.Binary,
                "ascii" => Hellmo.Data.DataType.Ascii,
                "hex" => Hellmo.Data.DataType.Hex,
                _ => Hellmo.Data.DataType.NST,
            };
        }
        public static Hellmo.Data.DataType StringToStackDataType(string val)
        {
            return val.ToLower() switch
            {
                "stack.bit" => Hellmo.Data.DataType.StackBit,
                "stack.bin" => Hellmo.Data.DataType.StackBinary,
                "stack.ascii" => Hellmo.Data.DataType.StackAscii,
                "stack.hex" => Hellmo.Data.DataType.StackHex,
                _ => Hellmo.Data.DataType.NST,
            };
        }
        public static Hellmo.Data.DataType VarToStack(Hellmo.Data.DataType type) {
            return type switch {
                Hellmo.Data.DataType.Bit => Hellmo.Data.DataType.StackBit,
                Hellmo.Data.DataType.Binary => Hellmo.Data.DataType.StackBinary,
                Hellmo.Data.DataType.Ascii => Hellmo.Data.DataType.StackAscii,
                Hellmo.Data.DataType.Hex => Hellmo.Data.DataType.StackHex,
                _ => Hellmo.Data.DataType.NST,
            };
        }
        #endregion
    }
    public class Var
    {
        public int id = -1;
        public Hellmo.Data.DataType Type = new();
        public int content = 0;
        public int Length = 0;
    }
    public class Stack
    {
        public string name = "";
        public Hellmo.Data.DataType Type = new();
        public List<Var> Content = new();
        public int Length = 0;
        public List<string> Properties = new();
        public static Hellmo.Data.DataType ToVar(string type) => type switch
        {
            "DataType.Bit" => Hellmo.Data.DataType.Bit,
            "DataType.Binary" => Hellmo.Data.DataType.Binary,
            "DataType.Ascii" => Hellmo.Data.DataType.Ascii,
            "DataType.Hex" => Hellmo.Data.DataType.Hex,
            _ => Hellmo.Data.DataType.NST,
        };
    }
}