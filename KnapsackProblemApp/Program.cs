using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using KnapsackProblemCore.Entities;
using KnapsackProblemCore.Strategies.DecisionStrategies;

namespace KnapsackProblemApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            ProgramArguments arguments;

            try
            {
                arguments = new ProgramArguments(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            List<string> files = FileLoader.LoadInstanceFiles(arguments.TestDirectoryPath);

            var benchmarkResults = new List<BenchmarkResult>();

            IKnapsackDecisionStrategy strategy;
            if (arguments.Algorithm == "bf")
            {
                strategy = new KnapsackDecisionBruteForceStrategy();    
            }
            else
            {
                strategy = new KnapsackDecisionBranchAndBoundStrategy();
            }

            Console.WriteLine($"Strategy: {strategy}");

            foreach (string file in files)
            {
                benchmarkResults.Add(EvaluateAlgorithmDecision(file, arguments.RepeatOneFile, strategy));
            }

            PrintBenchmarkResults(benchmarkResults);
            SaveBenchmarkResults(benchmarkResults, arguments.OutputFileName);

            Console.ReadKey();
        }

        private static void PrintBenchmarkResults(List<BenchmarkResult> benchmarkResults)
        {
            foreach (var result in benchmarkResults)
            {
                Console.WriteLine(result.ToString());
            }
        }

        private static void SaveBenchmarkResults(List<BenchmarkResult> benchmarkResults, string outputFileName)
        {
            string outputPath = $"{Directory.GetCurrentDirectory()}\\{outputFileName}";

            using (var writer = new StreamWriter(outputPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(benchmarkResults);
            }
        }

        private static BenchmarkResult EvaluateAlgorithmDecision(string fileName, int repeatCount, IKnapsackDecisionStrategy strategy)
        {
            List<KnapsackInstance> problemInstances = KnapsackDataLoader.LoadProblemInstances(new List<string> { fileName });

            var context = new KnapsackDecisionSolveContext(strategy);
            
            var stopwatch = new Stopwatch();
            var visitedStates = new List<int>();
            var measuredTimes = new List<long>();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            for (int i = 0; i < repeatCount; i++)
            {
                foreach (var problem in problemInstances)
                {
                    stopwatch.Start();
                    var solution = context.GetKnapsackSolution(problem);
                    stopwatch.Stop();

                    visitedStates.Add(solution.VisitedStates);
                    measuredTimes.Add(stopwatch.ElapsedMilliseconds);
                }
            }

            var result = new BenchmarkResult()
            {
                InstanceSize = problemInstances.First().Count,
                AverageTime = measuredTimes.Average(),
                MaximumTime = measuredTimes.Max(),
                VisitedStates = visitedStates.Average()
            };

            ExportVisitedStates(visitedStates);

            Console.WriteLine(result);
            return result;
        }

        private static void ExportVisitedStates(List<int> visitedStates)
        {
            string outputPath = $"{Directory.GetCurrentDirectory()}\\visitedStates.csv";

            using (var writer = new StreamWriter(outputPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(visitedStates);
            }
        }
    }
}
