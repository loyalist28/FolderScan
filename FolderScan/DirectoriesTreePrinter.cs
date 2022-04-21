using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderScan
{
    internal static class DirectoriesTreePrinter
    {
        // Выводит список директорий 'directoriesTree' в указаннвй файл 'outputFile' и стандартный поток вывода в соответствии с признаком вывода сообщений 'quite'.
        // Если 'quite' имеет значение true, то выводит список только в файл.
        public static void PrintDirectoriesTree(List<string> directoriesTree, string outputFile, bool quite)
        {
            PrintListToFile(directoriesTree, outputFile);

            if (!quite)
            {
                PrintListStandOutput(directoriesTree);
            }
        }

        // Выводит список 'listForPrint' в файл 'file' построчно.
        static void PrintListToFile(List<string> listForPrint, string file)
        {
            using (StreamWriter sw = File.CreateText(file))
            {
                sw.AutoFlush = true;

                foreach (string item in listForPrint)
                {
                    sw.WriteLine(item);
                }
            }
        }

        // Выводит список 'listForPrint' в стандартный поток вывода построчно.
        static void PrintListStandOutput(List<string> listForPrint)
        {
            foreach (string item in listForPrint)
            {
                Console.WriteLine(item);
            }
        }
    }
}
