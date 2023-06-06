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

    public void ScheduleJobs()
    {
        // Genetic Algorithm Parameters
        const int generations = 100;
        const double mutationRate = 0.05;
        const double eliteRate = 0.1;

        for (var generation = 0; generation < generations; generation++)
        {
            EvaluateFitness();

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
            if (totalWeight < processors[i].Weight(jobs))
                totalWeight = processors[i].Weight(jobs);
        }
        Console.WriteLine($"Total Weight: {totalWeight}");
    }

    private void EvaluateFitness()
    {
        foreach (var chromosome in population)
        {
            InitializeProcessors();

            var jobIndex = 0;
            foreach (var gene in chromosome)
            {
                var processor = jobIndex % processors.Count;
                processors[processor].Jobs.Add(gene);
                jobIndex++;
            }

            var currentWeight = processors.Max(p => p.Weight(jobs));
            if (currentWeight > totalWeight)
                totalWeight = currentWeight;
        }
    }


    private void InitializeProcessors()
    {
        foreach (var processor in processors)
            processor.Jobs.Clear();
    }

    private int[] GenerateChromosome()
    {
        var chromosome = Enumerable.Range(0, jobs.Length).ToArray();
        Shuffle(chromosome);
        return chromosome;
    }

    private void Shuffle<T>(T[] array)
    {
        var n = array.Length;
        while (n > 1)
        {
            var k = random.Next(n--);
            var temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    private int[] SelectParent()
    {
        var parent1 = population[random.Next(population.Count)];
        var parent2 = population[random.Next(population.Count)];
        return CalculateFitness(parent1) > CalculateFitness(parent2) ? parent1 : parent2;
    }

    private double CalculateFitness(int[] chromosome)
    {
        InitializeProcessors();

        for (var i = 0; i < chromosome.Length; i++)
        {
            var processor = i % processors.Count;
            processors[processor].Jobs.Add(chromosome[i]);
        }

        return processors.Max(p => p.Weight(jobs));
    }

    private List<int[]> SelectElite(int eliteCount)
    {
        var sortedPopulation = population.OrderByDescending(chromosome => CalculateFitness(chromosome)).ToList();
        return sortedPopulation.GetRange(0, eliteCount);
    }

    private int[] Crossover(int[] parent1, int[] parent2)
    {
        var crossoverPoint = random.Next(1, jobs.Length - 1);
        var offspring = new int[jobs.Length];

        Array.Copy(parent1, offspring, crossoverPoint);

        var remainingJobs = parent2.Where(job => !offspring.Contains(job)).ToArray();
        Array.Copy(remainingJobs, 0, offspring, crossoverPoint, remainingJobs.Length);

        return offspring;
    }

    private int[] Mutate(int[] chromosome)
    {
        var index1 = random.Next(jobs.Length);
        var index2 = random.Next(jobs.Length);

        var temp = chromosome[index1];
        chromosome[index1] = chromosome[index2];
        chromosome[index2] = temp;

        return chromosome;
    }

    private void SelectBestChromosome()
    {
        var bestChromosome = population.OrderByDescending(chromosome => CalculateFitness(chromosome)).First();
        InitializeProcessors();

        for (var i = 0; i < bestChromosome.Length; i++)
        {
            var processor = i % processors.Count;
            processors[processor].Jobs.Add(bestChromosome[i]);
        }
    }
}