using System;
using System.Collections.Generic;

class Job
{
    public int id;
    public int weight;
    public Job(int id, int weight)
    {
        this.id = id;
        this.weight = weight;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Input: S array of Jobs with weight and k as the number of processors
        List<Job> S = new List<Job>()
        {
            new Job(1, 3),
            new Job(2, 1),
            new Job(3, 4),
            new Job(4, 2)
        };
        int k = 2; // default number of processors
        
        // Sort jobs in decreasing order of their weights
        S.Sort((a, b) => b.weight - a.weight);
        
        // Create an array to store the completion time of each processor
        int[] completionTime = new int[k];
        
        // Assign the first k jobs to different processors
        for (int i = 0; i < k; i++)
        {
            completionTime[i] = S[i].weight;
            Console.WriteLine("Processor {0} starts Job {1} at time {2}", i + 1, S[i].id, completionTime[i] - S[i].weight);
        }
        
        // Assign the remaining jobs to the processor with the earliest completion time
        for (int i = k; i < S.Count; i++)
        {
            int minTime = completionTime[0];
            int minIndex = 0;
            for (int j = 1; j < k; j++)
            {
                if (completionTime[j] < minTime)
                {
                    minTime = completionTime[j];
                    minIndex = j;
                }
            }
            completionTime[minIndex] += S[i].weight;
            Console.WriteLine("Processor {0} starts Job {1} at time {2}", minIndex + 1, S[i].id, completionTime[minIndex] - S[i].weight);
        }
        
        // Output the completion time of the last job
        int maxTime = completionTime[0];
        for (int i = 1; i < k; i++)
        {
            if (completionTime[i] > maxTime)
            {
                maxTime = completionTime[i];
            }
        }
        Console.WriteLine("Total completion time: {0}", maxTime);
    }
}
