using System.Collections;

namespace KnapsackProblemCore.Entities
{
    public class KnapsackReferenceSolution
    {
        public int Id { get; }
        public int Count { get; }
        public int SolutionPrice { get; }
        public BitArray ItemVector { get; }

        public KnapsackReferenceSolution(int id, int count, int solutionPrice, BitArray itemVector)
        {
            Id = id;
            Count = count;
            SolutionPrice = solutionPrice;
            ItemVector = itemVector;
        }
    }
}
