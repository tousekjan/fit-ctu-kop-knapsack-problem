using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SATSolverApp.Entities;

namespace SATSolverApp.SAT
{
    public class SATSolver
    {
        private Population Population { get; set; }
        private Random Random { get; }
        private SATInstance Instance { get; }
        public GeneticStrategyParameters Parameters { get; }
        private Func<Chromosome> _selectionMethod { get; }
        private Func<Chromosome, Chromosome, Chromosome> _crossoverMethod { get; }
        public SATSolver(SATInstance instance, GeneticStrategyParameters parameters)
        {
            Parameters = parameters;
            Instance = instance;
            Population = new Population(Parameters.PopulationSize, instance);
            Random = new Random();

            if (Parameters.SelectionMethod == SelectionMethod.Tournament)
            {
                _selectionMethod = TournamentMethod;
            }
            else
            {
                _selectionMethod = RouletteMethod;
            }

            if (Parameters.CrossoverMethod == CrossoverMethod.SinglePoint)
            {
                _crossoverMethod = CrossoverSinglePoint;
            }
            else if (Parameters.CrossoverMethod == CrossoverMethod.TwoPoint)
            {
                _crossoverMethod = CrossoverTwoPoint;
            }
            else
            {
                _crossoverMethod = CrossoverUniform;
            }
        }

        public SATSolution SolveSAT()
        {
            var solution = new SATSolution();
            solution.InstanceId = Instance.InstanceId;

            //Console.WriteLine("Initialize");
            Population.InitializePopulation(Instance.VariableCount, Parameters.InitializatonMethod);
            //Population.Print();

            //Console.WriteLine("Evaluate");
            Population.EvaluatePopulation();
            //Population.Print();
            int generationId = 0;

            solution.Generations.Add(new Generation(Population.GetBestChromosome().Fitness, generationId, Population.GetBestWeight(), 
                Population.GetAverageFitness(), Population.GetSatisfiedClausesCount(), Population.AdaptiveMutationRate, Population.ASD));
            generationId++;

            while (generationId < Parameters.MaxGenerations)
            {
                //Console.WriteLine("Selection");
                RunSelection();
                //Population.Print();

                //Console.WriteLine("Crossover");
                RunCrossover();
                //Population.Print();

                //Console.WriteLine("Mutation");
                RunMutation();
                //Population.Print();

                //Console.WriteLine("Evaluation");
                Population.EvaluatePopulation();
                //Population.Print();

                solution.Generations.Add(new Generation(Population.GetBestChromosome().Fitness, generationId, Population.GetBestWeight(), 
                    Population.GetAverageFitness(), Population.GetSatisfiedClausesCount(), Population.AdaptiveMutationRate, Population.ASD));

                //Console.WriteLine($"Generation: {generationId} MaxPrice: {Population.GetBestWeight()} AvgPrice: {Population.GetAverageWeight()}");

                generationId++;
            }

            solution.Weight = Population.GetBestWeight();
            solution.IsSatisfied = Population.IsSatisfied;

            var bestChromosome = Population.GetBestChromosome();
            solution.UnsatisfiedClausesCount = Instance.ClausesCount - bestChromosome.SatisfiedClauses;
            solution.Literals = GenotypeToLiterals(bestChromosome.Genotype);

            return solution;
        }

        private List<int> GenotypeToLiterals(List<bool> genotype)
        {
            var literals = new List<int>();
            for (int i = 0; i < genotype.Count; i++)
            {
                if (genotype[i])
                {
                    literals.Add(i + 1);
                }
                else
                {
                    literals.Add((i + 1) * -1);
                }
            }
            return literals;
        }

        private void RunSelection()
        {
            var newPopulation = new Population(Population.PopulationSize, Instance);

            // Elitism - get k best individuals
            newPopulation.AddChromosomes(Population.GetElite(Parameters.ElitismCount));

            int countToSelect = (int)Math.Floor(Population.PopulationSize * Parameters.SelectionRate);

            for (int i = 0; i < countToSelect; i++)
            {
                Chromosome newChromosome = _selectionMethod();
                newPopulation.AddChromosome(newChromosome);
            }

            Population = newPopulation;
        }

