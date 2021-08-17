using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using KnapsackProblemCore.Entities;
using KnapsackProblemCore.Strategies.ConstructiveStrategies;
using KnapsackProblemCore.Strategies.GeneticStrategies;

namespace KnapsackProblemApp
{
    public static class GeneticExperimentRunner
    {
        private const string OutputDataPath = @"..\\..\\..\\..\\Data\\hw04\\Output";
        public static void Run()
        {
            var stopwatch = Stopwatch.StartNew();
            EvaluateSingle();
            stopwatch.Stop();

            Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds + " ms");
        }

        private static void EvaluateAllParameters()
        {
            var instanceSizes = new int[] { 40, 50, 60, 70, };
            var populationSizes = new int[] { 20, 40, 60, 80, 100 };
            var generationCounts = new int[] { 20, 40, 60, 80, 100 };
            var selectionRates = new double[] { 0.3, 0.4, 0.5, 0.6, 0.7 };
            var mutationRates = new double[] { 0.02, 0.05, 0.1 };

            var output = new List<GeneticAlgorithmResult>();

            Parallel.ForEach(instanceSizes, (instanceSize) =>
            {
                Console.WriteLine($"InstanceSize: {instanceSize} started processing");

                List<KnapsackInstance> problemInstances = GenerateInstances(1, instanceSize, 100, 0.8, 200, "bal", 200, "uni", 1);
                List<KnapsackSolution> referenceSolutions = new List<KnapsackSolution>();

                foreach (KnapsackInstance problemInstance in problemInstances)
                {
                    referenceSolutions.Add(GetReferenceSolution(problemInstance));
                }

                foreach (int populationSize in populationSizes)
                {
                    foreach (int generationCount in generationCounts)
                    {
                        foreach (double selectionRate in selectionRates)
                        {
                            foreach (double mutationRate in mutationRates)
                            {
                                var errors = new List<double>();

                                var parameters = new GeneticStrategyParameters()
                                {
                                    ElitismCount = 2,
                                    PopulationSize = populationSize,
                                    MaxGenerations = generationCount,
                                    SelectionRate = selectionRate,
                                    MutationRate = mutationRate,
                                };

                                var stopwatch = new Stopwatch();

                                foreach (var instance in problemInstances)
                                {
                                    var strategy = new KnapsackConstructiveGeneticStrategy(instance, parameters);

                                    stopwatch.Start();
                                    var solution = strategy.SolveKnapsackConstructive();
                                    stopwatch.Stop();

                                    var referenceSolution = referenceSolutions.Find(x => x.KnapsackInstance.Id == instance.Id);

                                    errors.Add(CalculateError(solution.SolutionPrice, referenceSolution.SolutionPrice));
                                }

                                output.Add(new GeneticAlgorithmResult(parameters)
                                {
                                    InstanceSize = instanceSize,
                                    AvgRelativeError = errors.Average(),
                                    MaxRelativeError = errors.Max(),
                                    TimeElapsed = stopwatch.ElapsedMilliseconds / problemInstances.Count,
                                });
                            }
                        }
                    }
                }

                Console.WriteLine($"InstanceSize: {instanceSize} done");
            });

            SaveBenchmarkResults(output, "MeasuredData3");
        }

