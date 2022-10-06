using static Hellmo.Terminal;

namespace Hellmo {
    public static class Parser {
        public static void Parse(string[] script) {
            List<int> stack = new List<int> { };
            int p = 0, i = 0;
            while (true) {
                string x = script[i];
                if (x.StartsWith("\n")) x.Remove(0,2);
                switch(script[i].ToLower()) {
                    case "eof":
                        Environment.Exit(0);
                        break;

                    case "0x0b":
                        Environment.Exit(0);
                        break;

                    case "0x01": // Move Pointer Up
                        p++;
                        i++;
                        break;

                    case "0x02": // Move Pointer Down
                        p--;
                        i++;
                        break;

                    case "0x03": // In the current script, Jump to this instruction 
                        if (script[i+1] == null) Error("Error: Instruction destination cannot be null");
                        i = Utils.JumpTo(script, Hex.ToNumber(script, i+1));
                        break;

                    case "0x04": // In the stack, jumpt to this address
                        if (script[i+1] == null) Error("TypeError: Target pointer destination cannot be null");
                        p = Hex.ToNumber(script, i+1);
                        break;

                    case "0x05": // Increment current address by 1
                        Utils.ModAt(stack, p, 1);
                        i++;
                        break;

                    case "0x06": // Decrement current address by 1
                        Utils.ModAt(stack, p, -1);
                        i++;
                        break;

                    case "0x07": // Set this address to this value
                        Utils.ModAt(stack, Hex.ToNumber(script, i+1), Hex.ToNumber(script, i+2));
                        i+=3;
                        break;

                    case "0x08": // In the script, Print everything within "" 
                        if (script[i+1] == "stack") {
                            Output("[ "); 
                            if (stack.Count > 0) {
                                foreach(int y in stack) { 
                                    Output(y.ToString() + " "); 
                                } 
                            }
                            Outputln("]"); 
                            i++;
                            break;
                        }

                        if (script[i+1].StartsWith("\"")) { 
                            bool isString = false;
                            string line = "";
                            int e = 1;
                            isString = true; 
                            line += script[i+e].Remove(0,1) + " "; 
                            e++;
                            while (isString) {
                                if (script[i+e].EndsWith('\"')) {
                                    line += script[i+e].Remove(script[i+e].Length-1, 1);
                                    break;
                                } else {
                                    line += script[i+e] + " ";
                                    e++;
                                }
                            }
                            Outputln(line);
                            i += line.Split(" ").Length;
                        }
                        break;

                    case "0x09":

                        // if (script[i+1] == null || script[i+2] == null || script[i+3] == null) Error($"Error: 0x05: Arguments of 0x05 cannot be null");
                        
                        if (script[i+1] == null) Error($"Error: The target adress cannot be null");
                        if (script[i+2] == null) Error($"Error: The target value cannot be null");
                        if (script[i+3] == null) Error($"Error: Destination of If statement cannot be null");
                        
                        int pos = Hex.ToNumber(script, i+1); // Position of target bit we are trying to check 
                        int bit = Hex.ToNumber(script, i+2); // what we are looking for within the pos
                        int jmpTo = Hex.ToNumber(script, i+3); // Jump if condition is true/matches

                        if (stack[pos] == bit) {
                            i = jmpTo;
                        } else {
                            i += 4;
                        }
                        break;

                    default: 
                        i++; 
                        break;
                }
            }
        }
    }
}
