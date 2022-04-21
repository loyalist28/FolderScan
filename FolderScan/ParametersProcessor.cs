using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderScan
{
    // Обрабатывает входные параметры и настраивает работу программы в соответствии с этими параметрами.
    internal static class ParametersProcessor
    {
        // Обрабатывает входные параметры приложения.
        //  
        // Параметры:
        //     parameters:
        //         Массив переданных аргументов командной строки.
        //     directoryForScan:
        //         Директория для сканирования.
        //     outputFile:
        //         Файл для вывода информации.
        public static void ProcessParameters(string[] parameters, ref string directoryForScan, ref string outputFile)
        {
            string[] permissibleParameters = new string[] { "-q", "--quite", "-p", "--path", "-o", "--output", "-h", "--humanread" };

            if (parameters.Length != 0)
            {
                // Проверяем не превышенно ли максимальное количество параметров.
                if (parameters.Length > 6)
                {
                    ErrorsHandler.PrintErrorMessage(Errors.OutOfRangeParametersList);

                    Environment.Exit(1);
                }

                // Проверяем отсутствие дублирования параметров.
                bool ununiqueParametersFound = CheckParametersUnique(parameters, permissibleParameters);

                if (ununiqueParametersFound)
                {
                    Environment.Exit(1);
                }

                for (int i = 0; i < parameters.Length; i++)
                {
                    // Обработка входного параметра -p (--path).
                    try
                    {
                        if (parameters[i] == "-p" || parameters[i] == "--path")
                        {
                            // Указан ли путь в одинарных кавычках после параметра .
                            if (System.Text.RegularExpressions.Regex.IsMatch(parameters[i + 1], "'*'"))
                            {
                                string clearPath = parameters[i + 1].Trim(new char[] { '\u0027' });
                                // Является ли указанный путь существующей директорией.
                                if (Directory.Exists(clearPath))
                                {
                                    directoryForScan = clearPath;

                                    i++;
                                }
                                else
                                {
                                    ErrorsHandler.PrintErrorMessage(Errors.NonexistentDirectory);

                                    Environment.Exit(1);
                                }
                            }
                            // Не указан путь к директории после параметра -p (--path).
                            else
                            {
                                ErrorsHandler.PrintErrorMessage(Errors.NotIntroducedPathToDirectory);

                                Environment.Exit(1);
                            }
                        }
                    }
                    // Если в массиве параметров только один параметр: -p(--path), и не указан путь к сканируемой директории.
                    catch (IndexOutOfRangeException)
                    {
                        ErrorsHandler.PrintErrorMessage(Errors.NotIntroducedPathToDirectory);

                        Environment.Exit(1);
                    }

                    // Обработка входного параметра -o (--output)
                    try
                    {
                        if (parameters[i] == "-o" || parameters[i] == "-output")
                        {
                            string clearPath = parameters[i + 1].Trim(new char[] { '\u0027' });

                            // Указан ли путь в одинарных кавычках после параметра.
                            if (System.Text.RegularExpressions.Regex.IsMatch(parameters[i + 1], "'*'"))
                            {
                                // Если указанный путь - директория, значит имя файла не указано.
                                if (Directory.Exists(clearPath))
                                {
                                    Console.WriteLine(Errors.NotIntroducedFileName);
                                }
                                else
                                {
                                    try
                                    {
                                        // Проверка на исключения связанные с путем к файлу.
                                        using (File.CreateText(clearPath)) { }

                                        // Является ли указанный файл вывода текстовым.
                                        if (System.Text.RegularExpressions.Regex.IsMatch(clearPath, @".txt*$"))
                                        {
                                            outputFile = clearPath;
                                        }
                                        else
                                        {
                                            ErrorsHandler.PrintErrorMessage(Errors.NonTxtFile);
                                        }
                                    }
                                    // Нет прав на доступ к файлу.
                                    catch (UnauthorizedAccessException)
                                    {
                                        Console.WriteLine(Errors.UnauthorizedAccess);
                                    }
                                    // Неопустимый формат пути или несуществующий путь к файлу.
                                    catch
                                    {
                                        Console.WriteLine(Errors.UnacceptableOrMissingPath);
                                    }
                                }
                            }
                            // Путь к файлу не указан после параметра -o (--output).
                            else
                            {
                                ErrorsHandler.PrintErrorMessage(Errors.NotIntroducedPathToFile);

                                Environment.Exit(1);
                            }
                        }
                    }
                    // Если в массиве параметров только один параметр: -o (--output), и не указан путь к файлу вывода.
                    catch (IndexOutOfRangeException)
                    {
                        ErrorsHandler.PrintErrorMessage(Errors.NotIntroducedPathToFile);

                        Environment.Exit(1);
                    }
                }
            }
        }

        // Ищет допустимые параметры командной строки в массиве переданных параметров и проверяет их уникальность.
        //  
        // Параметры:
        //     parameters:
        //         Массив переданных аргументов командной строки.
        //      
        //     permissibleParameters:
        //         Массив допустимых параметров командной строки.
        //
        // Возврат:
        //     Признак того, были ли переданны одинаковые параметры.
        public static bool CheckParametersUnique(string[] parameters, string[] permissibleParameters)
        {
            string[] foundArgsArr;
            bool ununiuqueFound = false;

            for (int i = 0; i < permissibleParameters.Length; i += 2)
            {
                foundArgsArr = Array.FindAll(parameters, arg => arg == permissibleParameters[i] || arg == permissibleParameters[i + 1]);

                if (foundArgsArr.Length > 1)
                {
                    ErrorsHandler.PrintErrorMessageForUniqueParCheck(permissibleParameters[i], permissibleParameters[i + 1]);

                    ununiuqueFound = true;
                }
            }

            return ununiuqueFound;
        }
    }
}
