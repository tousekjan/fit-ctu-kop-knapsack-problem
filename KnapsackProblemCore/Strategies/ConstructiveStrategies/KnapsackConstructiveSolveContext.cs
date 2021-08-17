using KnapsackProblemCore.Entities;

namespace KnapsackProblemCore.Strategies.ConstructiveStrategies
{
    public class KnapsackConstructiveSolveContext
    {
        private IKnapsackConstructiveStrategy _knapsackConstructiveStrategy;
        public KnapsackConstructiveSolveContext(IKnapsackConstructiveStrategy strategy)
        {
            _knapsackConstructiveStrategy = strategy;
        }

        public KnapsackSolution GetKnapsackSolution(KnapsackInstance knapsackInstance)
        {
            return _knapsackConstructiveStrategy.SolveKnapsackConstructive(knapsackInstance);
        }
    }
}
