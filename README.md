# NI-KOP Knapsack Problem

This project was implemented as semestral work at Czech Technical University, [Faculty of Information Technology](https://fit.cvut.cz/en).

The [Knapsack Problem](https://en.wikipedia.org/wiki/Knapsack_problem) is solved using multiple strategies: Brute Force, Branch and Bound, FPTAS, Dynamic programming, Greedy Heuristic and [Genetic (Evolutionary) algorithm](https://en.wikipedia.org/wiki/Genetic_algorithm).

Additionally Brute Force and Genetic algorithm are used to solve [SAT problem](https://en.wikipedia.org/wiki/Boolean_satisfiability_problem).
   
To build the project use `dotnet build`
To run default brute force algorithm use `.\KnapsackProblemApp.exe {PathToInputFiles} {Algorithm} {TimesToRepeatFile}`. Algorithm is "bf" or "bab". Mind, that some of the problem instances are very hard and the solution might take very long time!

To run other algorithm experiments Use ExperimentRunner or GeneticExperimentRunner classes.

### TODO
- [ ] Run example experiments by default when running from command line #feature

Warning: Some implemented experiments are dependent on Knapsack problem generator (kg2.exe) provided by the lecturer. If you want to experiment with the functionality either download some problem instances or implement your own generator.
