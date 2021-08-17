using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblemApp
{
    public class BenchmarkResultFPTAS
    {
        public int InstanceSize { get; set; }
        public double Epsilon { get; set; }
        public double MaximumRelativeError { get; set; }
        public double AverageTime { get; set; }

        public override string ToString()
        {
            return $"Size: {InstanceSize} | Epsilon: {Epsilon} | MaximumRelativeError: {MaximumRelativeError} | AverageTime: {AverageTime}";
        }
    }
}
