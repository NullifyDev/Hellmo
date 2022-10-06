using System;
using System.Collections.Generic;
using static Hellmo.Terminal;
using Hellmo;
// Hellmo.Files;
namespace Hellmo {
    class Program {
        static void Main(string[] args) {
            // Console.Clear();
            if (args.Length == 0) {
                Outputln("[Please enter your script]");
                string? input = Console.ReadLine();
                if (input.StartsWith(">")) Parser.Parse(input.Split(' '));
                else if (input.StartsWith("file:")) {
                    Start(input);
                } else Error("Error: No script found.");
            } else {
                if (args[0].StartsWith("file:")) {
                    Start(args[0]);
                } else Error("Error: No script found.");
            }
        }

        private static void Start(string path) {
            string filename = path.Remove(0,5);
            if (!File.Exists(filename)) Error("Error: The file \""+ path.Remove(0,5) + "\" was not found.");
            string[] script = Files.Read(filename).Replace("\n", " ").Split(" ");
            Parser.Parse(script);
        }
    }
} 