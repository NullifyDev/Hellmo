namespace Hellmo {
    public static class Files {
        public static string Read(string path) => File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path));
    }
}