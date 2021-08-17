using System.Collections.Generic;
using System.Linq;
using KnapsackProblemCore.Entities;

namespace KnapsackProblemCore.Strategies.ConstructiveStrategies
{
    public class KnapsackConstructiveGreedyHeuristicReduxStrategy : IKnapsackConstructiveStrategy
    {
        public KnapsackSolution SolveKnapsackConstructive(KnapsackInstance knapsackInstance)
        {
            var greedyHeuristicStrategy = new KnapsackConstructiveGreedyHeuristicStrategy();
            KnapsackSolution greedyHeuristicSolution = greedyHeuristicStrategy.SolveKnapsackConstructive(knapsackInstance);

            var orderedItems = knapsackInstance.Items.OrderByDescending(x => x.Price);

            KnapsackSolution reduxHeuristicSolution = null;

            foreach (var item in orderedItems)
            {
                if (item.Weight <= knapsackInstance.Capacity)
                {
                    reduxHeuristicSolution = new KnapsackSolution(knapsackInstance, item.Weight, item.Price, new List<KnapsackItem> { item }, 0);
                    break;
                }
            }

            if (reduxHeuristicSolution == null)
            {
                reduxHeuristicSolution = new KnapsackSolution(knapsackInstance, 0, 0, new List<KnapsackItem>(), 0);
            }

            if (greedyHeuristicSolution.SolutionPrice > reduxHeuristicSolution.SolutionPrice)
            {
                return greedyHeuristicSolution;
            }
            else
            {
                return reduxHeuristicSolution;
            }
        }

        public override string ToString()
        {
            return "GreedyHeuristicRedux";
        }
    }
}
