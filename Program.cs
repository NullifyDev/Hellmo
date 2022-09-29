using System;
using static Hellmo.Terminal;
// Hellmo.Files;
namespace Hellmo {
    class Program {
        static void Main(string[] args) {
            Console.Clear();
            string[] script;
            int p = 0;
            int[] stack = new int[Int16.MaxValue-1];
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
                script = contents.Replace("\r\n", " ").Split(' ');
                foreach(string x in script) {
                    switch(x) {
                        case "0x00":
                             Outputln("["+x+"]: Exit");
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
                            stack[p] += 1;
                            break;                            
                        case "0x04":
                            stack[p] -= 1;
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
                    Outputln("["+x+"]: "+p.ToString() + ": " + stack[p].ToString());
                }
                foreach (var x in stack) { if (x > 0) { Output(x.ToString(), 1); } }
            } else {
                Outputln("\\R Error: No script found."); Environment.Exit(0);
            }
        }
    }
} 


/*
switch (program[p]) {
            case 0x00:
                outputln("0x00");
                exit(0);
                break;
            case 0x01:
                outputln("0x01");
                p++;
                break;
            case 0x02:
                outputln("0x02");
                p--;
                break;
            case 0x03:
                outputln("0x03");
                stack[p]++;
                break;
            case 0x04:
                outputln("0x04");
                stack[p]--;
                break;
            case 0x05:
                outputln("0x05");
                //  Set the variable into the value of the specified position relative to current position   
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
*/