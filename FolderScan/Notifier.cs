using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderScan
{
    // Класс для формирования сообщений в консоли. 
    internal static class Notifier
    {

        // Выводит справку по программе.
        public static void GetHelp()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nFolderScan: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.WriteLine("\nПрограмма для формирования и вывода содержимого директории рекурсивно с указанием размеров элементов файловой системы в текстовый файл и консоль." +
                "\nВсе пути в параметрах задаются в одинарных кавычках." +
                "\nФайл вывода должен быть формата.txt.Если файла по указанному пути не существует, то он будет создан." +
                "\nЕсли параметры не указаны, то используются значения по - умолчанию." +
                "\n\nПараметры:");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("    -p(--path) 'path_to_directory'");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" - путь к папке, содержимое которой нужно вывести." + 
                "\n    Путь после параметра указывается в одинарных кавычках." +
                "\n    По-умолчанию текущая папка вызова программы.");
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n\n    -o(--output) 'path_to_file'");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" - путь к текстовому файлу, куда будет выведен результат программы." + 
                "\n    Путь после параметра указывается в одинарных кавычках." +
                "\n    По-умолчанию будет создан файл с именем формата sizes-YYYY-MM-DD.txt в текущей папке вызова программы.");
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n\n    -q(--quiet)");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" - признак вывода сообщений в стандартный поток вывода." + 
                "\n    Если указан, то вывод в консоль произведен не будет.Только в файл." +
                "\n    По-умолчанию результат выводится и в файл, и в консоль.");
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n\n    -h(--humanread)");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" - признак формирования размеров файлов в удобочитаемой форме." + 
                "\n    Байты будут переведены в килобайты, мегабайты или гигабайты в зависимости от размера элемента файловой системы." +
                "\n    По-умолчанию размеры элементов указываются в байтах.");
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n\n    -?(--help)");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" - справка по программе");
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.ForegroundColor = ConsoleColor.White;
        }

        // Выводит сообщение об ошибке в консоль, выбираемое в соответствии с типом возникшей ошибки.
        // 
        // Параметры:
        //    error: 
        //        Тип возникшей ошибки
        public static void PrintErrorMessage(Errors error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nFolderScan ошибка: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            switch (error)
            {
                case Errors.OutOfRangeParametersList:
                    Console.WriteLine("Превышено количество параметров.");
                    break;
                case Errors.HelpParameterNotAlone:
                    Console.WriteLine("Параметр -?(--help) должен быть указан без других параметров.\nПример корректного использования параметра: FolderScan -?");
                    break;
                case Errors.NotCorrectOrNonexistentDirectory:
                    Console.WriteLine("Введен некорректный или указывающий на несуществующую директорию путь в параметре -p (--path).");
                    break;
                case Errors.NotIntroducedPathToDirectory:
                    Console.WriteLine("Не введен или введен без одинарных кавычек путь к сканируемой папке.\nВведите относительный или абсолютный путь к существующей директории после параметра '-p' ('--path'), используя одинарные кавычки.\n" +
                                    "Пример: -p 'C:\\path\\to\\folder'.");
                    break;
                case Errors.NotIntroducedPathToFile:
                    Console.WriteLine("Не введен или введен без одинарных кавычек путь к файлу вывода.\nВведите относительный или абсолютный путь к текстовому файлу после параметра '-o'('--output'), используя одинарные кавычки.\n" +
                                    "Пример: -o 'C:\\path\\to\\file.txt'.");
                    break;
                case Errors.NotIntroducedFileName:
                    Console.WriteLine("В параметре -o (--output) не указано имя файла вывода.");
                    break;
                case Errors.UnauthorizedAccessDirectory:
                    Console.WriteLine("Отсутсвует разрешение на доступ по пути, указанному в параметре '-p' ('--path')");
                    break;
                case Errors.UnauthorizedAccessFile:
                    Console.WriteLine("Отсутсвует разрешение на доступ по пути, указанному в параметре '-o' ('--output')");
                    break;
                case Errors.NotCorrectOrNonexistentPathToFile:
                    Console.WriteLine("Введен некорректный или указывающий на несуществующую директорию путь в параметре '-o' ('--output').");
                    break;
                case Errors.NonTxtFile:
                    Console.WriteLine("Указанный файл не является текстовым. Файл вывода должен иметь расширение .txt");
                    break;                    
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        // Выводит сообщение об ошибке при дублировании параметров из командной строки, основанное на параметре, вызввавшем ошибку.
        // 
        // Параметры:
        //    shortParam: 
        //        Сокращенная версия параметра.
        // 
        //    fullParam: 
        //        Полная версия параметра.
        public static void PrintErrorMessageForUniqueParCheck(string shortParam, string fullParam)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nFolderScan ошибка: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.WriteLine($"Список параметров должен содержать только один параметр '{shortParam}' ('{fullParam}')");

            Console.ForegroundColor = ConsoleColor.White;
        }

        // Выводит сообщение об ошибке при вводе некорретных параметров командной строки.
        // 
        // Параметры:
        //    uncorrectParameter: 
        //        Введенный некорректный параметр.
        public static void PrintErrorMessageUncorrectParameter(string uncorrectParameter)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nFolderScan ошибка: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.WriteLine($"Введен некорректный параметр '{uncorrectParameter}'");

            Console.ForegroundColor = ConsoleColor.White;
        }

        // Выводит сообщение о начале работы программы.
        public static void StartProcess()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nFolderScan:");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Сканирование выполняется, ждите.");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine();
        }

        // Выводит сообщение об окончании работы программы.
        public static void EndProcess()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nСканирование завершено.");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    // Перечисление типов ошибок для метода PrintErrorMessage(Errors error).
    enum Errors
    {
        OutOfRangeParametersList,
        HelpParameterNotAlone,
        NotCorrectOrNonexistentDirectory,
        NotIntroducedPathToDirectory,
        NotIntroducedPathToFile,
        NotIntroducedFileName,
        UnauthorizedAccessFile,
        UnauthorizedAccessDirectory,
        NotCorrectOrNonexistentPathToFile,
        NonTxtFile
    }
}
