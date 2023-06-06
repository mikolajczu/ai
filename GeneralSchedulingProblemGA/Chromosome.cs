namespace GeneralSchedulingProblemGA;

public class Chromosome
{
    public int[] Genes { get; set; }
    public int Fitness { get; set; }

    public Chromosome(int[] genes)
    {
        Genes = genes;
        Fitness = 0;
    }
}