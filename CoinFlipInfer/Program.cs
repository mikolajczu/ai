using Microsoft.ML.Probabilistic.Models;

class Program
{
    private static void Main(string[] args)
    {
        // Sample data for coin flips
        var coins_data = new int[100];
        var rand = new Random();
        for (var i = 0; i < coins_data.Length; i++)
            coins_data[i] = rand.Next(2); // 0 represents tails, 1 represents heads

        // Probability variable for coin flip
        var probability = Variable.Beta(1, 1);

        foreach (var t in coins_data)
        {
            var x = Variable.Bernoulli(probability);
            x.ObservedValue = t == 1; // true if heads, false if tails
        }

        var engine = new InferenceEngine();
        Console.WriteLine("probability = " + engine.Infer(probability));
        // Posterior distribution of the probability variable, given the observed data
    }
}