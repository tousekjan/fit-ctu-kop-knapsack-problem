namespace SATSolverApp
{
    public class ExperimentOutput
    {
        public string Parameter { get; set; }
        public double RelativeError { get; set; }
        public double SatisfiedFormulas { get; set; }
        public double AvgUnsatisfiedClauses { get; set; }
        public double MaxUnsatisfiedClauses { get; set; }
        public double Time { get; set; }
    }
}
