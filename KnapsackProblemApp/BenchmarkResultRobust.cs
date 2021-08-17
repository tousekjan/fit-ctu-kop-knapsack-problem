using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblemApp
{
    public class BenchmarkResultRobust
    {
        public double BFMinTime { get; set; }
        public double BFAvgTime { get; set; }
        public double BFMaxTime { get; set; }
        public double BaBMinTime { get; set; }
        public double BaBAvgTime { get; set; }
        public double BaBMaxTime { get; set; }
        public double HeuMinTime { get; set; }
        public double HeuAvgTime { get; set; }
        public double HeuMaxTime { get; set; }
        public double DPMinTime { get; set; }
        public double DPAvgTime { get; set; }
        public double DPMaxTime { get; set; }
    }
}
