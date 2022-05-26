using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using BetterConsoleTables;

namespace FileManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {


                Console.WriteLine("FILEMANAGER3000!!!!!!!!!!");
                var directoryPath = ReadValue("Directory", "C:\\Users\\Kijonka\\Downloads");
                var fileExt = ReadValue("File extension", "png");
                var allFiles = Directory.GetFiles(directoryPath).Select(x => new FileInfo(x)).ToList();
                var filteredFiles = allFiles.Where(x => x.Extension == $".{fileExt}").ToList();
                var filteredFilesSum = filteredFiles.Sum(x => x.Length);
                var filteredFilesCount = filteredFiles.Count();
                var filteredFilesAvg = filteredFiles.Average(x => x.Length);

                var table = new Table("file name", "size", "type");
                table.Config = TableConfiguration.UnicodeAlt();
                foreach (var file in filteredFiles)
                {
                    table.AddRow($"{file.Name}", $"{PrintInKB(file.Length)}", $"{fileExt}");
                }

                Console.WriteLine(table);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(
                    $"This directory has {filteredFilesCount} {fileExt} files tak takes about {PrintInMB(filteredFilesSum)} Of free space (avg length: {PrintInKB(filteredFilesAvg)})");
                Console.ResetColor();
                Console.ReadKey();

            }
        }

        public static string ReadValue(string label, string defaultValue)
        {
            Console.Write($"{label} (Default {defaultValue}):");
            string value = Console.ReadLine();
            if (value == "")
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
