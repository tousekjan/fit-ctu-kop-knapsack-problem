using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using KnapsackProblemCore.Entities;
using NUnit.Framework;

namespace KnapsackProblemTest
{
    public static class KnapsackTestUtils
    {
        public static string CurrentDirectory = Directory.GetCurrentDirectory();
        public static string TestFilesPathNR = Path.Combine(CurrentDirectory, @"..\\..\\..\\..\\Data\hw01\NR\");
        public static string TestFilesPathZR = Path.Combine(CurrentDirectory, @"..\\..\\..\\..\\Data\hw01\ZR\");

        public static string TestFilesPathNK = Path.Combine(CurrentDirectory, @"..\\..\\..\\..\\Data\hw02\NK\");
        public static string TestFilesPathZKC = Path.Combine(CurrentDirectory, @"..\\..\\..\\..\\Data\hw02\ZKC\");
        public static string TestFilesPathZKW = Path.Combine(CurrentDirectory, @"..\\..\\..\\..\\Data\hw02\ZKW\");

        public static void TestCorrectColutionDecision(KnapsackReferenceSolution referenceSolution, KnapsackSolutionDecision solution)
        {
            if (solution.HasSolution)
            {
                Assert.That(solution.SolutionWeight <= solution.KnapsackInstance.Capacity);
                Assert.That(referenceSolution.SolutionPrice >= solution.KnapsackInstance.MinimumPrice);
            }
            else
            {
                Assert.That(referenceSolution.SolutionPrice < solution.KnapsackInstance.MinimumPrice);
            }
        }

        internal static void TestCorrectSolutionConstructiveHeuristics(KnapsackReferenceSolution referenceSolution, KnapsackSolution solution)
        {
            Assert.That(IsPriceAndWeightCorrect(solution));
            Assert.That(solution.SolutionWeight <= solution.KnapsackInstance.Capacity);
        }

        internal static void TestCorrectSolutionConstructiveFPTAS(KnapsackReferenceSolution referenceSolution, KnapsackSolution solution)
        {
            throw new NotImplementedException();
        }

        public static void TestCorrectColutionConstructive(KnapsackReferenceSolution referenceSolution, KnapsackSolution solution)
        {
            Assert.That(solution.SolutionWeight <= solution.KnapsackInstance.Capacity);
            Assert.AreEqual(referenceSolution.SolutionPrice, solution.SolutionPrice);
            Assert.That(IsPriceAndWeightCorrect(solution));
        }

        private static bool IsPriceAndWeightCorrect(KnapsackSolution solution)
        {
            int actualPrice = 0;
            int actualWeight = 0;

            for (int i = 0; i < solution.ItemVector.Count; i++)
            {
                if (solution.ItemVector[i])
                {
                    actualPrice += solution.KnapsackInstance.Items[i].Price;
                    actualWeight += solution.KnapsackInstance.Items[i].Weight;
                }
            }

            if (actualPrice == solution.SolutionPrice &&
                actualWeight == solution.SolutionWeight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
