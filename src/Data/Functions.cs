namespace Velox {
    public class Function {

        public int id = new int();
        public List<object>? arguments = new();  
        public List<Token> block = new();

        public static void New(int id, List<Velox.Function> instructions) => Data.Functions.Add(id, instructions);

        public static List<Velox.Function> CallFunction(int id) {
            for (int i = 0; i < Data.Functions.Count(); i++) {
                Console.WriteLine(Data.Functions[id].ToString());
            }
            return null;
        }
    }
}