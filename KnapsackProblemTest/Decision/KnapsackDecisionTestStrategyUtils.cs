using System;
using System.Collections.Generic;
using System.Linq;
using KnapsackProblemApp;
using KnapsackProblemCore.Entities;
using KnapsackProblemCore.Strategies.DecisionStrategies;

namespace KnapsackProblemTest.Decision
{
    public static class KnapsackDecisionTestStrategyUtils
    {
        public static void TestOneFile(int fileNumber, string dataType, IKnapsackDecisionStrategy strategy)
        {
            string problemFileName;
            string solutionFileName;

            if (dataType == "ZR")
            {
                problemFileName = KnapsackTestUtils.TestFilesPathZR + $"ZR{fileNumber}_inst.dat";
                solutionFileName = KnapsackTestUtils.TestFilesPathZR + $"ZK{fileNumber}_sol.dat";
            }
            else
            {
                problemFileName = KnapsackTestUtils.TestFilesPathNR + $"NR{fileNumber}_inst.dat";
                solutionFileName = KnapsackTestUtils.TestFilesPathNR + $"NK{fileNumber}_sol.dat";
            }

            List<KnapsackInstance> problemInstances = KnapsackDataLoader.LoadProblemInstances(new List<string> { problemFileName });
            List<KnapsackReferenceSolution> problemSolutions = KnapsackDataLoader.LoadSolutions(new List<string> { solutionFileName });

            var solver = new KnapsackDecisionSolveContext(strategy);

            foreach (var instance in problemInstances)
            {
                Console.WriteLine($"Testing problem Id {instance.Id}");

                KnapsackSolutionDecision solution = solver.GetKnapsackSolution(instance);

                var referenceSolution = problemSolutions.First(x => x.Id == instance.Id);

                KnapsackTestUtils.TestCorrectColutionDecision(referenceSolution, solution);
            }
        }
    }
}
