using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static Velox.Parser;

namespace Velox
{
    public class Data
    {
        public string Type = "", Name = "";
        public object? Value = null;
        Parser parser = new Parser();

        public static Dictionary<int, List<Velox.Function>> Functions = new();
        public static Dictionary<string, Arr> stacks = new(); 
        public static Dictionary<string, Var> variables = new(); 
        private void VerboseLog(string msg, string cmd = "") => parser.VerboseLog(msg, 2, cmd);

        public void NewStack(string name, string type, List<int>? value = null, List<string>? props = null) {
            if (props == null) { props = new(); }

            if (props.Count() > 0) {
                this.VerboseLog("Looping through 'properties' for invalid strings");
                foreach (string x in props) {
                    switch(x) {
                        case "isFunctionArg":
                            break;
                        default:
                            Utils.Outputln($"\\R [Source] TypeError: The property {x} for variable {name} is not known. \n Pleasse consult the developer and/or report the issue on github. \\Reset");
                            Environment.Exit(1);
                            break;
                    }
                }
            }

            this.VerboseLog($"[Creating New Stack] Name: {name}, Type: {type}, Other: {props}");
            Arr stack = type switch {
                "bit"   =>  Velox.Variables.NewStack("Bit"),
                "bin"   =>  Velox.Variables.NewStack("Binary"),
                "ascii" =>  Velox.Variables.NewStack("Ascii"),
                "hex"   =>  Velox.Variables.NewStack("Hex"),
                _       =>  Velox.Variables.NewStack("NST"),
            };
            this.VerboseLog($"Checking Stack Type...");
            if (stack.Type == "NST") { parser.AddError("TypeError", $"No such type (NST) {type}"); this.VerboseLog($"BAD TYPE [{type}]!", "exit"); }
            Data.stacks.Add(name, stack);
        }

        public static Arr? GetStack(string name) => stacks.ContainsKey(name) ? stacks[name] : null;

        public void NewVar(string type, string name, string? value)
        {
            Var variable = type switch {
                "bit"   =>  Velox.Variables.NewVar("Bit"),
                "bin"   =>  Velox.Variables.NewVar("Binary"),
                "ascii" =>  Velox.Variables.NewVar("Ascii"),
                "hex"   =>  Velox.Variables.NewVar("Hex"),
                _       =>  Velox.Variables.NewVar("NST"),
            };
            this.VerboseLog($"Checking Variable Type...");
            if (variable.Type == "NST") { parser.AddError("TypeError", $"No such type (NST) {type}"); this.VerboseLog($"BAD TYPE [{type}]!", "exit"); }
            Data.variables.Add(name, variable);
        }
        
        public static string GetDataType<T>(T obj) {
            return "GetDataType() is not programmed yet";
        }

    }
    public static class Binary {
        public static void Validate() {}
        public static bool IsValid() {
            return false;
        } 
    }

    public class Var {
        public string Type = "";
        public int content = 0;
        public int Length = 0;
    }
}