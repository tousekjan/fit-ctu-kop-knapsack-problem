using KnapsackProblemCore.Entities;

namespace KnapsackProblemCore.Strategies.DecisionStrategies
{
    public interface  IKnapsackDecisionStrategy
    {
        public KnapsackSolutionDecision SolveKnapsackDecision(KnapsackInstance knapsackInstance);
    }
}
