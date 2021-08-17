namespace SATSolverApp.Entities
{
    public class Generation
    {
        public int MaxFitness { get; set; }
        public double AverageFitness { get; set; }
        public int GenerationId { get; set; }
        public int BestWeight { get; set; }
        public int SatisfiedClauses { get; set; }
        public double AdaptiveMutationRate { get; set; }
        public double ASD { get; set; }

        public Generation(int fitness, int generationId, int maxWeight, double avgFitness, int unsatisfiedClauses, 
            double adaptiveMutationRate, double asd)
        {
            MaxFitness = fitness;
            GenerationId = generationId;
            BestWeight = maxWeight;
            AverageFitness = avgFitness;
            SatisfiedClauses = unsatisfiedClauses;
            AdaptiveMutationRate = adaptiveMutationRate;
            ASD = asd;
        }
    }
}