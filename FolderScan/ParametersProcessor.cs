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
        //     quiteOn:
        //         Признак включения режима quite.
        //     humanreadOn:
        //         Признак включения режима humanread.
        public static void ProcessParameters(string[] parameters, ref string directoryForScan, ref string outputFile, ref bool quiteOn, ref bool humanreadOn)
        {
            string[] permissibleParameters = new string[] { "-?", "--help", "-q", "--quite", "-p", "--path", "-o", "--output", "-h", "--humanread" };
            
            if (parameters.Length != 0)
            {
                // Обработка параметра -? (--help).
                if (parameters[0] == "-?" || parameters[0] == "--help")
                {
                    Notifier.GetHelp();

                    Environment.Exit(1);
                }

                // Проверяем не превышенно ли максимальное количество параметров.
                if (parameters.Length > 6)
                {
                    Notifier.PrintErrorMessage(Errors.OutOfRangeParametersList);

                    Environment.Exit(1);
                }

                // Проверяем корректность введенных параметров.
                CheckParametersСorrectness(parameters, permissibleParameters);

                // Проверяем отсутствие дублирования параметров.
                CheckParametersUnique(parameters, permissibleParameters);

                for (int i = 0; i < parameters.Length; i++)
                {
                    // Проверка на то, что с остальными параметрами не передан -? (--help).
                    if (parameters[i] == "-?" || parameters[i] == "--help")
                    {
                        Notifier.PrintErrorMessage(Errors.HelpParameterNotAlone);

                        Environment.Exit(1);
                    }

                    // Обработка входного параметра -p (--path).
                    try
                    {
                        if (parameters[i] == "-p" || parameters[i] == "--path")
                        {
                            // Указан ли путь в одинарных кавычках после параметра .
                            if (System.Text.RegularExpressions.Regex.IsMatch(parameters[i + 1], @"'.*'"))
                            {
                                string clearPath = Path.GetFullPath(parameters[i + 1].Trim(new char[] { '\u0027' }));

                                try
                                {
                                    Directory.GetDirectories(clearPath);
                                }
                                catch (UnauthorizedAccessException)
                                {
                                    Notifier.PrintErrorMessage(Errors.UnauthorizedAccessDirectory);

                                    Environment.Exit(1);
                                }
                                catch
                                {
                                    Notifier.PrintErrorMessage(Errors.NotCorrectOrNonexistentDirectory);

                                    Environment.Exit(1);
                                }

                                // Является ли указанный путь существующей директорией.
                                if (Directory.Exists(clearPath))
                                {
                                    directoryForScan = clearPath;

                                    // Пропускаем параметр содержащий путь к папке.
                                    i++;
                                }
                                else
                                {
                                    Notifier.PrintErrorMessage(Errors.NotCorrectOrNonexistentDirectory);

                                    Environment.Exit(1);
                                }
                            }
                            // Не указан путь к директории после параметра -p (--path).
                            else
                            {
                                Notifier.PrintErrorMessage(Errors.NotIntroducedPathToDirectory);

                                Environment.Exit(1);
                            }
                        }
                    }
                    // Если в массиве параметров только один параметр: -p(--path), и не указан путь к сканируемой директории.
                    catch (IndexOutOfRangeException)
                    {
                        Notifier.PrintErrorMessage(Errors.NotIntroducedPathToDirectory);

                        Environment.Exit(1);
                    }

                    // Обработка входного параметра -o (--output).
                    try
                    {
                        if (parameters[i] == "-o" || parameters[i] == "--output")
                        {
                            // Указан ли путь в одинарных кавычках после параметра.
                            if (System.Text.RegularExpressions.Regex.IsMatch(parameters[i + 1], @"'.*'"))
                            {
                                string clearPath = parameters[i + 1].Trim(new char[] { '\u0027' });

                                // Если указанный путь - директория, значит имя файла не указано.
                                if (Directory.Exists(clearPath))
                                {
                                    Notifier.PrintErrorMessage(Errors.NotIntroducedFileName);

                                    Environment.Exit(1);
                                }
                                else
                                {
                                    try
                                    {
                                        // Проверка на исключения связанные с путем к файлу.
                                        using (File.CreateText(clearPath)) { }
                                        File.Delete(clearPath);

                                        // Является ли указанный файл вывода текстовым.
                                        if (System.Text.RegularExpressions.Regex.IsMatch(clearPath, @".txt*$"))
                                        {
                                            outputFile = clearPath;

                                            // Пропускаем параметр содержащий путь к файлу.
                                            i++;
                                        }
                                        else
                                        {
                                            Notifier.PrintErrorMessage(Errors.NonTxtFile);

                                            Environment.Exit(1);
                                        }
                                    }
                                    // Нет прав на доступ к файлу.
                                    catch (UnauthorizedAccessException)
                                    {
                                        Notifier.PrintErrorMessage(Errors.UnauthorizedAccessFile);

                                        Environment.Exit(1);
                                    }
                                    // Неопустимый формат пути или несуществующий путь к файлу.
                                    catch (Exception)
                                    {
                                        Notifier.PrintErrorMessage(Errors.NotCorrectOrNonexistentPathToFile);

                                        Environment.Exit(1);
                                    }
                                }
                            }
                            // Путь к файлу не указан после параметра -o (--output).
                            else
                            {
                                Notifier.PrintErrorMessage(Errors.NotIntroducedPathToFile);

                                Environment.Exit(1);
                            }
                        }
                    }
                    // Если в массиве параметров только один параметр: -o (--output), и не указан путь к файлу вывода.
                    catch (IndexOutOfRangeException)
                    {
                        Notifier.PrintErrorMessage(Errors.NotIntroducedPathToFile);

                        Environment.Exit(1);
                    }

                    // Обработка входного параметра -q (--quite) 
                    if(parameters[i] == "-q" || parameters[i] == "--quite")
                    {
                        quiteOn = true;
                    }

                    // Обработка входного параметра -h (--humanread) 
                    if (parameters[i] == "-h" || parameters[i] == "--humanread")
                    {
                        humanreadOn = true;
                    }
                }
            }
        }

        // Проверяет корректность введенных параметров командной строки.
        //  
        // Параметры:
        //     parameters:
        //         Массив переданных аргументов командной строки.
        //      
        //     permissibleParameters:
        //         Массив допустимых параметров командной строки.
        static void CheckParametersСorrectness(string[] parameters, string[] permissibleParameters)
        {
            bool NonpermissibleParametersExists = false;

            for(int i = 0; i < parameters.Length; i++)
            {
                bool IsPermissibleParameter = false;

                for(int j = 0; j < permissibleParameters.Length; j++)
                {
                    if(parameters[i] == permissibleParameters[j])
                    {
                        IsPermissibleParameter = true;
                        break;  
                    }
                }

                if(IsPermissibleParameter)
                {
                    // Пропускаем параметр содержащий путь к папке или файлу.
                    if ((parameters[i] == "-p" || parameters[i] == "--path") || (parameters[i] == "-o" || parameters[i] == "--output"))
                    {
                        i++;
                    }
                }
                else
                {
                    Notifier.PrintErrorMessageUncorrectParameter(parameters[i]);

                    NonpermissibleParametersExists = true;
                }
            }

            if(NonpermissibleParametersExists) Environment.Exit(1);
        }

        // Ищет допустимые параметры командной строки в массиве переданных параметров и проверяет их уникальность.
        //  
        // Параметры:
        //     parameters:
        //         Массив переданных аргументов командной строки.
        //      
        //     permissibleParameters:
        //         Массив допустимых параметров командной строки.
        public static void CheckParametersUnique(string[] parameters, string[] permissibleParameters)
        {
            string[] foundArgsArr;
            bool ununiuqueFound = false;

            for (int i = 0; i < permissibleParameters.Length; i += 2)
            {
                foundArgsArr = Array.FindAll(parameters, arg => arg == permissibleParameters[i] || arg == permissibleParameters[i + 1]);

                if (foundArgsArr.Length > 1)
                {
                    Notifier.PrintErrorMessageForUniqueParCheck(permissibleParameters[i], permissibleParameters[i + 1]);

                    ununiuqueFound = true;
                }
            }

            if(ununiuqueFound) Environment.Exit(1);
        }
    }
}
