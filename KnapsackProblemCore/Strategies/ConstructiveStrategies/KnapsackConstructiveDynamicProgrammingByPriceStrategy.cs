using System;
using System.Collections.Generic;
using System.Linq;
using KnapsackProblemCore.Entities;

namespace KnapsackProblemCore.Strategies.ConstructiveStrategies
{
    public class KnapsackConstructiveDynamicProgrammingByPriceStrategy : IKnapsackConstructiveStrategy
    {
        public KnapsackSolution SolveKnapsackConstructive(KnapsackInstance knapsackInstance)
        {
            int items = knapsackInstance.Count;
            int maxPrice = knapsackInstance.Items.Sum(x => x.Price);

            int[,] table = new int[maxPrice + 1, items + 1];

            for (int item = 0; item <= items; item++)
            {
                for (int price = 0; price <= maxPrice; price++)
                {
                    if (price == 0)
                    {
                        continue;
                    }

                    if (item == 0)
                    {
                        table[price, item] = int.MaxValue;
                        continue;
                    }

                    KnapsackItem currentItem = knapsackInstance.Items[item - 1];

                    if (item == 1)
                    {
                        if (price == currentItem.Price)
                        {
                            table[price, item] = currentItem.Weight;
                        }
                        else
                        {
                            table[price, item] = int.MaxValue;
                        }

                        continue;
                    }

                    var lastWeight = table[price, item - 1];
                    int priceIndex = price - currentItem.Price;

                    if (priceIndex < 0)
                    {
                        if (lastWeight == int.MaxValue)
                        {
                            table[price, item] = int.MaxValue;
                        }
                        else
                        {
                            table[price, item] = lastWeight;
                        }
                    }   
                    else
                    {
                        if (table[priceIndex, item - 1] == int.MaxValue)
                        {
                            table[price, item] = Math.Min(lastWeight, table[priceIndex, item - 1]);
                        }
                        else
                        {
                            table[price, item] = Math.Min(lastWeight, table[priceIndex, item - 1] + currentItem.Weight);
                        }
                    }
                }
            }

            // solution in last column where weight is less than Capacity and has the maximum index
            int lastColumn = items - 1;
            int solutionPrice = 0;
            int solutionWeight = 0;
            int rowIndex;
            
            for (rowIndex = maxPrice - 1; rowIndex >= 0; rowIndex--)
            { 
                if (table[rowIndex, lastColumn] <= knapsackInstance.Capacity)
                {
                    solutionWeight = table[rowIndex, lastColumn];
                    solutionPrice = rowIndex;

                    break;
                }
            }

            var itemsAdded = new List<KnapsackItem>();
            int columnIndex = knapsackInstance.Count;

            while (rowIndex > 0)
            {
                if (table[rowIndex, columnIndex] == table[rowIndex, columnIndex - 1])
                {
                    columnIndex--;
                    continue;
                }

                var item = knapsackInstance.Items[columnIndex - 1];
                rowIndex -= item.Price;

                itemsAdded.Add(item);

                columnIndex--;
            }

            return new KnapsackSolution(knapsackInstance, solutionWeight, solutionPrice, itemsAdded, 0);
        }

        public override string ToString()
        {
            return "DynamicProgrammingPrice";
        }
    }
}
