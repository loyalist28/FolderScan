using System.Text.RegularExpressions;

namespace FolderScan
{
    // Статический класс, определяет ряд методов для получения форматированного дерева директорий и файлов относительно какой либо директории.
    internal static class DirectoriesTreeCreator
    {
        // Форматирует переданное дерево директорий и файлов.
        //
        // Параметры:
        //   targetDirectory:
        //     Путь к целевой папке сканирования.
        //
        // Возврат:
        //     Форматированное дерево директорий.
        public static List<string> GetFormattingDirectoriesTree(string targetDirectory, bool humanreadBytes)
        {
            List<string> rawDirectoriesTree = GetDirectoriesTree(targetDirectory);
            List<string> directoriesTreeFormating = new List<string>();

            // Переменные для формирования отступов.
            string firstDirectory = rawDirectoriesTree[0].Substring(0, rawDirectoriesTree[0].LastIndexOf("(") - 1);
            string relativePath; // Путь элемента дерева относительно самой первой директории дерева для вычисления уровня вложености.
            string indent = "";
            int level = 0;

            foreach (string element in rawDirectoriesTree)
            {
                relativePath = element.Substring(firstDirectory.Length);

                level = relativePath.Split(@"\").Length;

                // Если первая директория, то отступ "особый".
                if (level == 1)
                {
                    directoriesTreeFormating.Add($"-{element.Substring(element.LastIndexOf(@"\") + 1)}");
                }
                else
                {
                    for (int i = 1; i < level; i++)
                    {
                        indent += "--";
                    }

                    directoriesTreeFormating.Add($"{indent}{element.Substring(element.LastIndexOf(@"\") + 1)}");
                }

                indent = "";
            }

            if (humanreadBytes)
            {
                long bytesValue;
                string convertingBytesValue;

                for (int i = 0; i < directoriesTreeFormating.Count; i++)
                {
                    bytesValue = Int64.Parse(Regex.Match(directoriesTreeFormating[i], @"\((\d+) \w+\)$").Groups[1].Value);
                    convertingBytesValue = ConvertBytes(bytesValue);

                    directoriesTreeFormating[i] = Regex.Replace(directoriesTreeFormating[i], @"\(\d+ \w+\)$", $"({convertingBytesValue})");
                }
            }

            return directoriesTreeFormating;
        }

        // Формирует и возвращает дерево директорий и файлов относительно переданной директории с указанием размеров элементов дерева.
        //
        // Параметры:
        //   targetDirectory:
        //     Путь к целевой папке сканирования.
        //
        //   humanreadBytes:
        //     Признак того, что размеры файлов и папок нужно формировать в сконвертированной форме.
        //   
        // Возврат:
        //     Дерево директорий и файлов относительно переданной директории.
        static List<string> GetDirectoriesTree(string targetDirectory)
        {
            List<string> directoryContent = new List<string>();

            bool IsInaccessible = false; // Флаг недоступности для чтения директории.
            long directorySize = 0;

            directoryContent.Add(targetDirectory);

            string[] contentFolders = new string[] { };

            try
            {
                contentFolders = Directory.GetDirectories(targetDirectory);
            }
            catch(UnauthorizedAccessException)
            {
                directoryContent[0] = $"{directoryContent[0]} - нет разрешения на доступ к директории.";

                IsInaccessible = true;
            }

            if(!IsInaccessible)
            {
                foreach (string folder in contentFolders)
                {
                    // Список вложенных элементов с родительской папкой.
                    List<string> subList = GetDirectoriesTree(folder);

                    // Получение строки с размером подпапки.
                    string subfolderSizeString = Regex.Match(subList[0], @"\((\d+) \w+\)$").Groups[1].Value;

                    // Пробуем преобразовать в число для прибавления к размеру текущей папки.
                    // Если к подпапке небыл получен доступ, то ее размер неизвестен и учитан не будет.
                    bool subfolderSizeExists = Int64.TryParse(subfolderSizeString, out long subfolderSize);
                    if(subfolderSizeExists)
                    {
                        // Получение размера подпапки и прибавление к размеру текущей папки.
                        directorySize = subfolderSize;
                    }

                    // Добавляем к текущему списку вложеных элементов текущей папки.
                    directoryContent.AddRange(subList);
                }

                string[] contentFiles = Directory.GetFiles(targetDirectory);

                foreach (string file in contentFiles)
                {
                    long fileSize = new FileInfo(file).Length;

                    directorySize += fileSize;

                    directoryContent.Add($"{file} ({fileSize} bytes)");
                }

                // Добавление в строку текущей папки информации о её размере.
                directoryContent[0] = $"{directoryContent[0]} ({directorySize} bytes)";
            }    

            return directoryContent;
        }


        // Конвертирует значение числовое байтов в значения более высоких единиц измерения байтов.
        //
        // Параметры:
        //   bytesValue:
        //     Число байтов для конвертации.
        //
        // Возврат:
        //     Форматированная строка сконвертированного значения.
        static string ConvertBytes(long bytesValue)
        {
            string[] suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double result = bytesValue;

            for (i = 0; i < suffix.Length && bytesValue >= 1024; i++, bytesValue /= 1024)
            {
                result = bytesValue / 1024.0;
            }

            return String.Format("{0:0.##} {1}", result, suffix[i]);
        }
    }
}
