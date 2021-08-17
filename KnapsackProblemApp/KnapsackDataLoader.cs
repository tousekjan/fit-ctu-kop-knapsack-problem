using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KnapsackProblemCore;
using KnapsackProblemCore.Entities;

namespace KnapsackProblemApp
{
    public static class KnapsackDataLoader
    {
        public static List<KnapsackInstance> LoadProblemInstances(List<string> fileNames)
        {
            var parsedProblemInstances = new List<KnapsackInstance>();

            foreach (var fileName in fileNames)
            {
                IEnumerable<string> lines = File.ReadLines(fileName);

                foreach (string line in lines)
                {
                    KnapsackInstance problemInstance = ParseProblemInstanceLine(line);

                    parsedProblemInstances.Add(problemInstance);
                }
            }

            return parsedProblemInstances;
        }

        public static List<KnapsackInstance> LoadProblemInstances(string instances)
        {
            var parsedProblemInstances = new List<KnapsackInstance>();

            IEnumerable<string> lines = instances.Split(
                                                    new[] { "\r\n", "\r", "\n" },
                                                    StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                KnapsackInstance problemInstance = ParseProblemInstanceLine(line);

                parsedProblemInstances.Add(problemInstance);
            }

            return parsedProblemInstances;
        }

        public static List<KnapsackReferenceSolution> LoadSolutions(List<string> fileNames)
        {
            var parsedSolutions = new List<KnapsackReferenceSolution>();

            foreach (var fileName in fileNames)
            {
                IEnumerable<string> lines = File.ReadLines(fileName);

                foreach (string line in lines)
                {
                    KnapsackReferenceSolution solution = ParseSolutionLine(line);

                    parsedSolutions.Add(solution);
                }
            }

            return parsedSolutions;
        }

        private static KnapsackInstance ParseProblemInstanceLine(string line)
        {
            string[] properties = line.Split(' ');

            int id = int.Parse(properties[0]);
            int count = int.Parse(properties[1]);
            int capacity = int.Parse(properties[2]);
            int minimumPrice = int.Parse(properties[3]);
            KnapsackType type;

            int propertiesToSkip;

            if (id < 0)
            {
                id = Math.Abs(id);
                type = KnapsackType.Decision;
                propertiesToSkip = 4;
            }
            else
            {
                type = KnapsackType.Constructive;
                propertiesToSkip = 3;
            }

            return new KnapsackInstance(type, id, count, capacity, minimumPrice, 
                ParseProblemInstanceItems(properties.Skip(propertiesToSkip).ToList()));
        }

        private static KnapsackReferenceSolution ParseSolutionLine(string line)
        {
            string[] properties = line.Split(' ');

            int id = int.Parse(properties[0]);
            int count = int.Parse(properties[1]);
            int solutionPrice = int.Parse(properties[2]);

            int propertiesToSkip = 3;

            return new KnapsackReferenceSolution(id, count, solutionPrice, 
                            ParseSolutionItems(properties
                                .Skip(propertiesToSkip)
                                .Where(x => !string.IsNullOrEmpty(x))
                                .ToList()));
        }

        private static List<KnapsackItem> ParseProblemInstanceItems(List<string> knapsackItems)
        {
            var parsedItems = new List<KnapsackItem>();

            for (int i = 0; i < knapsackItems.Count; i+=2)
            {
                int weight = int.Parse(knapsackItems[i]);
                int price = int.Parse(knapsackItems[i+1]);

                parsedItems.Add(new KnapsackItem(i/2, weight, price));
            }

            return parsedItems;
        }

        private static BitArray ParseSolutionItems(List<string> knapsackItems)
        {
            BitArray itemsVector = new BitArray(knapsackItems.Count);

            for (int i = 0; i < knapsackItems.Count; i++)
            {
                if (knapsackItems[i] == "1")
                {
                    itemsVector[i] = true;
                }
            }

            return itemsVector;
        }
    }
}
