using System.Reflection;
using System.Data;
using System;

namespace Hellmo
{
    public static class Terminal
    {
        public static void Error(string message) => Console.WriteLine($"\u001b[31m{message}\u001b[0m");
        public static void Warning(string message) { 
            Console.WriteLine($"\u001b[33m{message}\u001b[0m"); 
            Environment.Exit(0); 
        }
        public static void Output(string message = "", int newLine = 0)
        {
            string[] msg = message.Split(' ');
            string line = "";
            for (int i = 0; i < msg.Count(); i++)
            {
                line += msg[i] switch
                {
                    "\\R" => "\u001b[31m",
                    "\\G" => "\u001b[32m",
                    "\\Y" => "\u001b[33m",
                    "\\B" => "\u001b[34m",
                    "\\M" => "\u001b[35m",
                    "\\C" => "\u001b[36m",
                    "\\W" => "\u001b[37m",
                    "\\b" => "\u001b[1m",
                    "\\u" => "\u001b[4m",
                    "\\r" => "\u001b[7m",
                    "\\Reset" => "\u001b[0m",
                    "\\BW" => "\u001b[47m",
                    _ => $"{msg[i]}"
                };
                if (i < msg.Count()) { line += " "; }
            }
            if (newLine > 0)
            {
                for (int i = 1; i < newLine; i++) if (i < newLine-1) line += "\n";
                Console.WriteLine($"{line}");
            }
            else Console.Write($"{line}");
        }
        public static void Outputln(string msg) => Terminal.Output(msg, 1);
    }

}