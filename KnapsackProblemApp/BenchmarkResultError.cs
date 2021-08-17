namespace KnapsackProblemApp
{
    public class BenchmarkResultError
    {
        public int InstanceSize { get; set; }
        public string Algorithm { get; set; }
        public double AverageRelativeError { get; set; }
        public double MaximumRelativeError { get; set; }

        public override string ToString()
        {
            return $"Size: {InstanceSize} | Algorithm: {Algorithm} | AverageRelativeError: {AverageRelativeError}";
        }
    }
}
