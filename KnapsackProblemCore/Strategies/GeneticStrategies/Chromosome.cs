using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KnapsackProblemCore.Entities;

namespace KnapsackProblemCore.Strategies.GeneticStrategies
{
    public class Chromosome
    {
        private KnapsackInstance KnapsackInstance { get; set; }
        public BitArray Genotype { get; set; }
        public int Price { get; set; }
        public int Weight { get; set; }
        private readonly Random random = new Random();

        public Chromosome(BitArray bitArray, KnapsackInstance knapsackInstance)
        {
            KnapsackInstance = knapsackInstance;
            Genotype = new BitArray(bitArray);
            UpdatePriceWeight();
        }

        public int UpdatePriceWeight()
        {
            int currentPrice = 0;
            int currentWeight = 0;

            for (int i = 0; i < Genotype.Count; i++)
            {
                if (Genotype[i])
                {
                    KnapsackItem currentItem = KnapsackInstance.Items[i];
                    currentPrice += currentItem.Price;
                    currentWeight += currentItem.Weight;
                }
            }

            if (currentWeight <= KnapsackInstance.Capacity)
            {
                Price = currentPrice;
                Weight = currentWeight;
                return currentPrice;
            }
            else
            {
                Price = 0;
                Weight = currentWeight;
                return 0;
            }
        }

        public int GetFitness()
        {
            int solutionPrice = UpdatePriceWeight();

            if (solutionPrice > 0)
            {
                return solutionPrice;
            }
            else
            {
                return RelaxChromosome();
            }
        }

        private int RelaxChromosome()
        {
            var indexes = new List<int>(Genotype.Length);

            for (int i = 0; i < Genotype.Length; i++)
            {
                indexes.Add(i);
            }

            indexes = indexes.OrderBy(x => random.Next()).ToList();

            for (int i = 0; i < indexes.Count; i++)
            {
                if (Genotype[indexes[i]])
                {
                    Genotype[indexes[i]] = false;
                }

                UpdatePriceWeight();

                if (Weight <= KnapsackInstance.Capacity || Weight == 0)
                {
                    break;
                }
            }

            return Price;
        }
    }
}