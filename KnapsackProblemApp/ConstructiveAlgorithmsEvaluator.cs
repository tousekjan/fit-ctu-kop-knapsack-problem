using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using CsvHelper;
using KnapsackProblemCore.Entities;
using KnapsackProblemCore.Strategies.ConstructiveStrategies;

namespace KnapsackProblemApp
{
    public static class ConstructiveAlgorithmsEvaluator
    {
        private static string EvaluationDataPath = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\\\Data\\hw02\\{DataType}");
        private const string DataType = "ZKW";
        public static void EvaluateAlgorithmsRunTime()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            var strategies = new List<IKnapsackConstructiveStrategy>
            {
                //new KnapsackConstructiveBruteForceStrategy(),
                //new KnapsackConstructiveBranchAndBoundStrategy(),
                //new KnapsackConstructiveDynamicProgrammingByWeightStrategy(),
                new KnapsackConstructiveGreedyHeuristicStrategy(),
                new KnapsackConstructiveGreedyHeuristicReduxStrategy(),
                //new KnapsackConstructiveDynamicProgrammingByPriceStrategy(),
            };

            List<string> files = FileLoader.LoadInstanceFiles(EvaluationDataPath + "\\");

            var benchmarkResults = new List<BenchmarkResult>();

            foreach (string file in files)
            {
                foreach (var strategy in strategies)
                {
                    List<KnapsackInstance> problemInstances = KnapsackDataLoader.LoadProblemInstances(new List<string> { file });

                    string solutionFileName = EvaluationDataPath + $"\\{DataType}{problemInstances.First().Count}_sol.dat";
                    List <KnapsackReferenceSolution> referenceSolutions = KnapsackDataLoader.LoadSolutions(new List<string> { solutionFileName });

                    if (problemInstances.First().Count < 23)
                    {
                        benchmarkResults.Add(EvaluateAlgorithmConstructiveTime(problemInstances, 6, strategy));
                    }
                }
            }

            SaveBenchmarkResults(benchmarkResults);
        }

