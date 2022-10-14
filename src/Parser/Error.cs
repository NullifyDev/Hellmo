namespace Velox {
    public class Errors {
        List<Token> token = new();
        
        public Errors(List<Token> tokens) {
            this.token = tokens;
        } 

        private static List<Error> errors = new();
        public static void PrintError(string message) => Utils.Outputln($"\\R {message}");
        public static void Add(string type, int line, string message) { 
            Error err = new Error();
            err.Type = type;
            err.Line = line;
            err.Message = message;
            errors.Add(err);
        }
    }
    public class Error {
        public string Type = "";
        public string Message = "";
        public int Line = 1; 
    } 
}