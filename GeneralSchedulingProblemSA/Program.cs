namespace GeneralSchedulingProblemSA;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter the job weights separated by spaces:");
        var jobs = Console.ReadLine().Split().Select(int.Parse).ToArray();

        Console.WriteLine("Enter the number of processors:");
        var processorCount = int.Parse(Console.ReadLine());

        var problem = new SchedulingProblem(jobs, processorCount);
        problem.ScheduleJobs();
    }
}