        private static void EvaluateSingle()
        {
            var output = new List<GeneticAlgorithmResult>();
            var generations = new List<List<Generation>>();

            List<KnapsackInstance> problemInstances = GenerateInstances(1, 60, 1, 0.8, 200, "bal", 200, "uni", 1);

            List<KnapsackSolution> referenceSolutions = new List<KnapsackSolution>();

            foreach (KnapsackInstance problemInstance in problemInstances)
            {
                referenceSolutions.Add(GetReferenceSolution(problemInstance));
            }

            var errors = new List<double>();

            var parameters = new GeneticStrategyParameters()
            {
                ElitismCount = 2,
                MaxGenerations = 100,
                MutationRate = 0.05,
                PopulationSize = 100,
                SelectionRate = 0.5
            };

            var stopwatch = new Stopwatch();

            foreach (var instance in problemInstances)
            {
                var strategy = new KnapsackConstructiveGeneticStrategy(instance, parameters);

                stopwatch.Start();
                var solution = strategy.SolveKnapsackConstructive();
                stopwatch.Stop();

                var referenceSolution = referenceSolutions.Find(x => x.KnapsackInstance.Id == instance.Id);

                errors.Add(CalculateError(solution.SolutionPrice, referenceSolution.SolutionPrice));
                generations.Add(solution.Generations);
            }

            output.Add(new GeneticAlgorithmResult(parameters)
            {
                AvgRelativeError = errors.Average(),
                MaxRelativeError = errors.Max(),
                InstanceSize = 0,
                TimeElapsed = stopwatch.ElapsedMilliseconds,
            });

            SaveBenchmarkResults(output, "80");
            SaveBenchmarkResults(generations.First(), "generations2");
        }

        private static void Evaluate_40()
        {
            var output = new List<GeneticAlgorithmResult>();

            string instanceFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\\\Data\\hw04\\Input");
            List<string> files = FileLoader.LoadInstanceFiles(instanceFilesPath);

            foreach (string file in files)
            {
                List<KnapsackInstance> problemInstances = KnapsackDataLoader.LoadProblemInstances(new List<string> { file });
                List<KnapsackReferenceSolution> referenceSolutions = KnapsackDataLoader.LoadSolutions(new List<string> { file });
                var errors = new List<double>();

                var parameters = new GeneticStrategyParameters()
                {
                    ElitismCount = 2,
                    MaxGenerations = 30,
                    MutationRate = 0.05,
                    PopulationSize = 40,
                    SelectionRate = 0.4
                };

                var stopwatch = new Stopwatch();

                foreach (var instance in problemInstances)
                {
                    var strategy = new KnapsackConstructiveGeneticStrategy(instance, parameters);

                    stopwatch.Start();
                    var solution = strategy.SolveKnapsackConstructive();
                    stopwatch.Stop();

                    

                    var referenceSolution = GetReferenceSolution(instance);

                    errors.Add(CalculateError(solution.SolutionPrice, referenceSolution.SolutionPrice));
                }

                output.Add(new GeneticAlgorithmResult(parameters)
                {
                    AvgRelativeError = errors.Average(),
                    MaxRelativeError = errors.Max(),
                    InstanceSize = 40,
                    TimeElapsed = stopwatch.ElapsedMilliseconds,
                });

                SaveBenchmarkResults(output, "40");
            }
        }

        private static double CalculateError(int solutionPrice, int referenceSolutionPrice)
        {
            if (referenceSolutionPrice > 0)
            {
                return (double)Math.Abs(referenceSolutionPrice - solutionPrice) / referenceSolutionPrice;
            }

            return 0;
        }

        private static KnapsackSolution GetReferenceSolution(KnapsackInstance instance)
        {
            var context = new KnapsackConstructiveSolveContext(new KnapsackConstructiveDynamicProgrammingByPriceStrategy());
            return context.GetKnapsackSolution(instance);
        }

        private static void GetSolution()
        {
            string instanceFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\Data\\hw02\\NK");
            List<string> files = FileLoader.LoadInstanceFiles(instanceFilesPath);

            foreach (string file in files)
            {
                List<KnapsackInstance> problemInstances = KnapsackDataLoader.LoadProblemInstances(new List<string> { file });
                List<KnapsackReferenceSolution> referenceSolutions = KnapsackDataLoader.LoadSolutions(new List<string> { file });

                foreach (var instance in problemInstances)
                {
                    var strategy = new KnapsackConstructiveGeneticStrategy(instance, new GeneticStrategyParameters());

                    var solution = strategy.SolveKnapsackConstructive();
                }
            }
        }

        private static void SaveBenchmarkResults<T>(List<T> benchmarkResults, string fileName)
        {
            using (var writer = new StreamWriter($"{OutputDataPath}\\{fileName}.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(benchmarkResults);
            }
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
    }
}
