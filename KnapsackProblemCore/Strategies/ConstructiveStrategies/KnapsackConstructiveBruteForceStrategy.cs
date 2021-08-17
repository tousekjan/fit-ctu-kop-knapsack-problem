using System;
using System.Collections;
using KnapsackProblemCore.Entities;

namespace KnapsackProblemCore.Strategies.ConstructiveStrategies
{
    public class KnapsackConstructiveBruteForceStrategy : IKnapsackConstructiveStrategy
    {
        public KnapsackSolution SolveKnapsackConstructive(KnapsackInstance knapsackInstance)
        {
            if (knapsackInstance.Type != KnapsackType.Constructive)
            {
                throw new ArgumentException("KnapsackInstance must be of type Constructive");
            }

            long permutationCount = (long)Math.Pow(2, knapsackInstance.Count);
            int visitedStates = 0;

            int bestPrice = 0;
            int bestWeight = 0;
            long bestPermutation = 0;

            for (long currentPermutation = 0; currentPermutation < permutationCount; currentPermutation++)
            {
                int currentPrice = 0;
                int currentWeight = 0;

                visitedStates++;
                for (int j = 0; j < knapsackInstance.Count; j++)
                {
                    if (currentPermutation.IsBitSet(j))
                    {
                        KnapsackItem currentItem = knapsackInstance.Items[j];
                        currentPrice += currentItem.Price;
                        currentWeight += currentItem.Weight;
                    }
                }

                if (currentWeight <= knapsackInstance.Capacity &&
                    currentPrice > bestPrice)
                {
                    bestPrice = currentPrice;
                    bestWeight = currentWeight;
                    bestPermutation = currentPermutation;
                }
            }

            var itemVector = new BitArray(knapsackInstance.Count);

            for (int i = 0; i < knapsackInstance.Count; i++)
            {
                itemVector[i] = bestPermutation.IsBitSet(i);
            }

            return new KnapsackSolution(knapsackInstance, bestWeight, bestPrice, itemVector, visitedStates);
        }

        public override string ToString()
        {
            return "BruteForce";
        }
    }
}
