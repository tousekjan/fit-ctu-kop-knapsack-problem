using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KnapsackProblemCore.Entities;

namespace KnapsackProblemCore.Strategies.ConstructiveStrategies
{
    public class KnapsackConstructiveFPTASStrategy : IKnapsackConstructiveStrategy
    {
        private double _epsilon;

        public KnapsackConstructiveFPTASStrategy(double epsilon)
        {
            _epsilon = epsilon;
        }
        public KnapsackSolution SolveKnapsackConstructive(KnapsackInstance knapsackInstance)
        {
            List<KnapsackItem> filteredItems = knapsackInstance.Items
                    .Where(x => x.Weight <= knapsackInstance.Capacity)
                    .ToList();

            if (filteredItems.Count == 0)
            {
                return new KnapsackSolution(knapsackInstance, 0, 0, new BitArray(knapsackInstance.Count), 0);
            }

            var modifiedItems = filteredItems.ConvertAll(item => new KnapsackItem(item));

            int maxPrice = modifiedItems.Max(x => x.Price);

            double k = _epsilon * maxPrice / modifiedItems.Count();

            for (int i = 0; i < modifiedItems.Count; i++)
            {
                var item = modifiedItems[i];

                int roundedPrice = (int)Math.Floor(item.Price / k);

                if (roundedPrice == 0)
                {
                    roundedPrice = 1;
                }

                item.Price = roundedPrice;
                item.Id = i;
            }

            var reducedInstance = new KnapsackInstance(
                    KnapsackType.Constructive, 
                    knapsackInstance.Id,
                    modifiedItems.Count,
                    knapsackInstance.Capacity, 
                    knapsackInstance.MinimumPrice, 
                    modifiedItems);

            var solver = new KnapsackConstructiveSolveContext(new KnapsackConstructiveDynamicProgrammingByPriceStrategy());

            KnapsackSolution dynamicSolution = solver.GetKnapsackSolution(reducedInstance);

            KnapsackSolution solution = new KnapsackSolution(knapsackInstance, 0, 0, new BitArray(knapsackInstance.Count), 0);

            for (int i = 0; i < dynamicSolution.ItemVector.Count; i++)
            {
                if (dynamicSolution.ItemVector[i] == true)
                {
                    var item = filteredItems[i];
                    solution.SolutionPrice += item.Price;
                    solution.SolutionWeight += item.Weight;
                    solution.ItemVector[item.Id] = true;
                }
            }

            return solution;
        }

        public override string ToString()
        {
            return "FPTAS";
        }
    }
}
