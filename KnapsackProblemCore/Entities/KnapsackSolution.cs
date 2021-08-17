using System.Collections;
using System.Collections.Generic;

namespace KnapsackProblemCore.Entities
{
    public class KnapsackSolution
    {
        public KnapsackInstance KnapsackInstance { get; }
        public int SolutionWeight { get; set;}
        public int SolutionPrice { get; set; }
        public BitArray ItemVector { get; set; }
        public int VisitedStates { get; set; }

        public KnapsackSolution(KnapsackInstance knapsackInstance, int solutionWeight, int solutionPrice, BitArray itemVector, int visitedStates)
            :this(knapsackInstance, solutionWeight, solutionPrice, visitedStates)
        {
            ItemVector = itemVector;
        }

        public KnapsackSolution(KnapsackInstance knapsackInstance, int solutionWeight, int solutionPrice, IEnumerable<KnapsackItem> itemList, int visitedStates)
            : this(knapsackInstance, solutionWeight, solutionPrice, visitedStates)
        {
            ItemVector = new BitArray(knapsackInstance.Count);

            foreach (var item in itemList)
            {
                ItemVector[item.Id] = true;
            }
        }

        private KnapsackSolution(KnapsackInstance knapsackInstance, int solutionWeight, int solutionPrice, int visitedStates)
        {
            KnapsackInstance = knapsackInstance;
            SolutionWeight = solutionWeight;
            SolutionPrice = solutionPrice;
            VisitedStates = visitedStates;
        }
    }
}
