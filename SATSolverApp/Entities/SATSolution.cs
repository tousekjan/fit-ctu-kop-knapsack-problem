using System.Collections.Generic;

namespace SATSolverApp.Entities
{
    public class SATSolution
    {
        public string InstanceId { get; set; }
        public int Weight { get; set; }
        public bool IsSatisfied { get; set; }
        public int UnsatisfiedClausesCount { get; set; }
        public List<int> Literals { get; set; } = new List<int>();
        public List<Generation> Generations { get; set; } = new List<Generation>();
    }
}
