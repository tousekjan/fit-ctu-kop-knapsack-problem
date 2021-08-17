using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnapsackProblemApp;
using KnapsackProblemCore.Entities;
using KnapsackProblemCore.Strategies.ConstructiveStrategies;

namespace KnapsackProblemTest.Constructive
{
    public static class KnapsackConstructiveTestStrategyUtils
    {
        public static void TestOneFile(int instanceSize, string dataType, IKnapsackConstructiveStrategy strategy)
        {
            DataFile data = GetDataFile(dataType, instanceSize);

            List<KnapsackInstance> problemInstances = KnapsackDataLoader.LoadProblemInstances(new List<string> { data.ProblemFileName });
            List<KnapsackReferenceSolution> problemSolutions = KnapsackDataLoader.LoadSolutions(new List<string> { data.SolutionFileName });

            var solver = new KnapsackConstructiveSolveContext(strategy);

            foreach (var instance in problemInstances)
            {
                Console.WriteLine($"Testing problem Id {instance.Id}");

                KnapsackSolution solution = solver.GetKnapsackSolution(instance);

                var referenceSolution = problemSolutions.First(x => x.Id == instance.Id);

                if (strategy is KnapsackConstructiveGreedyHeuristicStrategy || 
                    strategy is KnapsackConstructiveGreedyHeuristicReduxStrategy ||
                    strategy is KnapsackConstructiveFPTASStrategy)
                {
                    KnapsackTestUtils.TestCorrectSolutionConstructiveHeuristics(referenceSolution, solution);
                    continue;
                }

                KnapsackTestUtils.TestCorrectColutionConstructive(referenceSolution, solution);
            }
        }

        public static DataFile GetDataFile(string dataType, int instanceSize)
        {
            string problemFileName;
            string solutionFileName;

            if (dataType == "NK")
            {
                problemFileName = KnapsackTestUtils.TestFilesPathNK + $"NK{instanceSize}_inst.dat";
                solutionFileName = KnapsackTestUtils.TestFilesPathNK + $"NK{instanceSize}_sol.dat";
            }
            else if (dataType == "ZKC")
            {
                problemFileName = KnapsackTestUtils.TestFilesPathZKC + $"ZKC{instanceSize}_inst.dat";
                solutionFileName = KnapsackTestUtils.TestFilesPathZKC + $"ZKC{instanceSize}_sol.dat";
            }
            else if (dataType == "ZKW")
            {
                problemFileName = KnapsackTestUtils.TestFilesPathZKW + $"ZKW{instanceSize}_inst.dat";
                solutionFileName = KnapsackTestUtils.TestFilesPathZKW + $"ZKW{instanceSize}_sol.dat";
            }
            else
            {
                throw new Exception("Invalid dataType");
            }

            return new DataFile { ProblemFileName = problemFileName, SolutionFileName = solutionFileName };
        }
    }
}
