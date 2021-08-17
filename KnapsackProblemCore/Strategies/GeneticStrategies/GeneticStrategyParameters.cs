namespace KnapsackProblemCore.Strategies.GeneticStrategies
{
    public enum SelectionMethod
    {
        Tournament = 0,
    }

    public enum CrossoverMethod
    {
        TwoPoint = 0,
    }

    public class GeneticStrategyParameters
    {
        public int PopulationSize { get; set; } = 100;
        public double SelectionRate { get; set; } = 0.3;
        public double MutationRate { get; set; } = 0.05;
        public int MaxGenerations { get; set; } = 40;
        public int ElitismCount { get; set; } = 2;
        public SelectionMethod SelectionMethod { get; set; } = SelectionMethod.Tournament;
        public CrossoverMethod CrossoverMethod { get; set; } = CrossoverMethod.TwoPoint;
    }
}
