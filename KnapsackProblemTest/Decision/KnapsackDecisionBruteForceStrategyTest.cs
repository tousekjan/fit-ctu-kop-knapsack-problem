using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using KnapsackProblemApp;
using KnapsackProblemCore.Entities;
using KnapsackProblemCore.Strategies.DecisionStrategies;
using NUnit.Framework;

namespace KnapsackProblemTest.Decision
{
    public class KnapsackDecisionBruteForceStrategyTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestAllProblems()
        {
            string dataPath = KnapsackTestUtils.TestFilesPathNR;

            List<string> problemFileNames = FileLoader.LoadInstanceFiles(dataPath);
            List<string> solutionFileNames = FileLoader.LoadSolutionFiles(dataPath);

            foreach (string problemFileName in problemFileNames)
            {
                Console.WriteLine($"Testing file: {problemFileName}");

                int number = int.Parse(Regex.Match(problemFileName, @"\\[NZ]R(\d+)_inst.dat$").Groups[1].Value);
                string solutionFileName = solutionFileNames.Single(x => x.Contains(number.ToString()));

                List<KnapsackInstance> problemInstances = KnapsackDataLoader.LoadProblemInstances(new List<string> { problemFileName });
                List<KnapsackReferenceSolution> problemSolutions = KnapsackDataLoader.LoadSolutions(new List<string> { solutionFileName });

                var strategy = new KnapsackDecisionBruteForceStrategy();
                var solver = new KnapsackDecisionSolveContext(strategy);

                foreach (var instance in problemInstances)
                {
                    KnapsackSolutionDecision solution = solver.GetKnapsackSolution(instance);

                    var referenceSolution = problemSolutions.Single(x => x.Id == instance.Id);

                    KnapsackTestUtils.TestCorrectColutionDecision(referenceSolution, solution);
                }
            }
        }

        [Test]
        public void Test_NR_4()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(4, "NR", new KnapsackDecisionBruteForceStrategy());
        }

        [Test]
        public void Test_NR_10()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(10, "NR", new KnapsackDecisionBruteForceStrategy());
        }

        [Test]
        public void Test_NR_15()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(15, "NR", new KnapsackDecisionBruteForceStrategy());
        }

        [Test]
        public void Test_NR_20()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(20, "NR", new KnapsackDecisionBruteForceStrategy());
        }

        [Test]
        public void Test22()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(22, "NR", new KnapsackDecisionBruteForceStrategy());
        }

        [Test]
        public void Test_ZR_4()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(4, "ZR", new KnapsackDecisionBruteForceStrategy());
        }

        [Test]
        public void Test_ZR_10()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(10, "ZR", new KnapsackDecisionBruteForceStrategy());
        }

        [Test]
        public void Test_ZR_15()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(15, "ZR", new KnapsackDecisionBruteForceStrategy());
        }

        [Test]
        public void Test_ZR_20()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(20, "ZR", new KnapsackDecisionBruteForceStrategy());
        }
    }
}