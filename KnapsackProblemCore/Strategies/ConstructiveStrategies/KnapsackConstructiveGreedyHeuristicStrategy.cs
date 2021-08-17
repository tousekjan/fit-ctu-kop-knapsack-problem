using System.Collections.Generic;
using System.Linq;
using KnapsackProblemCore.Entities;

namespace KnapsackProblemCore.Strategies.ConstructiveStrategies
{
    public class KnapsackConstructiveGreedyHeuristicStrategy : IKnapsackConstructiveStrategy
    {
        public KnapsackSolution SolveKnapsackConstructive(KnapsackInstance knapsackInstance)
        {
            List<KnapsackItem> orderedItems = knapsackInstance
                                .Items
                                .OrderByDescending(x => x.PriceToWeightRatio)
                                .ToList();

            int solutionPrice = 0;
            int solutionWeight = 0;
            var addedItems = new List<KnapsackItem>();

            foreach (var item in orderedItems)
            {
                if (solutionWeight == knapsackInstance.Capacity)
                {
                    break;
                }

                if (item.Weight + solutionWeight <= knapsackInstance.Capacity)
                {
                    solutionPrice += item.Price;
                    solutionWeight += item.Weight;

                    addedItems.Add(item);
                }
            }

            return new KnapsackSolution(knapsackInstance, solutionWeight, solutionPrice, addedItems, 0);
        }

        public override string ToString()
        {
            return "GreedyHeuristic";
        }
    }
}
