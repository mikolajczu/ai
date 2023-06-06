using System;
using System.Collections.Generic;

class Program
{
    static Random random = new Random();

    static void Main(string[] args)
    {
        int[] jobs = { 3, 2, 6 };
        int numProcessors = 2;

        List<List<int>> population = GeneratePopulation(jobs.Length, numProcessors);

        int generations = 100;
        int populationSize = 50;
        double mutationRate = 0.1;

        for (int i = 0; i < generations; i++)
        {
            population = EvolvePopulation(population, jobs, mutationRate);
        }

        List<int> bestSchedule = GetBestSchedule(population, jobs);
        int totalWeight = CalculateTotalWeight(bestSchedule, jobs);

        Console.WriteLine("Best Schedule: " + string.Join(", ", bestSchedule));
        Console.WriteLine("Total Weight: " + totalWeight);
    }

    static List<List<int>> GeneratePopulation(int numJobs, int numProcessors)
    {
        List<List<int>> population = new List<List<int>>();

        for (int i = 0; i < numProcessors; i++)
        {
            List<int> processorSchedule = new List<int>();

            for (int j = 0; j < numJobs; j++)
            {
                processorSchedule.Add(random.Next(2)); // 0 or 1 (job assigned or not)
            }

            population.Add(processorSchedule);
        }

        return population;
    }

    static List<List<int>> EvolvePopulation(List<List<int>> population, int[] jobs, double mutationRate)
    {
        List<List<int>> newPopulation = new List<List<int>>();

        // Select parents
        for (int i = 0; i < population.Count; i++)
        {
            List<int> parent1 = SelectParent(population, jobs);
            List<int> parent2 = SelectParent(population, jobs);

            // Crossover
            List<int> child = Crossover(parent1, parent2);

            // Mutation
            child = Mutate(child, mutationRate);

            newPopulation.Add(child);
        }

        return newPopulation;
    }

    static List<int> SelectParent(List<List<int>> population, int[] jobs)
    {
        // Tournament selection
        int tournamentSize = 5;
        List<int> tournament = new List<int>();

        for (int i = 0; i < tournamentSize; i++)
        {
            tournament.Add(random.Next(population.Count));
        }

        List<int> bestParent = null;
        int bestWeight = 0;

        foreach (int index in tournament)
        {
            List<int> parent = population[index];
            int weight = CalculateTotalWeight(parent, jobs);

            if (bestParent == null || weight > bestWeight)
            {
                bestParent = parent;
                bestWeight = weight;
            }
        }

        return bestParent;
    }

    static List<int> Crossover(List<int> parent1, List<int> parent2)
    {
        List<int> child = new List<int>();

        for (int i = 0; i < parent1.Count; i++)
        {
            if (random.NextDouble() < 0.5)
            {
                child.Add(parent1[i]);
            }
            else
            {
                child.Add(parent2[i]);
            }
        }

        return child;
    }

    static List<int> Mutate(List<int> schedule, double mutationRate)
    {
        for (int i = 0; i < schedule.Count; i++)
        {
            if (random.NextDouble() < mutationRate)
            {
                schedule[i] = 1 - schedule[i]; // Flip the bit
            }
        }

        return schedule;
    }

    static int CalculateTotalWeight(List<int> schedule, int[] jobs)
    {
        int totalWeight = 0;

        for (int i = 0; i < schedule.Count; i++)
        {
            if (schedule[i] == 1)
            {
                totalWeight += jobs[i];
            }
        }

        return totalWeight;
    }

    static List<int> GetBestSchedule(List<List<int>> population, int[] jobs)
    {
        List<int> bestSchedule = null;
        int bestWeight = 0;

        foreach (List<int> schedule in population)
        {
            int weight = CalculateTotalWeight(schedule, jobs);

            if (bestSchedule == null || weight > bestWeight)
            {
                bestSchedule = schedule;
                bestWeight = weight;
            }
        }

        return bestSchedule;
    }
}
