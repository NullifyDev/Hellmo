using System;
using System.Collections.Generic;
using static Hellmo.Terminal;

namespace Hellmo {
    public static class Utils {
        public static void ModAt(List<int> list, int targetIndex, int incrementBy = 0) {
            if (list.Count > targetIndex) {
                list[targetIndex] += incrementBy;
            } else {
                for (int i = list.Count-1; i < targetIndex; i++) {
                    list.Add(0);
                    if(list.Count > targetIndex) { 
                        list[targetIndex] += incrementBy;
                    }
                }
            }
        }
        public static int LookAt(List<int> list, int targetIndex) {
            if (list.Count > targetIndex) {
                return list[targetIndex];
            } else Error("Error: Stack is too small for this task.");
            return 0;
        }
        public static int JumpTo(List<int> list, int targetIndex) {
            if (list.Count < targetIndex) {
                return targetIndex;
            } else {
                for (int i = list.Count-1; i < targetIndex; i++) {
                    list.Add(0);
                    if(list.Count > targetIndex) { 
                        return targetIndex;
                    }
                }
            }
            Error($"JumpTo(): Something Went Wrong. Here are the values: \nLength: {list.Count} \nRequested Index: {targetIndex}");
            return 0;
        }
        public static int JumpTo(string[] arr, int targetIndex) {
            if (arr.Length > targetIndex) {
                return targetIndex; 
            } else {
                    if(arr.Length > targetIndex) { 
                        return targetIndex;
                    }
            }
            Error($"JumpTo(): Something Went Wrong. Here are the values: \nLength: {arr.Length} \nRequested Index: {targetIndex}");
            return 0;
        }
    }
}