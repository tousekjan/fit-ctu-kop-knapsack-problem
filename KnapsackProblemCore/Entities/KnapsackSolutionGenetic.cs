using System.Collections;
using System.Collections.Generic;

namespace KnapsackProblemCore.Entities
{
    public class KnapsackSolutionGenetic
    {
        public int SolutionWeight { get; set; }
        public int SolutionPrice { get; set; }
        public BitArray ItemVector { get; set; }
        public List<Generation> Generations { get; set; }

        public KnapsackSolutionGenetic()
        {
            Generations = new List<Generation>();
        }
    }
}
