using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnapsackProblemCore.Entities;

namespace KnapsackProblemCore.Strategies.GeneticStrategies
{
    public class Population
    {
        private KnapsackInstance KnapsackInstance { get; set; }
        private Random Random { get; }
        public int PopulationSize { get; }
        public List<Chromosome> Chromosomes { get; set; }
        public Population(int populationSize, KnapsackInstance knapsackInstance)
        {
            Random = new Random();
            PopulationSize = populationSize;
            KnapsackInstance = knapsackInstance;
            Chromosomes = new List<Chromosome>();
        }


        internal int GetBestPrice()
        {
            return Chromosomes[0].Price;
        }

        internal int GetBestWeight()
        {
            return Chromosomes[0].Weight;
        }

        internal Chromosome GetBestChromosome()
        {
            return Chromosomes[0];
        }

        internal double GetAveragePopulationPrice()
        {
            return Chromosomes.Average(x => x.Price);
        }

        internal void InitializePopulation(int itemsCount)
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                var bitArray = new BitArray(itemsCount);
                for (int j = 0; j < itemsCount; j++)
                {
                    bitArray[j] = Random.Next(0, 2) != 0;
                }

                AddChromosome(bitArray);
            }
        }

        public void AddChromosome(BitArray chromosome)
        {
            Chromosomes.Add(new Chromosome(chromosome, KnapsackInstance));
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

            Chromosomes = Chromosomes.OrderByDescending(x => x.GetFitness()).ToList();

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

                for (int j = 0; j < KnapsackInstance.Count; j++)
                {
                    Console.Write($"{Convert.ToInt32(Chromosomes[i].Genotype[j])}");
                }

                Console.Write($" | {Chromosomes[i].GetFitness()} {Environment.NewLine}");
            }
        }
    }
}
