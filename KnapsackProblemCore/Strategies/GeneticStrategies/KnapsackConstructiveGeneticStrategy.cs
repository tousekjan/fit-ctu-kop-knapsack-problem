using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnapsackProblemCore.Entities;

namespace KnapsackProblemCore.Strategies.GeneticStrategies
{
    public class KnapsackConstructiveGeneticStrategy
    {
        private Population Population { get; set; }
        private Random Random { get; }
        private KnapsackInstance KnapsackInstance { get; }
        public GeneticStrategyParameters Parameters { get; }

        public KnapsackConstructiveGeneticStrategy(KnapsackInstance knapsackInstance, GeneticStrategyParameters parameters)
        {
            Parameters = parameters;
            KnapsackInstance = knapsackInstance;
            Population = new Population(Parameters.PopulationSize, knapsackInstance);
            Random = new Random();
        }

        public KnapsackSolutionGenetic SolveKnapsackConstructive()
        {
            var solution = new KnapsackSolutionGenetic();

            //Console.WriteLine("Initialize");
            Population.InitializePopulation(KnapsackInstance.Count);
            //Population.Print();

            //Console.WriteLine("Evaluate");
            Population.EvaluatePopulation();
            //Population.Print();

            int generationId = 0;
            
            while (generationId < Parameters.MaxGenerations)
            {
                //Console.WriteLine("Selection");
                RunSelection();
                //Population.Print();

                //Console.WriteLine("Crossover");
                RunCrossover();
                //Population.Print();

                //Console.WriteLine("Mutation");
                RunMutation();
                //Population.Print();

                //Console.WriteLine("Evaluation");
                Population.EvaluatePopulation();
                //Population.Print();

                int maxPrice = Population.GetBestPrice();
                double avgPrice = Population.GetAveragePopulationPrice();
                solution.Generations.Add(new Generation(generationId, maxPrice, avgPrice));

                //Console.WriteLine($"Generation: {generationId} MaxPrice: {maxPrice} AvgPrice: {avgPrice}");

                generationId++;
            }

            BitArray bestChromosome = Population.GetBestChromosome().Genotype;
            solution.SolutionPrice = Population.GetBestPrice();
            solution.SolutionWeight = Population.GetBestWeight();
            solution.ItemVector = bestChromosome;

            return solution;
        }

        private void RunSelection()
        {
            var newPopulation = new Population(Population.PopulationSize, KnapsackInstance);

            // Elitism - get k best individuals
            newPopulation.AddChromosomes(Population.GetElite(Parameters.ElitismCount));

            int countToSelect = (int)Math.Floor(Population.PopulationSize * Parameters.SelectionRate);

            Func<Chromosome> selectionMethod;

            if (Parameters.SelectionMethod == SelectionMethod.Tournament)
            {
                selectionMethod = TournamentMethod;
            }
            else
            {
                // Implement other selection methods
                selectionMethod = TournamentMethod;
            }

            for (int i = 0; i < countToSelect; i++)
            {
                Chromosome newChromosome = selectionMethod();
                newPopulation.AddChromosome(newChromosome);
            }

            Population = newPopulation;
        }

        private Chromosome TournamentMethod()
        {
            int k = 2;
            List<Chromosome> selectedChromosomes = Population.GetRandomChromosomesFromPopulation(k);

            // Get winner of tournament
            return selectedChromosomes.OrderByDescending(x => x.GetFitness()).First();
        }

        private void RunCrossover()
        {
            while (Population.Chromosomes.Count < Parameters.PopulationSize)
            {
                List<Chromosome> parents = Population.GetRandomChromosomesFromPopulation(2);
                Chromosome parent1 = parents[0];
                Chromosome parent2 = parents[1];

                Tuple<Chromosome, Chromosome> children = CrossoverTwoPoint(parent1, parent2);

                Population.AddChromosome(children.Item1);

                if (Population.Chromosomes.Count == Parameters.PopulationSize)
                    break;

                Population.AddChromosome(children.Item2);
            }
        }

        private Tuple<Chromosome, Chromosome> CrossoverTwoPoint(Chromosome parent1, Chromosome parent2)
        {
            int threshold1 = Random.Next(KnapsackInstance.Count);
            int threshold2 = Random.Next(threshold1, KnapsackInstance.Count);

            BitArray child1 = new BitArray(KnapsackInstance.Count);
            BitArray child2 = new BitArray(KnapsackInstance.Count);

            for (int i = 0; i < KnapsackInstance.Count; i++)
            {
                if (i < threshold1)
                {
                    child1[i] = parent1.Genotype[i];
                    child2[i] = parent2.Genotype[i];
                }
                else if (i >= threshold1 && i < threshold2)
                {
                    child1[i] = parent2.Genotype[i];
                    child2[i] = parent1.Genotype[i];
                }
                else
                {
                    child1[i] = parent1.Genotype[i];
                    child2[i] = parent2.Genotype[i];
                }
            }

            return new Tuple<Chromosome, Chromosome>(
                new Chromosome(child1, KnapsackInstance), 
                new Chromosome(child2, KnapsackInstance));
        }

        private void RunMutation()
        {
            for (int i = Parameters.ElitismCount; i < Population.PopulationSize; i++)
            {
                for (int j = 0; j < KnapsackInstance.Count; j++)
                {
                    if (Random.NextDouble() < Parameters.MutationRate)
                    {
                        ////Console.WriteLine($"Mutation Chromosome: {i} Bit: {j}");
                        Population.Chromosomes[i].Genotype[j] = !Population.Chromosomes[i].Genotype[j];

                        Population.Chromosomes[i].UpdatePriceWeight();
                    }
                }
            }
        }
    }
}
