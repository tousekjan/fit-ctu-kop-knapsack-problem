namespace KnapsackProblemCore.Entities
{
    public class Generation
    {
        public int GenerationId { get; set; }
        public int BestPrice { get; set; }
        public double AveragePrice { get; set; }

        public Generation(int generationId, int maxPrice, double avgPrice)
        {
            GenerationId = generationId;
            BestPrice = maxPrice;
            AveragePrice = avgPrice;
        }
    }
}
