namespace Hellmo {
    public static class Files {
        public static string Read(string path) { 
            string text = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path));
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(text.Replace("\r\n", "\n"));
            }
            text += "\nEOF";
            return text.Replace("\r\n", "\n");
        }
    }
}