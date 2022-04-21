using System.IO;

namespace FolderScan 
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryForScan = ".";
            string outputFile = $"sizes-{ DateTime.Now.ToString("yyyy-MM-hh")}.txt";

            FileSystemEntireCreator FSECreator = new FileSystemEntireCreator();
            List<string> FSysEntire = FSECreator.GetFSystemEntire(directoryForScan);

            DirectoriesTreeCreator dtCreator = new DirectoriesTreeCreator();
            List<string> directoriesTree = dtCreator.CreateDirectoriesTree(FSysEntire);

            DirectoriesTreePrinter.PrintDirectoriesTree(directoriesTree, outputFile, true);
        }
    }
}
