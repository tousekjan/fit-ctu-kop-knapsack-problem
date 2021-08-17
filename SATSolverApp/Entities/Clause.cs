using System;
using System.Collections.Generic;

namespace SATSolverApp.Entities
{
    public class Clause
    {
        private readonly int _maxLiterals;
        List<int> Literals { get; set; }

        public Clause(int maxLiterals)
        {
            _maxLiterals = maxLiterals;
            Literals = new List<int>(maxLiterals);
        }

        public void AddLiteral(int literal)
        {
            Literals.Add(literal);
        }

        //public bool GetVariable(int variable)
        //{
        //    return Variables[Math.Abs(variable) - 1];
        //}

        public bool IsSatisfied(List<bool> variableVector)
        {
            for (int i = 0; i < Literals.Count; i++)
            {
                if (Literals[i] > 0 && variableVector[Math.Abs(Literals[i]) - 1])
                {
                    return true;
                }
                else if (Literals[i] < 0 && !variableVector[Math.Abs(Literals[i]) - 1])
                {
                    return true;
                }
            }

            return false;
        }
    }
}