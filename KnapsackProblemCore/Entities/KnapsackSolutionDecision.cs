using System.Collections;

namespace KnapsackProblemCore.Entities
{
    public class KnapsackSolutionDecision : KnapsackSolution
    {
        public bool HasSolution { get; }

        public KnapsackSolutionDecision(
            KnapsackInstance knapsackInstance, 
            int solutionWeight, 
            int solutionPrice, 
            BitArray itemVector,
            int visitedStates, 
            bool hasSolution) 
            : base(knapsackInstance, solutionWeight, solutionPrice, itemVector, visitedStates)
        {
            HasSolution = hasSolution;
        }
    }
}
