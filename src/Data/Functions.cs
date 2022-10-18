namespace Hellmo
{
    public class Function
    {

        public int id = new int();
        public List<object>? arguments = new();
        public List<Token> block = new();

        public static void New(int id, List<Hellmo.Function> instructions) => Data.Functions.Add(id, instructions);

        public static List<Hellmo.Function> CallFunction(int id)
        {
            for (int i = 0; i < Data.Functions.Count(); i++)
            {
                Console.WriteLine(Data.Functions[id].ToString());
            }
            return null;
        }
    }
}