using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SATSolverApp.Entities;

namespace SATSolverApp
{
    public static class SATDataLoader
    {
        public const int SAT3 = 3;
        public static SATInstance LoadInstance(string fileName)
        {
            var instance = new SATInstance();

            instance.InstanceId = Path.GetFileNameWithoutExtension(fileName).Split('-', StringSplitOptions.RemoveEmptyEntries)[1];

            IEnumerable<string> lines = File.ReadLines(fileName);

            foreach (string line in lines)
            {
                if (line.StartsWith('c'))
                {
                    continue;
                }
                else if(line.StartsWith('w'))
                {
                    var weights = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
                    weights.RemoveAll(x => x == "0");

                    foreach (string weight in weights)
                    {
                        int value = int.Parse(weight);

                        instance.Weights.Add(value);
                    }
                }
                else if (line.StartsWith('p'))
                {
                    var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(2).ToArray();

                    instance.VariableCount = int.Parse(data[0]);
                    instance.ClausesCount = int.Parse(data[1]);
                }
                else
                {
                    var literals = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                    literals.RemoveAll(x => x == "0");

                    var clause = new Clause(SAT3);

                    foreach (string literal in literals)
                    {
                        int literalValue = int.Parse(literal);

                        clause.AddLiteral(literalValue);
                    }

                    instance.Clauses.Add(clause);
                }
            }

            instance.SumWeights = instance.Weights.Sum();
            return instance;
        }

        internal static List<SATSolution> LoadSolutions(string fileName)
        {
            var solutions = new List<SATSolution>();

            IEnumerable<string> lines = File.ReadLines(fileName);

            foreach (string line in lines)
            {
                solutions.Add(ParseSolutionLine(line));
            }

            return solutions;
        }

        private static SATSolution ParseSolutionLine(string line)
        {
            var solution = new SATSolution();

            string[] data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();

            solution.InstanceId = data[0].Split('-', StringSplitOptions.RemoveEmptyEntries)[1];
            solution.Weight = int.Parse(data[1]);

            for (int i = 1; i < data.Length - 1; i++)
            {
                solution.Literals.Add(int.Parse(data[i]));
            }

            return solution;
        }
    }
}
