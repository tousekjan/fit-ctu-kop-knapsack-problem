using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using KnapsackProblemCore.Entities;
using KnapsackProblemCore.Strategies.ConstructiveStrategies;

namespace KnapsackProblemApp
{
    public static class ExperimentRunner
    {
        private static string EvaluationDataPath = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\\\Data\\hw03\\");

        private static int I = 1; // start id
        private static int n = 15; // number of items
        private static int N = 2; // number of instances
        private static double m = 0.8; // capacity to weight ratio
        private static int W = 500; // max weight
        private static string w = "bal"; // weights distribution 	(bal|light|heavy)
        private static int C = 300; // max price
        private static string c = "uni"; // correlation with weight
        private static int k = 1; // exponent granularity

        public static void Run()
        {
            Experiment7_GranularityLightErr();
            Experiment7_GranularityHeavyErr();
        }

        public static void Experiment1_Weight()
        {
            int repeatCount = 5;

            var benchmarkResults = new List<BenchmarkResultWeight>();

            for (int weight = 1000; weight <= 10000; weight += 1000)
            {
                var instances = GenerateInstances(I: 1, n: 15, N: 100, m: 0.8, W: weight, w: "bal", C: 2000, c: "uni", k: 1);

                double bfTime = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveBruteForceStrategy()).AverageTime;
                double babTime = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveBranchAndBoundStrategy()).AverageTime;
                double heuTime = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveGreedyHeuristicStrategy()).AverageTime;
                double dpTime = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveDynamicProgrammingByWeightStrategy()).AverageTime;

                benchmarkResults.Add(new BenchmarkResultWeight
                {
                    Parameter = weight,
                    BruteForceTime = bfTime,
                    BaBTime = babTime,
                    HeuristicTime = heuTime,
                    DynamicProgrammingTime = dpTime
                });
            }

            SaveBenchmarkResults(benchmarkResults, "experiment1_weight");
        }

        public static void Experiment1_WeightErr()
        {
            int repeatCount = 1;

            var benchmarkResults = new List<BenchmarkResultRelativeError>();

            for (int weight = 1000; weight <= 10000; weight += 1000)
            {
                var instances = GenerateInstances(I: 1, n: 15, N: 100, m: 0.8, W: weight, w: "bal", C: 2000, c: "uni", k: 1);

                double error = EvaluateAlgorithmConstructiveRelativeError(instances, repeatCount, new KnapsackConstructiveGreedyHeuristicStrategy()).AverageRelativeError;

                benchmarkResults.Add(new BenchmarkResultRelativeError
                {
                    ParameterInt = weight,
                    AverageError = error
                });
            }

            SaveBenchmarkResults(benchmarkResults, "Experiment1_WeightErr");
        }

        public static void Experiment2_Price()
        {
            int repeatCount = 5;

            var benchmarkResults = new List<BenchmarkResultWeight>();

            for (int price = 1000; price <= 10000; price += 1000)
            {
                var instances = GenerateInstances(I: 1, n: 15, N: 100, m: 0.8, W: 2000, w: "bal", C: price, c: "uni", k: 1);

                double bfTime = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveBruteForceStrategy()).AverageTime;
                double babTime = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveBranchAndBoundStrategy()).AverageTime;
                double heuTime = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveGreedyHeuristicStrategy()).AverageTime;
                double dpTime = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveDynamicProgrammingByWeightStrategy()).AverageTime;

                benchmarkResults.Add(new BenchmarkResultWeight
                {
                    Parameter = price,
                    BruteForceTime = bfTime,
                    BaBTime = babTime,
                    HeuristicTime = heuTime,
                    DynamicProgrammingTime = dpTime
                });
            }

            SaveBenchmarkResults(benchmarkResults, "experiment2_price");
        }

        public static void Experiment2_PriceErr()
        {
            int repeatCount = 1;

            var benchmarkResults = new List<BenchmarkResultRelativeError>();

            for (int price = 1000; price <= 10000; price += 1000)
            {
                var instances = GenerateInstances(I: 1, n: 15, N: 100, m: 0.8, W: price, w: "bal", C: 2000, c: "uni", k: 1);

                double error = EvaluateAlgorithmConstructiveRelativeError(instances, repeatCount, new KnapsackConstructiveGreedyHeuristicStrategy()).AverageRelativeError;

                benchmarkResults.Add(new BenchmarkResultRelativeError
                {
                    ParameterInt = price,
                    AverageError = error
                });
            }

            SaveBenchmarkResults(benchmarkResults, "Experiment1_WeightErr");
        }

        public static void Experiment3_CapacityToWeight()
        {
            int repeatCount = 5;

            var benchmarkResults = new List<BenchmarkResultWeight>();

            double[] values = new double[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9 };

            foreach (double value in values)
            {
                //var instances = GenerateInstances(I: 1, n: 15, N: 100, m: value, W: 5000, w: "bal", C: 5000, c: "uni", k: 1);
                var instances = GenerateInstances(I: 1, n: 15, N: 500, m: value, W: 10000, w: "bal", C: 10000, c: "uni", k: 1);

                double bfTime = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveBruteForceStrategy()).AverageTime;
                double babTime = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveBranchAndBoundStrategy()).AverageTime;
                double heuTime = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveGreedyHeuristicStrategy()).AverageTime;
                double dpTime = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveDynamicProgrammingByWeightStrategy()).AverageTime;

                benchmarkResults.Add(new BenchmarkResultWeight
                {
                    ParameterDouble = value,
                    BruteForceTime = bfTime,
                    BaBTime = babTime,
                    HeuristicTime = heuTime,
                    DynamicProgrammingTime = dpTime
                });
            }

            SaveBenchmarkResults(benchmarkResults, "experiment3_capacityToWeight2");
        }

        public static void Experiment4_CapacityToWeightRelativeError()
        {
            int repeatCount = 5;

            var benchmarkResults = new List<BenchmarkResultRelativeError>();

            double[] values = new double[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9 };

            foreach (double value in values)
            {
                var instances = GenerateInstances(I: 1, n: 15, N: 100, m: value, W: 5000, w: "bal", C: 5000, c: "uni", k: 1);

                double error = EvaluateAlgorithmConstructiveRelativeError(instances, repeatCount, new KnapsackConstructiveGreedyHeuristicStrategy()).AverageRelativeError;

                benchmarkResults.Add(new BenchmarkResultRelativeError
                {
                    ParameterDouble = value,
                    AverageError = error
                });
            }

            SaveBenchmarkResults(benchmarkResults, "experiment4_capacityToWeightError");
        }

        public static void Experiment5_BranchAndBoundRobust()
        {
            int repeatCount = 1;

            var benchmarkResults = new List<BenchmarkResultRobust>();

            var instances = GenerateInstances(I: 1, n: 15, N: 1, m: 0.8, W: 5000, w: "bal", C: 5000, c: "uni", k: 1);
            var permutations = GeneratePermutations(instances.First().ToString());
            
            var bfResult = EvaluateAlgorithmConstructiveTime(permutations, repeatCount, new KnapsackConstructiveBruteForceStrategy());
            var babResult = EvaluateAlgorithmConstructiveTime(permutations, repeatCount, new KnapsackConstructiveBranchAndBoundStrategy());
            var heuResult = EvaluateAlgorithmConstructiveTime(permutations, repeatCount, new KnapsackConstructiveGreedyHeuristicStrategy());
            var dpResult = EvaluateAlgorithmConstructiveTime(permutations, repeatCount, new KnapsackConstructiveDynamicProgrammingByWeightStrategy());

            benchmarkResults.Add(new BenchmarkResultRobust
            {
                BFMinTime = bfResult.MinimumTime,
                BFAvgTime = bfResult.AverageTime,
                BFMaxTime = bfResult.MaximumTime,
                BaBMinTime = babResult.MinimumTime,
                BaBAvgTime = babResult.AverageTime,
                BaBMaxTime = babResult.MaximumTime,
                HeuMinTime = heuResult.MinimumTime,
                HeuAvgTime = heuResult.AverageTime,
                HeuMaxTime = heuResult.MaximumTime,
                DPMinTime = dpResult.MinimumTime,
                DPAvgTime = dpResult.AverageTime,
                DPMaxTime = dpResult.MaximumTime,
            });

            SaveBenchmarkResults(benchmarkResults, "Experiment5_BranchAndBoundRobust");
        }

        public static void Experiment6_Granularity()
        {
            int repeatCount = 5;
            string[] valuesw = new string[] { "bal", "light", "heavy" };
            var benchmarkResults = new List<BenchmarkResultGranularity>();

            foreach (var valuew in valuesw)
            {
                var instances = GenerateInstances(I: 1, n: 15, N: 100, m: 0.8, W: 5000, w: valuew, C: 5000, c: "uni", k: 1);

                var bfResult = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveBruteForceStrategy());
                var babResult = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveBranchAndBoundStrategy());
                var heuResult = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveGreedyHeuristicStrategy());
                var dpResult = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveDynamicProgrammingByWeightStrategy());

                benchmarkResults.Add(new BenchmarkResultGranularity
                {
                    w = valuew,
                    BaBTime = babResult.AverageTime,
                    BruteForceTime = bfResult.AverageTime,
                    DynamicProgrammingTime = dpResult.AverageTime,
                    HeuristicTime = heuResult.AverageTime
                });
            }

            SaveBenchmarkResults(benchmarkResults, "Experiment6_Granularity");
        }

        public static void Experiment6_GranularityErr()
        {
            int repeatCount = 1;
            string[] valuesw = new string[] { "bal", "light", "heavy" };
            var benchmarkResults = new List<BenchmarkResultRelativeError>();

            foreach (var valuew in valuesw)
            {
                var instances = GenerateInstances(I: 1, n: 15, N: 100, m: 0.8, W: 5000, w: valuew, C: 5000, c: "uni", k: 1);

                double error = EvaluateAlgorithmConstructiveRelativeError(instances, repeatCount, new KnapsackConstructiveGreedyHeuristicStrategy()).AverageRelativeError;

                benchmarkResults.Add(new BenchmarkResultRelativeError
                {
                    ParameterString = valuew,
                    AverageError = error
                });
            }

            SaveBenchmarkResults(benchmarkResults, "Experiment6_GranularityErr");
        }

        public static void Experiment7_GranularityLight()
        {
            int repeatCount = 5;
            var benchmarkResults = new List<BenchmarkResultWeight>();

            for(int k = 1; k < 10; k++)
            {
                var instances = GenerateInstances(I: 1, n: 15, N: 100, m: 0.8, W: 5000, w: "light", C: 5000, c: "uni", k: k);

                var bfResult = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveBruteForceStrategy());
                var babResult = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveBranchAndBoundStrategy());
                var heuResult = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveGreedyHeuristicStrategy());
                var dpResult = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveDynamicProgrammingByWeightStrategy());

                benchmarkResults.Add(new BenchmarkResultWeight
                {
                    Parameter = k,
                    BaBTime = babResult.AverageTime,
                    BruteForceTime = bfResult.AverageTime,
                    DynamicProgrammingTime = dpResult.AverageTime,
                    HeuristicTime = heuResult.AverageTime
                });
            }

            SaveBenchmarkResults(benchmarkResults, "Experiment7_GranularityLight");
        }

        public static void Experiment7_GranularityLightErr()
        {
            int repeatCount = 1;
            var benchmarkResults = new List<BenchmarkResultRelativeError>();

            for (int k = 1; k < 10; k++)
            {
                var instances = GenerateInstances(I: 1, n: 15, N: 100, m: 0.8, W: 5000, w: "light", C: 5000, c: "uni", k: k);

                double error = EvaluateAlgorithmConstructiveRelativeError(instances, repeatCount, new KnapsackConstructiveGreedyHeuristicStrategy()).AverageRelativeError;

                benchmarkResults.Add(new BenchmarkResultRelativeError
                {
                    ParameterInt = k,
                    AverageError = error
                });
            }

            SaveBenchmarkResults(benchmarkResults, "Experiment7_GranularityLightErr");
        }


        public static void Experiment7_GranularityHeavy()
        {
            int repeatCount = 10;
            var benchmarkResults = new List<BenchmarkResultWeight>();

            for (int k = 1; k < 10; k++)
            {
                var instances = GenerateInstances(I: 1, n: 15, N: 100, m: 0.8, W: 5000, w: "heavy", C: 5000, c: "uni", k: k);

                var bfResult = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveBruteForceStrategy());
                var babResult = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveBranchAndBoundStrategy());
                var heuResult = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveGreedyHeuristicStrategy());
                var dpResult = EvaluateAlgorithmConstructiveTime(instances, repeatCount, new KnapsackConstructiveDynamicProgrammingByWeightStrategy());

                benchmarkResults.Add(new BenchmarkResultWeight
                {
                    Parameter = k,
                    BaBTime = babResult.AverageTime,
                    BruteForceTime = bfResult.AverageTime,
                    DynamicProgrammingTime = dpResult.AverageTime,
                    HeuristicTime = heuResult.AverageTime
                });
            }

            SaveBenchmarkResults(benchmarkResults, "Experiment7_GranularityHeavy");
        }

        public static void Experiment7_GranularityHeavyErr()
        {
            int repeatCount = 1;
            var benchmarkResults = new List<BenchmarkResultRelativeError>();

            for (int k = 1; k < 10; k++)
            {
                var instances = GenerateInstances(I: 1, n: 15, N: 100, m: 0.8, W: 5000, w: "heavy", C: 5000, c: "uni", k: k);

                double error = EvaluateAlgorithmConstructiveRelativeError(instances, repeatCount, new KnapsackConstructiveGreedyHeuristicStrategy()).AverageRelativeError;

                benchmarkResults.Add(new BenchmarkResultRelativeError
                {
                    ParameterInt = k,
                    AverageError = error
                });
            }

            SaveBenchmarkResults(benchmarkResults, "Experiment7_GranularityHeavyErr");
        }

        private static List<KnapsackInstance> GenerateInstances(int I, int n, int N, double m, int W, string w, int C, string c, int k)
        {
            string generatedInstances = "";
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "..\\..\\..\\..\\Generator\\kg2.exe";
                process.StartInfo.Arguments = $"-I {I} -n {n} -N {N} -m {m} -W {W} -w {w} -C {C} -c {c} -k {k}";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                StreamReader reader = process.StandardOutput;
                generatedInstances = reader.ReadToEnd();

                //Console.WriteLine(generatedInstances);

                process.WaitForExit();
            }

            return KnapsackDataLoader.LoadProblemInstances(generatedInstances);
        }

        private static List<KnapsackInstance> GeneratePermutations(string instance)
        {
            int delta = 1;
            int count = 1000;

            string generatedInstances = "";
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "..\\..\\..\\..\\Generator\\kg_perm.exe";
                process.StartInfo.Arguments = $"-d {delta} -N {count}";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;

                process.Start();

                StreamWriter myStreamWriter = process.StandardInput;
                myStreamWriter.WriteLine(instance);
                myStreamWriter.Close();

                StreamReader reader = process.StandardOutput;
                generatedInstances = reader.ReadToEnd();

                //Console.WriteLine(generatedInstances);

                process.WaitForExit();
            }

            return KnapsackDataLoader.LoadProblemInstances(generatedInstances);
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
                MinimumTime = measuredTimes.Min(),
                AverageTime = measuredTimes.Average(),
                MaximumTime = measuredTimes.Max(),
            };

            Console.WriteLine(result);
            return result;
        }

        private static BenchmarkResultError EvaluateAlgorithmConstructiveRelativeError(
            List<KnapsackInstance> problemInstances, int repeatCount, IKnapsackConstructiveStrategy strategy)
        {
            var context = new KnapsackConstructiveSolveContext(strategy);
            var errors = new List<double>();

            foreach (var problem in problemInstances)
            {
                var solution = context.GetKnapsackSolution(problem);

                var contextBaB = new KnapsackConstructiveSolveContext(new KnapsackConstructiveBranchAndBoundStrategy());

                var referenceSolution = contextBaB.GetKnapsackSolution(problem);

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

        private static void SaveBenchmarkResults<T>(List<T> benchmarkResults, string fileName)
        {
            using (var writer = new StreamWriter($"{EvaluationDataPath}\\{fileName}.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(benchmarkResults);
            }
        }

        private static double CalculateError(int solutionPrice, int referenceSolutionPrice)
        {
            if (referenceSolutionPrice > 0)
            {
                return (double)(referenceSolutionPrice - solutionPrice) / referenceSolutionPrice;
            }

            return 0;
        }
    }
}
