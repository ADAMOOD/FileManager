using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using BetterConsoleTables;

namespace FileManager
{
    public class ExtensionStatistic
    {
        public string Name { get; set; }
        public long Count { get; set; }
        public long Sum { get; set; }

        public ExtensionStatistic(string name, long count, long sum)
        {
            Name = name;
            Count = count;
            Sum = sum;
        }
    }
    internal class Program
    {
        public const char Separator = ';';
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("FILEMANAGER3000!!!!!!!!!!");
                var directoryPath = "";
                do
                {
                    directoryPath = ReadValue("Directory", @"C:\Windows\System32");
                } while (!CheckIfDirExists(directoryPath));
                var fileExt = ReadValue("File extension", string.Empty).Trim();
                var allFiles = Directory.GetFiles(directoryPath).Select(x => new FileInfo(x)).ToList();
                if (!((fileExt == string.Empty) || string.IsNullOrEmpty(fileExt)))
                {
                    var trimmedExtensions = fileExt.Split(Separator).Select(x => x.Trim()).ToList();
                    allFiles = allFiles.Where(x => trimmedExtensions.Contains(x.Extension)).ToList();
                }

                var FileExtensions = allFiles.Select(x => x.Extension.ToLower()).Distinct().ToList();
                List<ExtensionStatistic> extensionStatistics = new List<ExtensionStatistic>();
                FileExtensions.ForEach(x =>
                {
                    var filesperextension = allFiles.Where(y => y.Extension.ToLower() == x).ToList();
                    var sum = filesperextension.Sum(x => x.Length);
                    var count = filesperextension.Count;
                    var stats = new ExtensionStatistic(x, count, sum);
                    extensionStatistics.Add(stats);
                });
                extensionStatistics = extensionStatistics.OrderByDescending(x => x.Count).ToList();
                var statTable = new Table("typ", "count", "sum");
                statTable.Config = TableConfiguration.UnicodeAlt();
                foreach (var stat in extensionStatistics)
                {
                    statTable.AddRow($"{stat.Name}", $"{stat.Count}", $"{stat.Sum}");
                }
                var table = new Table("file name", "size", "type");
                table.Config = TableConfiguration.UnicodeAlt();
                foreach (var file in allFiles)
                {
                    table.AddRow($"{file.Name}", $"{PrintInKB(file.Length)}", $"{file.Extension}");
                }
                Console.WriteLine(table);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(statTable);
                Console.ResetColor();
                Console.ReadKey();

            }
        }

        public static bool CheckIfDirExists(string DirectoryStr)
        {
            return Directory.Exists(DirectoryStr);
        }
        public static string ReadValue(string label, string defaultValue)
        {
            Console.Write($"{label} (Default {defaultValue}):");
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
