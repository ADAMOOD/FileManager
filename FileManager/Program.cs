using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using BetterConsoleTables;
using FileManager.Items;

namespace FileManager
{

    internal class Program
    {
        public const char Separator = ';';
        static void Main(string[] args)
        {
            Console.WriteLine("FILEMANAGER3000!!!!!!!!!!");
            Console.WriteLine("Choose Option");
            Console.WriteLine("A->File extension statistics\nB->Create backup file\nC->List backup files");
            while (true)
            {
                Helpers.ClearCurrentConsoleArea(5);
                char choice = Console.ReadKey().KeyChar;
                if (!CheckIfChoiceInputIsValid(choice))
                {
                    Error("\nInvalid menu option selected");
                    continue;
                }
                switch (choice)
                {
                    case 'a':
                        {
                            Console.WriteLine();
                            var directoryPath = GetExistingDirectoryPath(@"c:\Windows\System32");
                            var fileExt = ReadValue("File extension", string.Empty).Trim();
                            FileExtensionStatistics(directoryPath, fileExt);
                            Console.WriteLine("Press any key to exit");
                            Console.ReadKey();
                            break;
                        }
                    case 'b':
                        {
                            Console.WriteLine();
                            var directoryPath = ReadValue("Directory", @"C:\tmp");
                            if (!CheckIfDirExists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }
                            var root = directoryPath;
                            //nevim uplne presne co je root directory
                            var allfiles = Directory.GetFiles(root).Select(x => new FileInfo(x)).ToList();
                            var folderPath = $"{directoryPath}\\{DateTime.Now.ToString("yyyy-mm-dd_hh-mm-ss")}";
                            Directory.CreateDirectory(folderPath);
                            var zipPath = $"{folderPath}\\backup.zip";
                            var time = Stopwatch.StartNew();

                            using var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create);
                            {
                                foreach (var file in allfiles)
                                {
                                    Console.WriteLine($"mooving {file.Name} to zip");
                                    archive.CreateEntryFromFile(file.FullName, file.Name);
                                }

                            }
                            time.Stop();
                            var timeInMs = time.ElapsedMilliseconds;
                            Console.WriteLine($"This operation lasted {timeInMs} ms");
                            Console.WriteLine("Press any key to exit");
                            Console.ReadKey();
                            break;
                        }
                    case 'c':
                        {
                            Console.WriteLine();
                            var directoroyPath = GetExistingDirectoryPath("C:\\tmp");

                            List<FileInfo> allZips = new List<FileInfo>();
                            int i = 1;
                            var allZipFolders = Directory.EnumerateFiles(directoroyPath, "backup.zip", SearchOption.AllDirectories).ToList();
                            foreach (var folder in allZipFolders)
                            {
                                Console.WriteLine($"{i}-> {folder}");
                                i++;
                            }
                            var folderNumber = 0;
                            Int32.TryParse(ReadValue("Select folder by number", "1"), out folderNumber);
                            if ((folderNumber > i) || (folderNumber < 1))
                            {
                                Error("\nInvalid menu option selected");
                                break;
                            }
                            Process.Start("explorer.exe", allZipFolders[folderNumber-1]);
                            Console.WriteLine("Press any key to exit");
                            Console.ReadKey();
                            break;
                        }
                }

            }
        }

        private static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            Thread.Sleep(750);
        }


        public static bool CheckIfChoiceInputIsValid(char input)
        {
            return ((input == 'a') || (input == 'b') || (input == 'c'));
        }
        private static void FileExtensionStatistics(string directoryPath, string fileExt)
        {
            var allFiles = Directory.GetFiles(directoryPath).Select(x => new FileInfo(x)).ToList();
            if (!((fileExt == string.Empty) || string.IsNullOrEmpty(fileExt)))
            {
                var trimmedExtensions = fileExt.Split(Separator).Select(x => x.Trim()).ToList();
                allFiles = allFiles.Where(x => trimmedExtensions.Contains(x.Extension)).ToList();
            }

            var FileExtensions = allFiles.Select(x => x.Extension.ToLower()).Distinct().ToList();
            List<ExtensionStatistic> extensionStatistics = new List<ExtensionStatistic>();
            List<FileInfo> orderedFileInfos = new List<FileInfo>();
            FileExtensions.ForEach(x =>
            {
                var filesperextension = allFiles.Where(y => y.Extension.ToLower() == x).ToList();
                orderedFileInfos = filesperextension.OrderBy(x => x.Name).ToList();
                var sum = filesperextension.Sum(x => x.Length);
                var count = filesperextension.Count;
                var stats = new ExtensionStatistic(x, count, sum);
                extensionStatistics.Add(stats);
            });
            extensionStatistics = extensionStatistics.OrderByDescending(x => x.Count).ToList();
            CreateAndPrintTable(extensionStatistics);
        }

        private static void CreateAndPrintTable(List<ExtensionStatistic> extensionStatistics)
        {
            var statTable = new Table("type", "count", "sum");
            statTable.Config = TableConfiguration.UnicodeAlt();
            foreach (var stat in extensionStatistics)
            {
                statTable.AddRow($"{stat.Name}", $"{stat.Count}", $"{PrintInKB(stat.Sum)}");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(statTable);
            Console.ResetColor();
        }

        private static string GetExistingDirectoryPath(string DefaultPath)
        {

            var directoryPath = "";
            do
            {
                directoryPath = ReadValue("Directory", $"{DefaultPath}");
            } while (!CheckIfDirExists(directoryPath));

            return directoryPath;
        }

        public static bool CheckIfDirExists(string DirectoryStr)
        {
            return Directory.Exists(DirectoryStr);
        }
        public static string ReadValue(string label, string defaultValue)
        {
            Console.WriteLine($"{label} (Default {defaultValue}):");
            string value = Console.ReadLine();
            if (value == string.Empty)
                return defaultValue;
            return value;

        }

        public static long GetInMB(long num)
        {
            return num / 1024 / 1024;
        }

        public static long GetInKB(long num)
        {
            return num / 1024;
        }
        public static double GetInKB(double num)
        {
            return num / 1024;
        }

        public static string PrintInKB(long num)
        {
            return $"{GetInKB(num)} kB";
        }
        public static string PrintInKB(double num)
        {
            return $"{GetInKB(num):F} kB";
        }
        public static string PrintInMB(long num)
        {
            return $"{GetInMB(num)} MB";
        }

    }
}