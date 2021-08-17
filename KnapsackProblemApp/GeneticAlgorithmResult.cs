using KnapsackProblemCore.Strategies.GeneticStrategies;

namespace KnapsackProblemApp
{
    public class GeneticAlgorithmResult
    {
        public int InstanceSize { get; set; }
        public string SelectionMethod { get; set; }
        public string CrossoverMethod { get; set; }
        public int PopulationSize { get; set; }
        public int MaxGenerations { get; set; }
        public double SelectionRate { get; set; }
        public double MutationRate { get; set; }
        public double AvgRelativeError { get; set; }
        public double MaxRelativeError { get; set; }
        public double TimeElapsed { get; set; }

        public GeneticAlgorithmResult(GeneticStrategyParameters parameters)
        {
            SelectionMethod = parameters.SelectionMethod.ToString();
            CrossoverMethod = parameters.CrossoverMethod.ToString();
            PopulationSize = parameters.PopulationSize;
            MaxGenerations = parameters.MaxGenerations;
            SelectionRate = parameters.SelectionRate;
            MutationRate = parameters.MutationRate;
        }
    }
}
