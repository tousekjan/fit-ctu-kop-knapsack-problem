namespace KnapsackProblemApp
{
    public class BenchmarkResult
    {
        public int InstanceSize { get; set; }
        public string Algorithm { get; set; }
        public double MinimumTime{ get; set; }
        public double AverageTime { get; set; }
        public long MaximumTime { get; internal set; }
        public double VisitedStates { get; set; }

        public override string ToString()
        {
            return $"Size: {InstanceSize} | Algorithm: {Algorithm} | Time: {AverageTime} | MaxTime: {MaximumTime}";
        }
    }
}
