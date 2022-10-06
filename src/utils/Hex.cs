using System;
using static Hellmo.Terminal;

namespace Hellmo {
    public class Hex {
        public static int ToNumber(string[] arr, int index) {
            if (index < arr.Length) {
                return Convert.ToInt32(arr[index], 16);
            };
            Error($"PointerError: POOR (Pointer Out of Range)");
            Environment.Exit(0);
            return 0;
        }
        // public static string ToString(string hex) => (char)Int16.Parse(hex.Remove(0,2), NumberStyles.AllowHexSpecifier);
        public static int ToNumber(List<string> list, int index) {
            if (index < list.Count) { 
                if (list[index].StartsWith("-")) {
                    int num = Convert.ToInt32(list[index].Remove(0, 1), 16) * -1;
                        
                    Outputln(num.ToString());
                    return num;
                }
            };
            Error($"PointerError: POOR (Pointer Out of Range)");
            Environment.Exit(0);
            return 0;
        }
    } 
}
