using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
                Helpers.ClearCurrentConsoleLine(5);
                char choice = Console.ReadKey().KeyChar;
                if (!CheckIfChoiceInputIsValid(choice))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid menu option selected");
                    Console.ResetColor();
                    Thread.Sleep(750);
                    continue;
                }
                switch (choice)
                {
                    case 'a':
                        {
                            var directoryPath = GetExistingDirectoryPath(@"c:\Windows\System32");
                            var fileExt = ReadValue("File extension", string.Empty).Trim();
                            FileExtensionStatistics(directoryPath, fileExt);
                            Console.WriteLine("Press any key to exit");
                            Console.ReadKey();
                            break;
                        }
                    case 'b':
                        {
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
                            using (FileStream zipToOpen = new FileStream($"{folderPath}\\backup.zip", FileMode.Create))
                            {
                                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                                {
                                    foreach (var file in allfiles)
                                    {
                                        Console.WriteLine($"mooving {file.Name} to zip");
                                        archive.CreateEntryFromFile(file.FullName, file.Name);
                                    }
                                }
                            }
                            Console.WriteLine("Press any key to exit");
                            Console.ReadKey();
                            break;
                        }
                    case 'c':
                        {
                            GetExistingDirectoryPath("C:\\tmp");
                            break;
                        }
                }

            }
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
            Console.WriteLine();
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