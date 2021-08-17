using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblemCore.Entities
{
    public enum KnapsackType
    {
        Decision = 0,
        Constructive = 1
    }

    public class KnapsackInstance
    {
        /// <summary>
        /// Unique ID (positive)
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// Count of items - n
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// Capacity of the knapsack - M
        /// </summary>
        public int Capacity { get; }
        /// <summary>
        /// Minimum price of items - B
        /// </summary>
        public int MinimumPrice { get; }
        /// <summary>
        /// List of items in Knapsack
        /// </summary>
        public List<KnapsackItem> Items { get; }
        /// <summary>
        /// The type of Knapsack problem 
        /// </summary>
        public KnapsackType Type { get; }

        public KnapsackInstance(KnapsackType type, int id, int count, int capacity, int minimumPrice, List<KnapsackItem> items)
        {
            Type = type;
            Id = id;
            Count = count;
            Capacity = capacity;
            Items = items;

            if (type == KnapsackType.Decision)
            {
                MinimumPrice = minimumPrice;
            }
        }

        public override string ToString()
        {
            return $"{Id} {Count} {Capacity} {MinimumPrice} {SerializeItems()}\n";
        }

        private string SerializeItems()
        {
            var builder = new StringBuilder();

            foreach (var item in Items)
            {
                builder.Append($"{item.Weight} {item.Price} ");
            }

            return builder.ToString();
        }
    }
}
