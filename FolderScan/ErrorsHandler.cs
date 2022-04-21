using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderScan
{
    internal class ErrorsHandler
    {
        // Выводит цветное сообщение об ошибке в консоль, выбираемое в соответствии с типом возникшей ошибки.
        // 
        // Параметры:
        //    error: 
        //        Тип возникшей ошибки
        public static void PrintErrorMessage(Errors error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nFolderScan: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            switch (error)
            {
                case Errors.OutOfRangeParametersList:
                    Console.WriteLine("Превышено количество параметров.");
                    break;
                case Errors.NonexistentDirectory:
                    Console.WriteLine("Введена несуществующая директория.");
                    break;
                case Errors.NotIntroducedPathToDirectory:
                    Console.WriteLine("Не введен путь к сканируемой папке.\nВведите относительный или абсолютный путь к существующей директории после параметра '-p' ('--path'), используя одинарные кавычки.\n" +
                                    "Пример: -p 'C:\\path\\to\\folder'.");
                    break;
                case Errors.NotIntroducedPathToFile:
                    Console.WriteLine("Не введен путь к файлу вывода.\nВведите относительный или абсолютный путь к текстовому файлу после параметра '-o'('--output'), используя одинарные кавычки.\n" +
                                    "Пример: -p 'C:\\path\\to\\file.txt'.");
                    break;
                case Errors.NotIntroducedFileName:
                    Console.WriteLine("В параметре '-o' (-output') не указано имя файла вывода.");
                    break;
                case Errors.UnauthorizedAccess:
                    Console.WriteLine("Отсутсвует разрешение на доступ по пути, указанному в параметре '-o' ('-output')");
                    break;
                case Errors.UnacceptableOrMissingPath:
                    Console.WriteLine("В параметре '-o' '-output' указан недопустимый или отсутствующий в системе путь");
                    break;
                case Errors.NonTxtFile:
                    Console.WriteLine("Указанный файл не является текстовым. Файл выводак должен иметь расширение .txt");
                    break;                    
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        // Выводит сообщение об ошибке при дублировании параметров из командной строки, основанное на параметре, вызввавшем ошибку.
        // 
        // Параметры:
        //    shortParam: 
        //        Сокращенная версия параметра
        // 
        //    fullParam: 
        //        Полная версия параметра
        public static void PrintErrorMessageForUniqueParCheck(string shortParam, string fullParam)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nFolderScan: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.WriteLine($"Список параметров должен содержать только один параметр '{shortParam}' ('{fullParam}')");

            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    // Перечисление типов ошибок.
    enum Errors
    {
        OutOfRangeParametersList,
        NonexistentDirectory,
        NotIntroducedPathToDirectory,
        NotIntroducedPathToFile,
        NotIntroducedFileName,
        UnauthorizedAccess,
        UnacceptableOrMissingPath,
        NonTxtFile
    }
}
