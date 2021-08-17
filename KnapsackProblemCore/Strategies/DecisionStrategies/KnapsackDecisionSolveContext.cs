using System;
using System.Collections.Generic;
using System.Text;
using KnapsackProblemCore.Entities;

namespace KnapsackProblemCore.Strategies.DecisionStrategies
{
    public class KnapsackDecisionSolveContext
    {
        private IKnapsackDecisionStrategy _knapsackDecisionStrategy;
        public KnapsackDecisionSolveContext(IKnapsackDecisionStrategy strategy)
        {
            _knapsackDecisionStrategy = strategy;
        }

        public KnapsackSolutionDecision GetKnapsackSolution(KnapsackInstance knapsackInstance)
        {
            return _knapsackDecisionStrategy.SolveKnapsackDecision(knapsackInstance);
        }
    }
}
