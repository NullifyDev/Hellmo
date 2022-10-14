using System;

using Velox;
namespace Velox
{
    public class Variables
    {
        // static List<Arr> stack = new();

        public static Arr NewStack(string type, List<int>? content = null) {
            var arr = new Arr();
            
            arr.Type = type switch 
            {
                "Bit"    => "stack.bit",
                "Binary" => "Bin",
                "Ascii"  => "stack.ascii",
                "Hex"    => "stack.hex",
                _        => "NST", // No Such Type
            };
            arr.content = content == null ? new() { 0 } : content;
            arr.Length = arr.content.Count;
            return arr;
        }
        public static Var NewVar(string type, string? content = null) {
            var var = new Var();
            
            var.Type = type switch 
            {
                "Bit"    => "bit",
                "Binary" => "Bin",
                "Ascii"  => "ascii",
                "Hex"    => "hex",
                _        => "NST", // No Such Type
            };
            var.content = content == null ? 0 : int.Parse(content);
            var.Length = var.content.ToString().Length;
            return var;
        }
        public static int Length(Arr stack) => stack.Length;

        public static void Push(Arr array, Var val) { 
            if(array.Type == "Binary" && val.Type == "Binary") array.content.Add(val.content);
        }
        public static void Push(Arr stack, Arr val)
        {
            if(stack.Type == "Binary") {
                if (val.GetType().Name == "Boolean[]") {
                    foreach (var x in val.content) {
                        if (x == 1) stack.content.Add(1);
                        else stack.content.Add(0);
                    }
                }
            }
        }
    }
    public class Arr {
        public string Type = "";
        public List<int> content = new List<int>();
        public int Length = 0;
    }
}
