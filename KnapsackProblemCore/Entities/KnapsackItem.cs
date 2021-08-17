namespace KnapsackProblemCore.Entities
{
    public class KnapsackItem
    {
        public int Id { get; set; }
        public int Weight { get; set; }
        public int Price { get; set; }
        public double PriceToWeightRatio
        {
            get
            {
                return (double)Price / Weight;
            }
        }

        public KnapsackItem(int id, int weight, int price)
        {
            Id = id;
            Weight = weight;
            Price = price;
        }

        public KnapsackItem(KnapsackItem item)
        {
            Id = item.Id;
            Weight = item.Weight;
            Price = item.Price;
        }

        public KnapsackItem() {}
    }
}
