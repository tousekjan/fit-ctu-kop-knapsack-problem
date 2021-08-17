using System.Collections.Generic;
using KnapsackProblemCore.Entities;

namespace KnapsackProblemCore.Strategies.Helpers
{
    public static class BoundCalculator
    {
        public static double CalculateBound(KnapsackInstance knapsackInstance, List<KnapsackItem> orderedItems, KnapsackNode node)
        {
            double weight = node.Weight;
            double bound = node.Price;

            var currentItem = new KnapsackItem();

            for (int i = node.Level; i < knapsackInstance.Count; i++)
            {
                currentItem = orderedItems[i];

                if (weight + currentItem.Weight > knapsackInstance.Capacity)
                {
                    break;
                }

                weight += currentItem.Weight;
                bound += currentItem.Price;
            }

            bound += (knapsackInstance.Capacity - weight) * currentItem.PriceToWeightRatio;

            return bound;
        }
    }
}
