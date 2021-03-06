using System.Collections.Generic;
using System.Linq;
using KnapsackProblemCore.Entities;
using KnapsackProblemCore.Strategies.Helpers;

namespace KnapsackProblemCore.Strategies.ConstructiveStrategies
{
    public class KnapsackConstructiveBranchAndBoundStrategy : IKnapsackConstructiveStrategy
    {
        public KnapsackSolution SolveKnapsackConstructive(KnapsackInstance knapsackInstance)
        {
            int visitedStates = 0;

            List<KnapsackItem> orderedItems = knapsackInstance
                                .Items
                                .OrderByDescending(x => x.PriceToWeightRatio)
                                .ToList();

            var bestNode = new KnapsackNode();
            var rootNode = new KnapsackNode();

            rootNode.Bound = BoundCalculator.CalculateBound(knapsackInstance, orderedItems, rootNode);
            rootNode.Level = -1;

            var queue = new Queue<KnapsackNode>();
            queue.Enqueue(rootNode);

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                if (currentNode.Bound > bestNode.Price && currentNode.Level < knapsackInstance.Count - 1)
                {
                    visitedStates++;

                    KnapsackNode withNode = new KnapsackNode(currentNode);
                    var item = orderedItems[withNode.Level];
                    withNode.Weight += item.Weight;

                    if (withNode.Weight <= knapsackInstance.Capacity)
                    {
                        withNode.Price += item.Price;
                        withNode.Bound = BoundCalculator.CalculateBound(knapsackInstance, orderedItems, withNode);
                        withNode.Items.Add(item);

                        if (withNode.Price > bestNode.Price)
                        {
                            bestNode = withNode;
                        }

                        if (withNode.Bound > bestNode.Price)
                        {
                            queue.Enqueue(withNode);
                        }
                    }

                    var withoutNode = new KnapsackNode(currentNode);
                    withoutNode.Bound = BoundCalculator.CalculateBound(knapsackInstance, orderedItems, withoutNode);

                    if (withoutNode.Bound > bestNode.Price)
                    {
                        queue.Enqueue(withoutNode);
                    }
                }
            }

            return new KnapsackSolution(knapsackInstance, bestNode.Weight, bestNode.Price, bestNode.Items, visitedStates);
        }
        public override string ToString()
        {
            return "BranchAndBound";
        }
    }
}
