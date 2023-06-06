namespace GeneralSchedulingProblemGA;

public class SchedulingProblem
 {
     private int[] jobs;
     private List<Processor> processors;
     private int totalWeight;
     private static readonly Random RandomGenerator = new();

     public SchedulingProblem(int[] jobs, int processorCount)
     {
         this.jobs = jobs;
         processors = new List<Processor>(processorCount);

         // Initialize processors
         for (var i = 0; i < processorCount; i++)
             processors.Add(new Processor());

         totalWeight = 0;
     }

     public void ScheduleJobs()
     {
         // Initialize population
         var population = InitializePopulation(processors.Count, jobs.Length);

         // Genetic algorithm loop
         for (var generation = 1; generation <= 100; generation++)
         {
             // Evaluate fitness of each chromosome
             foreach (var chromosome in population)
                 CalculateFitness(chromosome);

             // Select parents for reproduction
             var parents = SelectParents(population);

             // Generate offspring through crossover and mutation
             var offspring = GenerateOffspring(parents);

             // Replace random chromosomes in the population with offspring
             ReplacePopulation(population, offspring);
         }

         // Get the best chromosome with the minimal total weight
         var bestChromosome = population.OrderBy(CalculateTotalWeight).First();

         // Clear previous job assignments and assign jobs based on the best chromosome
         foreach (var processor in processors)
             processor.Jobs.Clear();

         for (var i = 0; i < jobs.Length; i++)
         {
             var processorIndex = bestChromosome.Genes[i];
             processors[processorIndex].Jobs.Add(i);
         }

         // Print the scheduled jobs
         Console.WriteLine("Jobs scheduled:");
         for (var i = 0; i < processors.Count; i++)
         {
             Console.WriteLine($"Processor {i + 1}: {string.Join(", ", processors[i].Jobs.Select(j => jobs[j]))}");
         }

         // Calculate the total weight
         var totalWeight = CalculateTotalWeight(bestChromosome);
         Console.WriteLine($"Total Weight: {totalWeight}");
     }
     
     private List<Chromosome> InitializePopulation(int processorCount, int jobCount)
     {
         var population = new List<Chromosome>();

         for (var i = 0; i < 50; i++)
         {
             var genes = new int[jobCount];
             for (var j = 0; j < jobCount; j++)
                 genes[j] = RandomGenerator.Next(processorCount);
             
             var chromosome = new Chromosome(genes);
             population.Add(chromosome);
         }

         return population;
     }
     
     private int CalculateTotalWeight(Chromosome chromosome)
     {
         var processorWeights = new int[processors.Count];
         for (var i = 0; i < chromosome.Genes.Length; i++)
         {
             var processorIndex = chromosome.Genes[i];
             processorWeights[processorIndex] += jobs[i];
         }
         return processorWeights.Max();
     }

     private void CalculateFitness(Chromosome chromosome)
     {
         chromosome.Fitness = CalculateTotalWeight(chromosome);
     }

     private List<Chromosome> SelectParents(IReadOnlyList<Chromosome> population)
     {
         var parents = new List<Chromosome>();

         // Select two parents using tournament selection
         for (var i = 0; i < 2; i++)
         {
             var tournament = new List<Chromosome>();
             for (var j = 0; j < 5; j++) // Perform tournament selection with a tournament size of 5
             {
                 tournament.Add(population[RandomGenerator.Next(population.Count)]);
             }
             parents.Add(tournament.OrderBy(c => c.Fitness).First());
         }

         return parents;
     }

     private List<Chromosome> GenerateOffspring(IReadOnlyList<Chromosome> parents)
     {
         var offspring = new List<Chromosome>();

         // Perform crossover and mutation to generate offspring
         for (var i = 0; i < 50; i++) // Generate 50 offspring
         {
             var parent1 = parents[0];
             var parent2 = parents[1];

             var child = Crossover(parent1, parent2);
             Mutate(child);

             offspring.Add(child);
         }

         return offspring;
     }
     
     private Chromosome Crossover(Chromosome parent1, Chromosome parent2)
     {
         var childGenes = new int[parent1.Genes.Length];
         var crossoverPoint = RandomGenerator.Next(1, parent1.Genes.Length - 1);

         for (var i = 0; i < crossoverPoint; i++)
         {
             childGenes[i] = parent1.Genes[i];
         }

         for (var i = crossoverPoint; i < parent2.Genes.Length; i++)
         {
             childGenes[i] = parent2.Genes[i];
         }

         return new Chromosome(childGenes);
     }

     private void Mutate(Chromosome chromosome)
     {
         var mutationIndex = RandomGenerator.Next(chromosome.Genes.Length);
         var newProcessor = RandomGenerator.Next(processors.Count);

         chromosome.Genes[mutationIndex] = newProcessor;
     }

     private void ReplacePopulation(IList<Chromosome> population, IReadOnlyList<Chromosome> offspring)
     {
         // Replace random chromosomes in the population with offspring
         foreach (var t in offspring)
         {
             var replaceIndex = RandomGenerator.Next(population.Count);
             population[replaceIndex] = t;
         }
     }
 }