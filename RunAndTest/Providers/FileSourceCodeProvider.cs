using System.IO;

namespace RunAndTest.Providers
{
    public class FileSourceCodeProvider : ISourceCodeProvider
    {
        public string FilePath { get; set; }
        public string SourceCode => File.ReadAllText(FilePath);
    }
}