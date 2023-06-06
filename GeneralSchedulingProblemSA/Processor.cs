namespace GeneralSchedulingProblemSA;

public class Processor
{
    public List<int> Jobs { get; }

    public Processor()
    {
        Jobs = new List<int>();
    }

    public int Weight(IEnumerable<int> jobWeights)
    {
        return Jobs.Sum(jobWeights.ElementAt);
    }
}