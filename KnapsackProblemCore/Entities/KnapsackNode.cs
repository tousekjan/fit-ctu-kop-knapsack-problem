using System.Collections;
using System.Collections.Generic;

namespace KnapsackProblemCore.Entities
{
    public class KnapsackNode
    {
        public int Price { get; set; }
        public int Weight { get; set; }
        public int Level { get; set; }
        public double Bound { get; set; }
        public List<KnapsackItem> Items { get; set; }

        public KnapsackNode()
        {
            Items = new List<KnapsackItem>();
        }

        public KnapsackNode(KnapsackNode node)
        {
            Price = node.Price;
            Weight = node.Weight;
            Level = node.Level + 1;
            Bound = node.Bound;

            Items = new List<KnapsackItem>(node.Items);
        }
    }
}
