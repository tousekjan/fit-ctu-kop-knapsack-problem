using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KnapsackProblemCore.Entities;

namespace KnapsackProblemCore.Strategies.ConstructiveStrategies
{
    public class KnapsackConstructiveDynamicProgrammingByWeightStrategy : IKnapsackConstructiveStrategy
    {
        public KnapsackSolution SolveKnapsackConstructive(KnapsackInstance knapsackInstance)
        {
            int columns = knapsackInstance.Capacity + 1;
            int rows = knapsackInstance.Count + 1;

            int[,] table = new int[rows, columns];

            for (int row = 1; row < rows; row++)
            {
                var item = knapsackInstance.Items[row - 1];
                for (int column = 0; column < columns; column++)
                {
                    if (item.Weight > column)
                    {
                        table[row, column] = table[row - 1, column];
                    }
                    else
                    {
                        table[row, column] = Math.Max(table[row - 1, column], 
                                                      table[row - 1, column - item.Weight] + item.Price);
                    }
                }
            }

            var takenItems = new List<KnapsackItem>();

            for (int row = rows - 1, column = columns - 1; row > 0; row--)
            {
                if (table[row, column] != table[row - 1, column])
                {
                    takenItems.Add(knapsackInstance.Items[row - 1]);
                    column -= knapsackInstance.Items[row - 1].Weight;
                }
            }

            return new KnapsackSolution(knapsackInstance, 
                                        takenItems.Select(x => x.Weight).Sum(), 
                                        takenItems.Select(x => x.Price).Sum(), 
                                        takenItems, 0);
        }

        public override string ToString()
        {
            return "DynamicProgrammingWeight";
        }
    }
}
