using System;
using System.Collections.Generic;
using System.Linq;
using SATSolverApp.Entities;

namespace SATSolverApp.SAT
{
    internal class Population
    {
        private SATInstance Instance { get; set; }
        private Random Random { get; }
        public int PopulationSize { get; }
        public List<Chromosome> Chromosomes { get; set; }
        public bool IsSatisfied { get; set; }
        public double AdaptiveMutationRate { get; set; }
        public double ASD { get; set; }
        public Population(int populationSize, SATInstance instance)
        {
            IsSatisfied = false;
            Random = new Random();
            PopulationSize = populationSize;
            Instance = instance;
            Chromosomes = new List<Chromosome>();
        }

        internal int GetBestWeight()
        {
            return Chromosomes[0].Weight;
        }

        internal int GetSatisfiedClausesCount()
        {
            return Chromosomes[0].SatisfiedClauses;
        }

        internal Chromosome GetBestChromosome()
        {
            return Chromosomes[0];
        }

        internal double GetAverageFitness()
        {
            return Chromosomes.Average(x => x.Fitness);
        }

        internal void InitializePopulation(int populationSize, InitializatonMethod initializatonMethod)
        {
            if (initializatonMethod == InitializatonMethod.Random)
            {
                InitializePopulationRandom(populationSize);
            }
            else if (initializatonMethod == InitializatonMethod.OppositeCorners)
            {
                InitializePopulationOppositeCorners(populationSize);
            }
        }

        private void InitializePopulationRandom(int populationSize)
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                var newChromosome = InitRandomChromosome(populationSize);

                AddChromosome(newChromosome);
            }
        }

        private void InitializePopulationOppositeCorners(int populationSize)
        {
            var population = new List<Chromosome>(populationSize * 2);

            for (int i = 0; i < populationSize * 2; i++)
            {
                var randomChromosome = InitRandomChromosome(populationSize);
                var newChromosome = new Chromosome(randomChromosome, Instance);
                var inverseChromosome = new Chromosome(GetInverseChromosome(randomChromosome), Instance);

                newChromosome.UpdateFitness();
                inverseChromosome.UpdateFitness();

                population.Add(newChromosome);
                population.Add(inverseChromosome);
            }

            foreach (Chromosome chromosome in population)
            {
                AddChromosome(chromosome);
            }
        }

        private List<bool> GetInverseChromosome(List<bool> chromosome)
        {
            var inverseChromosome = new List<bool>();

            foreach (bool value in chromosome)
            {
                inverseChromosome.Add(!value);
            }

            return inverseChromosome;
        }

        private List<bool> InitRandomChromosome(int vectorSize)
        {
            var bitArray = new bool[vectorSize];
            for (int j = 0; j < vectorSize; j++)
            {
                bitArray[j] = Random.Next(0, 2) != 0;
            }

            return bitArray.ToList();
        }

        public void AddChromosome(List<bool> chromosome)
        {
            var newChromosome = new Chromosome(chromosome, Instance);
            
            if (newChromosome.IsSatisfied)
            {
                IsSatisfied = true;
            }

            Chromosomes.Add(new Chromosome(chromosome, Instance));
        }

        public void AddChromosome(Chromosome chromosome)
        {
            AddChromosome(chromosome.Genotype);
        }

        public void AddChromosomes(List<Chromosome> chromosomes)
        {
            foreach (var chromosome in chromosomes)
            {
                AddChromosome(chromosome);
            }
        }

        public void EvaluatePopulation()
        {
            //Console.WriteLine("EvaluatePopulation");

            //Chromosomes = Chromosomes
            //    .OrderByDescending(x => x.IsSatisfied)
            //    .ThenByDescending(x => x.SatisfiedClauses)
            //    .ThenByDescending(x => x.Weight)
            //    .ToList();

            Chromosomes = Chromosomes.OrderByDescending(x => x.Fitness).ToList();

            //PrintPopulation();
        }

        internal List<Chromosome> GetElite(int elitismCount)
        {
            return Chromosomes.GetRange(0, elitismCount);
        }

        internal List<Chromosome> GetRandomChromosomesFromPopulation(int k)
        {
            return Chromosomes
                .OrderBy(x => Random.Next())
                .ToList()
                .GetRange(0, k);
        }

        public void Print()
        {
            for (int i = 0; i < Chromosomes.Count; i++)
            {
                Console.Write($"Index: {i}\t| ");

                for (int j = 0; j < Instance.VariableCount; j++)
                {
                    Console.Write($"{Convert.ToInt32(Chromosomes[i].Genotype[j])}");
                }

                Console.Write($" | {Chromosomes[i].Fitness} {Environment.NewLine}");
            }
        }

        internal void UpdateAdaptiveMutationRate(double mutationRate)
        {
            double averageFitness = GetAverageFitness();
            
            ASD = ((double)1 / PopulationSize) * Math.Sqrt(Chromosomes.Select(x => Math.Pow(x.Fitness - averageFitness, 2)).Sum());

            double maxFitness = Chromosomes.Max(x => x.Fitness);

            AdaptiveMutationRate = mutationRate * (1 + (maxFitness - ASD) / (maxFitness + ASD));
        }
    }
}