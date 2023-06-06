namespace GeneralSchedulingProblemSA;

public class SchedulingProblem
{
    private int[] jobs;
    private List<Processor> processors;
    private int totalWeight;

    public SchedulingProblem(int[] jobs, int processorCount)
    {
        this.jobs = jobs;
        processors = new List<Processor>(processorCount);
        totalWeight = 0;
    }

    public void ScheduleJobs()
    {
        // Simulated Annealing Parameters
        const double initialTemperature = 1000;
        const double coolingRate = 0.95;

        var random = new Random();

        // Initialize the processors randomly
        for (var i = 0; i < processors.Capacity; i++)
            processors.Add(new Processor());

        for (var i = 0; i < jobs.Length; i++)
        {
            var processor = random.Next(processors.Count);
            processors[processor].Jobs.Add(i);
        }

        // Simulated Annealing
        var temperature = initialTemperature;

        while (temperature > 1)
        {
            // Randomly select a job to move
            var jobIndex = random.Next(jobs.Length);
            var currentProcessor = FindProcessorForJob(jobIndex);

            // Randomly select a processor to move the job to except currentProcessor
            int newProcessor;
            do
            {
                newProcessor = random.Next(processors.Count);
            } while (newProcessor == currentProcessor);

            // Calculate the new total weight after moving the job
            var oldProcessorWeight = processors[currentProcessor].Weight(jobs);
            var newProcessorWeight = processors[newProcessor].Weight(jobs) + jobs[jobIndex];

            var diffWeight = newProcessorWeight - oldProcessorWeight;

            // Calculate the acceptance probability
            var acceptanceProbability = Math.Exp(-diffWeight / temperature);

            if (diffWeight < 0 || random.NextDouble() < acceptanceProbability)
            {
                // Move the job to the new processor
                processors[currentProcessor].Jobs.Remove(jobIndex);
                processors[newProcessor].Jobs.Add(jobIndex);
            }

            // Cool down the temperature
            temperature *= coolingRate;
        }

        Console.WriteLine("Jobs scheduled:");
        for (var i = 0; i < processors.Count; i++)
        {
            Console.WriteLine($"Processor {i + 1}: {string.Join(", ", processors[i].Jobs.Select(j => jobs[j]))}");
            if (totalWeight < processors[i].Weight(jobs))
                totalWeight = processors[i].Weight(jobs);
        }
        Console.WriteLine($"Total Weight: {totalWeight}");
    }

    private int FindProcessorForJob(int jobIndex)
    {
        for (var i = 0; i < processors.Count; i++)
            if (processors[i].Jobs.Contains(jobIndex))
                return i;
        
        return 0;
    }
}