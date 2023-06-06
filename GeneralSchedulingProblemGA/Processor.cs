namespace GeneralSchedulingProblemGA;

public class Processor
{
    public List<int> Jobs { get; set; }

    public Processor()
    {
        Jobs = new List<int>();
    }

    public int Weight(int[] jobs)
    {
        return Jobs.Sum(jobIndex => jobs[jobIndex]);
    }
}