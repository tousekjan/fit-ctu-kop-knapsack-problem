using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KnapsackProblemApp
{
    public static class FileLoader
    {
        public static List<string> LoadInstanceFiles(string path)
        {
            return LoadFiles(path, "*_inst.dat");
        }

        public static List<string> LoadSolutionFiles(string path)
        {
            return LoadFiles(path, "*_sol.dat");
        }

        private static List<string> LoadFiles(string path, string template)
        {
            if (Directory.Exists(path))
            {
                return Directory.EnumerateFiles(path, template)
                                .ToList();
            }
            else
            {
                Console.WriteLine($"Specified directory does not exist path: {path}");
                return new List<string>();
            }
        }
    }
}
