namespace Velox {
    class Program {
        private static Velox.Lexer? lexer;
        static void Main(string[] args) {
            Console.Clear();
            string filename = "";
            if (args.Length > 0) {
                string[] Switch = {};
                foreach(string x in args) {
                    if (x.StartsWith("/")){
                        Switch.Append(x);
                    } else if (x.EndsWith(".hm")) {
                        filename = x;
                    } else {
                        Utils.Error($"[Argument Error]: Invalid argument: {x}");
                    }
                }
                if (!File.Exists(filename)) Utils.Error($"Error: Couldn't find {filename} in the current directory.");

                lexer = new Velox.Lexer(File.ReadAllText(args[0]));
                var tokens = lexer.Scan();

                Parser parser = new Parser(tokens);
                parser.Parse(tokens);
            }
        }
    }
}