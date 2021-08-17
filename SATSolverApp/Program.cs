using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using SATSolverApp.Entities;
using SATSolverApp.SAT;

namespace SATSolverApp
{
    class Program
    {
        private static string currentDirectory = Directory.GetCurrentDirectory();

        static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            RunExperiment3();

            stopwatch.Stop();

            Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds);
            Console.ReadKey();
        }

        private static void RunExperiment()
        {
           
            string instanceFolderPath = Path.Combine(currentDirectory, @"..\\..\\..\\..\\Data\wuf-M1\wuf50-201-M1");
            string solutionFilePath = Path.Combine(currentDirectory, @"..\\..\\..\\..\\Data\wuf-M1\wuf20-78-M-opt.dat");
            var instanceFiles = GetInstanceFiles(instanceFolderPath);
            var instances = new List<SATInstance>();

            foreach (string file in instanceFiles.Take(50))
            {
                instances.Add(SATDataLoader.LoadInstance(file));
            }
            //var referenceSolutions = SATDataLoader.LoadSolutions(solutionFilePath);

            var output = new List<ExperimentOutput>();

            var parameterValues = new CrossoverMethod[] 
            { 
                CrossoverMethod.Uniform 
            };

            foreach (var parameterValue in parameterValues)
            {
                var errorsList = new List<List<double>>();
                var satisfiedFormulasRates = new List<double>();
                var unsatisfiedClausesList = new List<List<int>>();
                var timesList = new List<List<long>>();

                for (int i = 0; i < 3; i++)
                {
                    var errors = new List<double>();
                    var unsatisfiedClauses = new List<int>() { 0 };
                    var times = new List<long>();

                    var parameters = new GeneticStrategyParameters()
                    {
                        ElitismCount = 1,
                        MaxGenerations = 50,
                        PopulationSize = 400,
                        SelectionRate = 0.5,
                        MutationRate = 0.01,
                        InitializatonMethod = InitializatonMethod.Random,
                        SelectionMethod = SelectionMethod.Tournament,
                        CrossoverMethod = CrossoverMethod.Uniform
                    };

                    int totalFormulasCount = 0;
                    int satisfiedFormulasCount = 0;

                    foreach (SATInstance instance in instances)
                    {
                        //var referenceSolution = referenceSolutions.Find(x => x.InstanceId == instance.InstanceId);
                        //if (referenceSolution == null)
                        //    continue;

                        var solver = new SATSolver(instance, parameters);

                        var stopwatch = Stopwatch.StartNew();
                        var solution = solver.SolveSAT();
                        stopwatch.Stop();

                        
                        totalFormulasCount++;

                        if (solution.IsSatisfied)
                        {
                            double error = CalculateError(solution.Weight, /*referenceSolution.Weight*/ 0);
                            errors.Add(error);
                            satisfiedFormulasCount++;
                        }
                        else
                        {
                            unsatisfiedClauses.Add(solution.UnsatisfiedClausesCount);
                        }

                        times.Add(stopwatch.ElapsedMilliseconds);

                        //SaveBenchmarkResults(solution.Generations, "mutationRate" + DateTime.Now.ToString("yyyyMMdd_hhmmss"));
                    }

                    satisfiedFormulasRates.Add((double)satisfiedFormulasCount / totalFormulasCount);

                    errorsList.Add(errors);
                    unsatisfiedClausesList.Add(unsatisfiedClauses);
                    timesList.Add(times);
                }

                var experimentOutput = new ExperimentOutput()
                {
                    Parameter = Path.GetDirectoryName(instanceFolderPath),
                    AvgUnsatisfiedClauses = unsatisfiedClausesList.Select(x => x.Average()).Average(),
                    MaxUnsatisfiedClauses = unsatisfiedClausesList.Select(x => x.Max()).Average(),
                    RelativeError = errorsList.Select(x => x.Average()).Average(),
                    SatisfiedFormulas = satisfiedFormulasRates.Average(),
                    Time = timesList.Select(x => x.Average()).Average()
                };

                output.Add(experimentOutput);
            }

            SaveBenchmarkResults(output, "M50_05_" + DateTime.Now.ToString("yyyyMMdd_hhmmss"));
        }

        private static void RunExperiment2()
        {
            var output = new List<ExperimentOutput>();

            var parameterValues = new string[]
            {
                Path.Combine(currentDirectory, @"..\..\..\..\Data\wuf-A1\wuf20-91-A1"),
                Path.Combine(currentDirectory, @"..\..\..\..\Data\wuf-M1\wuf50-201-M1"),
                Path.Combine(currentDirectory, @"..\..\..\..\Data\wuf-N1\wuf50-201-N1"),
                Path.Combine(currentDirectory, @"..\..\..\..\Data\wuf-Q1\wuf50-201-Q1"),
                Path.Combine(currentDirectory, @"..\..\..\..\Data\wuf-R1\wuf50-201-R1"),
            };

            foreach (var parameterValue in parameterValues)
            {
                Console.WriteLine($"parameter = {parameterValue}");
                var instanceFiles = GetInstanceFiles(parameterValue);

                var instances = new List<SATInstance>();

                foreach (string file in instanceFiles.Take(50))
                {
                    instances.Add(SATDataLoader.LoadInstance(file));
                }

                var errorsList = new List<List<double>>();
                var satisfiedFormulasRates = new List<double>();
                var unsatisfiedClausesList = new List<List<int>>();
                var timesList = new List<List<long>>();

                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine($"i = {i}");
                    var errors = new List<double>();
                    var unsatisfiedClauses = new List<int>() { 0 };
                    var times = new List<long>();

                    var parameters = new GeneticStrategyParameters()
                    {
                        ElitismCount = 1,
                        MaxGenerations = 400,
                        PopulationSize = 400,
                        SelectionRate = 0.5,
                        MutationRate = 0.05,
                        InitializatonMethod = InitializatonMethod.Random,
                        SelectionMethod = SelectionMethod.Tournament,
                        CrossoverMethod = CrossoverMethod.Uniform
                    };

                    int totalFormulasCount = 0;
                    int satisfiedFormulasCount = 0;

                    foreach (SATInstance instance in instances)
                    {
                        var solver = new SATSolver(instance, parameters);

                        var stopwatch = Stopwatch.StartNew();
                        var solution = solver.SolveSAT();
                        stopwatch.Stop();


                        totalFormulasCount++;

                        if (solution.IsSatisfied)
                        {
                            double error = CalculateError(solution.Weight,  0);
                            errors.Add(error);
                            satisfiedFormulasCount++;
                        }
                        else
                        {
                            unsatisfiedClauses.Add(solution.UnsatisfiedClausesCount);
                        }

                        times.Add(stopwatch.ElapsedMilliseconds);
                        //SaveBenchmarkResults(solution.Generations, "Generations_75" + DateTime.Now.ToString("yyyyMMdd_hhmmss"));
                    }

                    satisfiedFormulasRates.Add((double)satisfiedFormulasCount / totalFormulasCount);

                    errorsList.Add(errors);
                    unsatisfiedClausesList.Add(unsatisfiedClauses);
                    timesList.Add(times);
                }

                var experimentOutput = new ExperimentOutput()
                {
                    Parameter = parameterValue,
                    AvgUnsatisfiedClauses = unsatisfiedClausesList.Select(x => x.Average()).Average(),
                    MaxUnsatisfiedClauses = unsatisfiedClausesList.Select(x => x.Max()).Average(),
                    SatisfiedFormulas = satisfiedFormulasRates.Average(),
                    Time = timesList.Select(x => x.Average()).Average()
                };

                output.Add(experimentOutput);
            }

            SaveBenchmarkResults(output, "Instances_20" + DateTime.Now.ToString("yyyyMMdd_hhmmss"));
        }


        private static void RunExperiment3()
        {
            string path = Path.Combine(currentDirectory, @"..\..\..\..\Data\");

            var tuples = new List<Tuple<string, string>>()
            {
                
                new Tuple<string, string>(@"wuf-A1\wuf20-88-A1", @"wuf-A1\wuf20-88-A-opt.dat"),
                new Tuple<string, string>(@"wuf-A1\wuf20-91-A1", @"wuf-A1\wuf20-91-A-opt.dat"),

                new Tuple<string, string>(@"wuf-M1\wuf20-78-M1", @"wuf-M1\wuf20-78-M-opt.dat"),
                new Tuple<string, string>(@"wuf-M1\wuf50-201-M1", @"wuf-M1\wuf50-201-M-opt.dat"),

                new Tuple<string, string>(@"wuf-N1\wuf20-78-N1", @"wuf-N1\wuf20-78-N-opt.dat"),
                new Tuple<string, string>(@"wuf-N1\wuf50-201-N1", @"wuf-N1\wuf50-201-N-opt.dat"),

                new Tuple<string, string>(@"wuf-Q1\wuf20-78-Q1", @"wuf-Q1\wuf20-78-Q-opt.dat"),
                new Tuple<string, string>(@"wuf-Q1\wuf50-201-Q1", @"wuf-Q1\wuf50-201-Q-opt.dat"),

                new Tuple<string, string>(@"wuf-R1\wuf20-78-R1", @"wuf-R1\wuf20-78-R-opt.dat"),
                new Tuple<string, string>(@"wuf-R1\wuf50-201-R1", @"wuf-R1\wuf50-201-R-opt.dat"),
            };

            var output = new List<ExperimentOutput>();

            foreach (var tuple in tuples)
            {
                Console.WriteLine(tuple.Item1);
                var stopwatchTuple = Stopwatch.StartNew();

                var instanceFiles = GetInstanceFiles(path + tuple.Item1);
                var instances = new List<SATInstance>();

                foreach (string file in instanceFiles)
                {
                    instances.Add(SATDataLoader.LoadInstance(file));
                }

                var referenceSolutions = SATDataLoader.LoadSolutions(path + tuple.Item2);

                var errorsList = new List<List<double>>();
                var satisfiedFormulasRates = new List<double>();
                var unsatisfiedClausesList = new List<List<int>>();
                var timesList = new List<List<long>>();

                for (int i = 0; i < 3; i++)
                {
                    var parameters = new GeneticStrategyParameters()
                    {
                        ElitismCount = 1,
                        MaxGenerations = 50,
                        PopulationSize = 400,
                        SelectionRate = 0.3,
                        MutationRate = 0.05,
                        InitializatonMethod = InitializatonMethod.Random,
                        SelectionMethod = SelectionMethod.Tournament,
                        CrossoverMethod = CrossoverMethod.Uniform
                    };

                    int totalFormulasCount = 0;
                    int satisfiedFormulasCount = 0;
                    var errors = new List<double>();
                    var unsatisfiedClauses = new List<int>();
                    var times = new List<long>();

                    foreach (SATInstance instance in instances.Where(x => referenceSolutions.Any(y => y.InstanceId == x.InstanceId)).Take(100))
                    {
                        var referenceSolution = referenceSolutions.Find(x => x.InstanceId == instance.InstanceId);

                        var solver = new SATSolver(instance, parameters);

                        var stopwatch = Stopwatch.StartNew();
                        var solution = solver.SolveSAT();
                        stopwatch.Stop();

                        totalFormulasCount++;

                        if (solution.IsSatisfied)
                        {
                            double error = CalculateError(solution.Weight, referenceSolution.Weight);
                            errors.Add(error);
                            satisfiedFormulasCount++;
                        }
                        else
                        {
                            unsatisfiedClauses.Add(solution.UnsatisfiedClausesCount);
                        }

                        times.Add(stopwatch.ElapsedMilliseconds);
                    }

                    satisfiedFormulasRates.Add((double)satisfiedFormulasCount / totalFormulasCount);

                    errorsList.Add(errors);
                    unsatisfiedClausesList.Add(unsatisfiedClauses);
                    timesList.Add(times);
                }

                stopwatchTuple.Stop();
                Console.WriteLine(tuple + " Time: " + stopwatchTuple.ElapsedMilliseconds.ToString());

                var experimentOutput = new ExperimentOutput()
                {
                    Parameter = tuple.Item1,
                    AvgUnsatisfiedClauses = unsatisfiedClausesList.Select(x => x.DefaultIfEmpty(0).Average()).Average(),
                    MaxUnsatisfiedClauses = unsatisfiedClausesList.Select(x => x.DefaultIfEmpty(0).Max()).Average(),
                    RelativeError = errorsList.Select(x => x.DefaultIfEmpty(0).Average()).Average(),
                    SatisfiedFormulas = satisfiedFormulasRates.Average(),
                    Time = timesList.Select(x => x.Average()).Average()
                };

                output.Add(experimentOutput);
            }

            SaveBenchmarkResults(output, "AllDatasets" + DateTime.Now.ToString("yyyyMMdd_hhmmss"));
        }

        private static double CalculateError(int solutionWeight, int referenceSolutionWeight)
        {
            if (referenceSolutionWeight > 0)
            {
                return (double)(referenceSolutionWeight - solutionWeight) / referenceSolutionWeight;
            }

            return 0;
        }

        private static List<string> GetInstanceFiles(string path)
        {
            if (Directory.Exists(path))
            {
                return Directory.EnumerateFiles(path)
                                .ToList();
            }
            else
            {
                Console.WriteLine($"Specified directory does not exist path: {path}");
                return new List<string>();
            }
        }

        private static void SaveBenchmarkResults<T>(List<T> benchmarkResults, string fileName)
        {
            string outputDataPath = Path.Combine(currentDirectory, @"..\..\..\..\Data\Output");
            using (var writer = new StreamWriter($"{outputDataPath}\\{fileName}.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(benchmarkResults);
            }
        }
    }
}
