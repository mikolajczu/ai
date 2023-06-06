namespace GeneralSchedulingProblemGAF;

public class SchedulingProblem
{
    private int[] jobs;
    private List<Processor> processors;
    private int totalWeight;
    private Random random;
    private List<int[]> population;

    public SchedulingProblem(int[] jobs, int processorCount, int populationSize)
    {
        this.jobs = jobs;
        processors = new List<Processor>(processorCount);
        totalWeight = 0;
        random = new Random();
        population = new List<int[]>();

        // Initialize the processors
        for (var i = 0; i < processorCount; i++)
            processors.Add(new Processor());

        // Generate initial population
        for (var i = 0; i < populationSize; i++)
        {
            var chromosome = GenerateChromosome();
            population.Add(chromosome);
        }
    }

    private int[] GenerateChromosome()
    {
        int[] chromosome = new int[jobs.Length];
        for (int i = 0; i < jobs.Length; i++)
        {
            int processorIndex = random.Next(processors.Count);
            chromosome[i] = processorIndex;
        }
        return chromosome;
    }

    private void EvaluateFitness()
    {
        foreach (var processor in processors)
            processor.Jobs.Clear();

        foreach (var chromosome in population)
        {
            for (int i = 0; i < chromosome.Length; i++)
            {
                int jobIndex = i;
                int processorIndex = chromosome[i];
                processors[processorIndex].Jobs.Add(jobs[jobIndex]);
            }
        }
    }

    private List<int[]> SelectElite(int eliteCount)
    {
        population.Sort((a, b) =>
        {
            int fitnessA = CalculateFitness(a);
            int fitnessB = CalculateFitness(b);
            return fitnessB.CompareTo(fitnessA);
        });

        return population.GetRange(0, eliteCount);
    }

    private int[] SelectParent()
    {
        int index = random.Next(population.Count);
        return population[index];
    }

    private int[] Crossover(int[] parent1, int[] parent2)
    {
        int crossoverPoint = random.Next(1, parent1.Length - 1);
        int[] child = new int[parent1.Length];

        for (int i = 0; i < crossoverPoint; i++)
        {
            child[i] = parent1[i];
        }

        for (int i = crossoverPoint; i < parent2.Length; i++)
        {
            child[i] = parent2[i];
        }

        return child;
    }

    private int[] Mutate(int[] chromosome)
    {
        int mutationPoint = random.Next(0, chromosome.Length);
        int newProcessorIndex = random.Next(processors.Count);
        chromosome[mutationPoint] = newProcessorIndex;
        return chromosome;
    }

    private int CalculateFitness(int[] chromosome)
    {
        EvaluateFitness();
        int fitness = 0;
        foreach (var processor in processors)
        {
            int processorWeight = processor.Weight(jobs);
            if (processorWeight > fitness)
                fitness = processorWeight;
        }
        return fitness;
    }

    private void SelectBestChromosome()
    {
        int bestFitness = 0;
        int[] bestChromosome = null;

        foreach (var chromosome in population)
        {
            int fitness = CalculateFitness(chromosome);
            if (fitness > bestFitness)
            {
                bestFitness = fitness;
                bestChromosome = chromosome;
            }
        }

        foreach (var processor in processors)
            processor.Jobs.Clear();

        for (int i = 0; i < bestChromosome.Length; i++)
        {
            int jobIndex = i;
            int processorIndex = bestChromosome[i];
            processors[processorIndex].Jobs.Add(jobs[jobIndex]);
        }

        totalWeight = bestFitness;
    }

    public void ScheduleJobs()
    {
        // Genetic Algorithm Parameters
        const int generations = 100;
        const double mutationRate = 0.05;
        const double eliteRate = 0.1;

        for (var generation = 0; generation < generations; generation++)
        {
            var newPopulation = new List<int[]>();

            var eliteCount = (int)Math.Ceiling(population.Count * eliteRate);
            var elite = SelectElite(eliteCount);

            newPopulation.AddRange(elite);

            while (newPopulation.Count < population.Count)
            {
                var parent1 = SelectParent();
                var parent2 = SelectParent();

                var offspring = Crossover(parent1, parent2);

                if (random.NextDouble() < mutationRate)
                    offspring = Mutate(offspring);

                newPopulation.Add(offspring);
            }

            population = newPopulation;
        }

        SelectBestChromosome();

        Console.WriteLine("Jobs scheduled:");
        for (var i = 0; i < processors.Count; i++)
        {
            Console.WriteLine($"Processor {i + 1}: {string.Join(", ", processors[i].Jobs.Select(j => jobs[j]))}");
        }
        Console.WriteLine($"Total Weight: {totalWeight}");
    }
}
