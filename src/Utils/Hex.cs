using static Velox.Utils;

namespace Velox
{
    public static class Hex
    {
        static Parser parser = new Parser();
        
        private static void VerboseLog(string msg) => parser.VerboseLog(msg, 2);
        
        public static void ModAt(List<int> list, int pointer, int incrementBy = 0)
        {
            Hex.VerboseLog("Checking pointer location.");
            if (list.Count > pointer)
            {
                list[pointer] += incrementBy;
            }
            else
            {
                Hex.VerboseLog($"Pointer Out Of Bounds. Target Location: {pointer}. Adding more indexes till target index reached...");
                for (int i = list.Count - 1; i < pointer; i++)
                {
                    list.Add(0);
                    if (list.Count > pointer)
                    {
                        Hex.VerboseLog($"Done. Incrementing Address {pointer} by {incrementBy}");
                        list[pointer] += incrementBy;
                    }
                }
            }
        }
        public static void SetAt(Arr stack, int pointer, int setTo = 0) {
            Hex.VerboseLog("Checking pointer location.");
            if (stack.content.Count > pointer) {
                stack.content[pointer] = setTo;
            }
            else {
                Hex.VerboseLog($"Pointer Out Of Bounds. Target Location: {pointer}. Adding more indexes till target index reached...");
                for (int i = stack.content.Count - 1; i < pointer; i++)
                {
                    stack.content.Add(0);
                    Hex.VerboseLog($"Done. Setting Address {pointer} to {setTo}");
                    if (stack.content.Count > pointer) stack.content[pointer] = setTo;
                }
            }
        }

        public static int LookAt(List<int> list, int pointer)
        {
            Hex.VerboseLog("Checking pointer location.");
            if (list.Count > pointer) return list[pointer];
            else parser.AddError("PointerError", "POOR (Pointer Out of Range)");
            return 0;
        }
        public static int JumpTo(int[] stack, int pointer) { 
            Hex.VerboseLog("Checking pointer location."); 
            return stack.Length > pointer ? pointer :  -1; 
        }
        public static string TwosComplement(string hx) { 
            Hex.VerboseLog($"Applying \"Two's Complement\" to {hx}"); 
            return string.Format("{0:X}", (ushort)(~Convert.ToUInt32(hx.Remove(0, 2), 32) + 1));
        }

        public static int ToNumber(string hex) { 
            Hex.VerboseLog($"Converting {hex} to integer");
            return Convert.ToInt32(hex, 16);
        }        
        
        
        public static bool IsConvertableToNumber(string hex) { 
            Hex.VerboseLog("Checking Conversion-ability..."); 
            return Convert.ToInt32(hex, 16) == null ? false : true;
        }
    }
}