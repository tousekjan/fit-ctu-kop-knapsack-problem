using System;
using System.Collections.Generic;
using System.Text;

namespace SATSolverApp.Entities
{
    public class SATInstance
    {
        public string InstanceId { get; set; }
        public int VariableCount { get; set; }
        public int ClausesCount { get; set; }
        public List<int> Weights { get; set; }
        public int SumWeights { get; set; }
        public List<Clause> Clauses { get; set; }

        public SATInstance()
        {
            Weights = new List<int>();
            Clauses = new List<Clause>();
        }
    }
}
