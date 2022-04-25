using System.IO;

namespace FolderScan 
{
    class Program
    {
        static void Main(string[] args)
        {
            // Значения параметров по-умолчанию.
            string directory = Path.GetFullPath(@".");
            string outputFile = $"sizes-{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
            bool quiteOn = false;
            bool humanreadOn = false;

            ParametersProcessor.ProcessParameters(args, ref directory, ref outputFile, ref quiteOn, ref humanreadOn);

            Notifier.StartProcess();

            List<string> dirTree = DirectoriesTreeCreator.GetFormattingDirectoriesTree(directory, humanreadOn);
            DirectoriesTreePrinter.PrintDirectoriesTree(dirTree, outputFile, quiteOn);

            Notifier.EndProcess();
        }
    }
}
