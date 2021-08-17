using System;
using System.Collections.Generic;
using System.Linq;
using SATSolverApp.Entities;

namespace SATSolverApp.SAT
{
    public class Chromosome
    {
        public int Fitness { get; set; }
        public int Weight { get; set; }
        private SATInstance Instance { get; set; }
        public List<bool> Genotype { get; set; }
        public bool IsSatisfied { get; set; }
        public int SatisfiedClauses { get; set; }

        public Chromosome(List<bool> bitArray, SATInstance instance)
        {
            IsSatisfied = false;
            Instance = instance;
            Genotype = new List<bool>(bitArray);
            UpdateFitness();
        }

        public void UpdateFitness()
        {
            bool isSatisfied = true;
            int satisfiedClausesCount = 0;

            foreach (Clause clause in Instance.Clauses)
            {
                if (clause.IsSatisfied(Genotype))
                {
                    satisfiedClausesCount++;
                }
                else
                {
                    isSatisfied = false;
                }
            }

            SatisfiedClauses = satisfiedClausesCount;

            IsSatisfied = isSatisfied;

            int weight = 0;

            for (int i = 0; i < Genotype.Count; i++)
            {
                if (Genotype[i])
                {
                    weight += Instance.Weights[i];
                }
            }

            Weight = weight;
            //Fitness = (isSatisfied ? 1 : 0) * 5000 + 100 * satisfiedClausesCount + weight;
            //Fitness = (isSatisfied ? 1 : 0) * Instance.ClausesCount * 1000 + 10 * satisfiedClausesCount + weight;
            Fitness = satisfiedClausesCount * Instance.SumWeights + weight;
        }

        //private int RelaxChromosome()
        //{
        //    var indexes = new List<int>(Genotype.Length);

        //    for (int i = 0; i < Genotype.Length; i++)
        //    {
        //        indexes.Add(i);
        //    }

        //    indexes = indexes.OrderBy(x => Random.Next()).ToList();

        //    for (int i = 0; i < indexes.Count; i++)
        //    {
        //        if (Genotype[indexes[i]])
        //        {
        //            Genotype[indexes[i]] = false;
        //        }

        //        UpdateFitness();

        //        if (Weight <= KnapsackInstance.Capacity || Weight == 0)
        //        {
        //            break;
        //        }
        //    }

        //    return Price;
        //}
    }
}