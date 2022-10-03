using System;
using System.Collections.Generic;
using static Hellmo.Terminal;

namespace Hellmo {
    public static class Utils {
        public static void ModAt(List<int> list, int targetIndex, int incrementBy) {
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
    }
}