        private Chromosome TournamentMethod()
        {
            int k = 2;
            List<Chromosome> selectedChromosomes = Population.GetRandomChromosomesFromPopulation(k);

            // Get winner of tournament
            return selectedChromosomes.OrderByDescending(x => x.Fitness).First();
        }

        private Chromosome RouletteMethod()
        {
            int sumOfFitness = Population.Chromosomes.Sum(x => x.Fitness);
            var relativeFitnesses = new List<double>();

            foreach (Chromosome chromosome in Population.Chromosomes)
            {
                relativeFitnesses.Add((double)chromosome.Fitness / sumOfFitness);
            }

            double actualFitness = 0;
            double random = Random.NextDouble();

            for (int i = 0; i < relativeFitnesses.Count; i++)
            {
                actualFitness += relativeFitnesses[i];

                if (actualFitness > random)
                {
                    return Population.Chromosomes[i];
                }
            }

            throw new Exception("Roulette error");
        }

        private void RunCrossover()
        {
            while (Population.Chromosomes.Count < Parameters.PopulationSize)
            {
                List<Chromosome> parents = Population.GetRandomChromosomesFromPopulation(2);
                Chromosome parent1 = parents[0];
                Chromosome parent2 = parents[1];

                Chromosome child = _crossoverMethod(parent1, parent2);

                Population.AddChromosome(child);
            }
        }

        private Chromosome CrossoverSinglePoint(Chromosome parent1, Chromosome parent2)
        {
            int threshold = Random.Next(Instance.VariableCount);

            var child = new bool[Instance.VariableCount];

            for (int i = 0; i < Instance.VariableCount; i++)
            {
                if (i < threshold)
                {
                    child[i] = parent1.Genotype[i];
                }
                else
                {
                    child[i] = parent2.Genotype[i];
                }
            }

            return new Chromosome(child.ToList(), Instance);
        }

        private Chromosome CrossoverTwoPoint(Chromosome parent1, Chromosome parent2)
        {
            int threshold1 = Random.Next(Instance.VariableCount);
            int threshold2 = Random.Next(threshold1, Instance.VariableCount);

            var child = new bool[Instance.VariableCount];

            for (int i = 0; i < Instance.VariableCount; i++)
            {
                if (i < threshold1)
                {
                    child[i] = parent1.Genotype[i];
                }
                else if (i >= threshold1 && i < threshold2)
                {
                    child[i] = parent2.Genotype[i];
                }
                else
                {
                    child[i] = parent1.Genotype[i];
                }
            }

            return new Chromosome(child.ToList(), Instance);
        }

        private Chromosome CrossoverUniform(Chromosome parent1, Chromosome parent2)
        {
            var bitArray = new bool[parent1.Genotype.Count];

            for (int i = 0; i < parent1.Genotype.Count; i++)
            {
                bool randomBit = Random.Next(0, 2) != 0;

                if (randomBit)
                {
                    bitArray[i] = parent1.Genotype[i];
                }
                else
                {
                    bitArray[i] = parent2.Genotype[i];
                }
            }

            return new Chromosome(bitArray.ToList(), Instance);
        }


        private void RunMutation()
        {
            Population.UpdateAdaptiveMutationRate(Parameters.MutationRate);

            for (int i = Parameters.ElitismCount; i < Population.PopulationSize; i++)
            {
                for (int j = 0; j < Instance.VariableCount; j++)
                {
                    if (Random.NextDouble() < Population.AdaptiveMutationRate)
                    {
                        ////Console.WriteLine($"Mutation Chromosome: {i} Bit: {j}");
                        Population.Chromosomes[i].Genotype[j] = !Population.Chromosomes[i].Genotype[j];

                        Population.Chromosomes[i].UpdateFitness();

                        if (Population.Chromosomes[i].IsSatisfied)
                        {
                            Population.IsSatisfied = true;
                        }
                    }
                }
            }
        }
    }
}