        private static BenchmarkResult EvaluateAlgorithmConstructiveTime(
            List<KnapsackInstance> problemInstances, int repeatCount, IKnapsackConstructiveStrategy strategy)
        {
            var context = new KnapsackConstructiveSolveContext(strategy);

            var stopwatch = new Stopwatch();
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

                    measuredTimes.Add(stopwatch.ElapsedMilliseconds);
                }
            }

            var result = new BenchmarkResult()
            {
                InstanceSize = problemInstances.First().Count,
                Algorithm = strategy.ToString(),
                AverageTime = measuredTimes.Average(),
                MaximumTime = measuredTimes.Max(),
            };

            Console.WriteLine(result);
            return result;
        }

        public static void EvaluateErrors()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            var strategies = new List<IKnapsackConstructiveStrategy>
            {
                new KnapsackConstructiveGreedyHeuristicStrategy(),
                new KnapsackConstructiveGreedyHeuristicReduxStrategy(),
            };

            List<string> files = FileLoader.LoadInstanceFiles(EvaluationDataPath + "\\");

            var benchmarkResults = new List<BenchmarkResultError>();

            foreach (string file in files)
            {
                foreach (var strategy in strategies)
                {
                    List<KnapsackInstance> problemInstances = KnapsackDataLoader.LoadProblemInstances(new List<string> { file });

                    string solutionFileName = EvaluationDataPath + $"\\{DataType}{problemInstances.First().Count}_sol.dat";
                    List<KnapsackReferenceSolution> referenceSolutions = KnapsackDataLoader.LoadSolutions(new List<string> { solutionFileName });

                    if (problemInstances.First().Count < 23)
                    {
                        benchmarkResults.Add(EvaluateApproximativeAlgorithmConstructiveError(problemInstances,referenceSolutions, strategy));
                    }
                }
            }

            SaveBenchmarkResultsError(benchmarkResults);
        }

        private static BenchmarkResultError EvaluateApproximativeAlgorithmConstructiveError(
            List<KnapsackInstance> problemInstances, List<KnapsackReferenceSolution> referenceSolutions, IKnapsackConstructiveStrategy strategy)
        {
            var context = new KnapsackConstructiveSolveContext(strategy);
            var errors = new List<double>();

            foreach (var problem in problemInstances)
            {
                var solution = context.GetKnapsackSolution(problem);

                var referenceSolution = referenceSolutions.Find(x => x.Id == problem.Id);

                double error = CalculateError(solution.SolutionPrice, referenceSolution.SolutionPrice);
                errors.Add(error);
            }

            var result = new BenchmarkResultError()
            {
                InstanceSize = problemInstances.First().Count,
                Algorithm = strategy.ToString(),
                AverageRelativeError = errors.Average(),
                MaximumRelativeError = errors.Max()
            };

            Console.WriteLine(result);
            return result;
        }

        private static double CalculateError(int solutionPrice, int referenceSolutionPrice)
        {
            if (referenceSolutionPrice > 0)
            {
                return (double)(referenceSolutionPrice - solutionPrice) / referenceSolutionPrice;
            }
            
            return 0;
        }

        public static void EvaluateFPTAS()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            List<string> files = FileLoader.LoadInstanceFiles(EvaluationDataPath + "\\");

            var benchmarkResultsFPTAS = new List<BenchmarkResultFPTAS>();

            foreach (string file in files)
            {
                List<KnapsackInstance> problemInstances = KnapsackDataLoader.LoadProblemInstances(new List<string> { file });

                string solutionFileName = EvaluationDataPath + $"\\{DataType}{problemInstances.First().Count}_sol.dat";
                List<KnapsackReferenceSolution> referenceSolutions = KnapsackDataLoader.LoadSolutions(new List<string> { solutionFileName });

                if (problemInstances.First().Count < 23)
                {
                    benchmarkResultsFPTAS.AddRange(EvaluateFPTAS(problemInstances, referenceSolutions));
                }
            }

            SaveFPTASResults(benchmarkResultsFPTAS);
        }


        private static List<BenchmarkResultFPTAS> EvaluateFPTAS(
            List<KnapsackInstance> problemInstances, List<KnapsackReferenceSolution> referenceSolutions)
        {
            var results = new List<BenchmarkResultFPTAS>();
            var epsilons = new List<double> { 0.3f, 0.5f, 0.9f };

            foreach (var epsilon in epsilons)
            {
                var context = new KnapsackConstructiveSolveContext(new KnapsackConstructiveFPTASStrategy(epsilon));
                var errors = new List<double>();
                var stopwatch = new Stopwatch();
                var measuredTimes = new List<long>();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                foreach (var problem in problemInstances)
                {
                    stopwatch.Start();
                    var solution = context.GetKnapsackSolution(problem);
                    stopwatch.Stop();

                    var referenceSolution = referenceSolutions.Find(x => x.Id == problem.Id);

                    double error = CalculateError(solution.SolutionPrice, referenceSolution.SolutionPrice);
                    errors.Add(error);

                    measuredTimes.Add(stopwatch.ElapsedMilliseconds);
                }

                var result = new BenchmarkResultFPTAS()
                {
                    InstanceSize = problemInstances.First().Count,
                    MaximumRelativeError = errors.Max(),
                    AverageTime = measuredTimes.Average(),
                    Epsilon = epsilon
                };

                Console.WriteLine(result);
                results.Add(result);
            }

            return results;
        }

        private static void SaveBenchmarkResults(List<BenchmarkResult> benchmarkResults)
        {
            string dirPath = $"{EvaluationDataPath}_results";
            Directory.CreateDirectory(dirPath);

            var grouped = benchmarkResults.GroupBy(x => x.Algorithm);

            foreach (var group in grouped)
            {
                using (var writer = new StreamWriter($"{dirPath}\\result_{group.Key}.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(group);
                }
            }
        }

        private static void SaveBenchmarkResultsError(List<BenchmarkResultError> benchmarkResults)
        {
            string dirPath = $"{EvaluationDataPath}_results";
            Directory.CreateDirectory(dirPath);

            using (var writer = new StreamWriter($"{dirPath}\\result_errors.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(benchmarkResults);
            }
        }

        private static void SaveFPTASResults(List<BenchmarkResultFPTAS> benchmarkResults)
        {
            string dirPath = $"{EvaluationDataPath}_results";
            Directory.CreateDirectory(dirPath);

            using (var writer = new StreamWriter($"{dirPath}\\result_fptas.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(benchmarkResults);
            }
        }
    }
}
