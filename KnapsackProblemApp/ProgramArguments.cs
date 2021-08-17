using System;
using System.IO;

namespace KnapsackProblemApp
{
    public class ProgramArguments
    {
        public string TestDirectoryPath { get; set; }
        public int RepeatOneFile { get; set; }
        public string Algorithm { get; set; }
        public string OutputFileName { get; set; }
        public ProgramArguments(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("Directory with test files must be specified");
            }

            string argPath = args[0];
            if (Path.IsPathRooted(argPath))
            {
                TestDirectoryPath = argPath;
            }
            else
            {
                TestDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), argPath);
            }

            if (args.Length < 2)
            {
                Algorithm = "bab";
            }
            else
            {
                Algorithm = args[1];
            }

            if (args.Length < 3)
            {
                RepeatOneFile = 1;
            }
            else
            {
                RepeatOneFile = int.Parse(args[2]);
            }

            if(args.Length < 4)
            {
                OutputFileName = "output";
            }
            else
            {
                OutputFileName = args[3];
            }
        }
    }
}
