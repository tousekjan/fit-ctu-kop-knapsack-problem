using System;
using System.Collections;
using KnapsackProblemCore.Entities;

namespace KnapsackProblemCore.Strategies.DecisionStrategies
{
    public class KnapsackDecisionBruteForceStrategy : IKnapsackDecisionStrategy
    {
        public KnapsackSolutionDecision SolveKnapsackDecision(KnapsackInstance knapsackInstance)
        {
            if (knapsackInstance.Type != KnapsackType.Decision)
            {
                throw new ArgumentException("KnapsackInstance must be of type Decision");
            }

            long permutationCount = (long)Math.Pow(2, knapsackInstance.Count);
            int visitedStates = 0;

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
                    currentPrice >= knapsackInstance.MinimumPrice)
                {
                    return new KnapsackSolutionDecision(knapsackInstance, currentWeight, currentPrice, new BitArray(0), visitedStates, true);
                }
            }

            return new KnapsackSolutionDecision(knapsackInstance, 0, 0, new BitArray(0), visitedStates, false);
        }

        public override string ToString()
        {
            return "KnapsackDecisionBranchAndBoundStrategy";
        }
    }
}
