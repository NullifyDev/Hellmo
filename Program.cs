using System;
using System.Collections.Generic;
using static Hellmo.Terminal;
using Hellmo;
// Hellmo.Files;
namespace Hellmo {
    class Program {
        static void Main(string[] args) {
            Console.Clear();
            string[] script;
            int p = 0;
            Outputln("\\C Elmo Welcomes you to Hell! \nEnjoy your stay while you can! Haha!");
            Outputln("[Please enter your script]");
            string input = Console.ReadLine();
            if (input.StartsWith(">")) {
                script = input.Split(' ');
            }
            else if (input.StartsWith("file:")) {
                string filename = input.Remove(0,5);
                if (!File.Exists(filename)) {
                    Outputln("\\R Error: The file \""+ input.Remove(0,5) + "\" was not found.");
                    Environment.Exit(0);
                }
                string contents = Files.Read(filename);
                script = contents.Replace("\n", " ").Split(" ");
                List<int> stack = new List<int> { 0 };
                foreach(string x in script) {
                    if (x.StartsWith("\n")) x.Remove(0,2);
                    switch(x) {
                        case "0x00":
                            Output("[");
                            foreach (var y in stack) { Output(y.ToString());}
                            Outputln("]");
                            Environment.Exit(0);
                            break;                            
                        case "0x01":
                            // if (p == stack.Length) {  }
                            p++;
                            break;                            
                        case "0x02":
                            p--;
                            break;                            
                        case "0x03":
                            Utils.ModAt(stack, p, 1);
                            break;
                        case "0x04":
                            Utils.ModAt(stack, p, -1);
                            break; 
                        case "0x05":
                            int jmpTo = stack[p+1]; // Jump if condition is true/matches
                            int pos   = stack[p+2]; // Position of target bit we are trying to check 
                            int bit   = stack[p+3]; // what we are looking for within the pos

                            if (stack[pos] == bit) {
                                p = jmpTo;
                            } else {
                                p += 4;
                            }
                            break;                                
                    }
                    // Outputln("["+x+"]: "+p.ToString() + ": " + stack[p].ToString());
                }
                Output("[");
                foreach (var y in stack) { Output(y.ToString());}
                Outputln("]");
            } else {
                Outputln("\\R Error: No script found."); Environment.Exit(0);
            }
        }
    }
} 