using Microsoft.ML.Probabilistic.Models;

class Program
{
    static void Main(string[] args)
    {
        // Sample data for coin flips
        int[] coins_data = new int[100];
        Random rand = new Random();
        for (int i = 0; i < coins_data.Length; i++)
            coins_data[i] = rand.Next(2); // 0 represents tails, 1 represents heads

        // Create probability variable for coin flip
        Variable<double> probability = Variable.Beta(1, 1);

        for (int i = 0; i < coins_data.Length; i++)
        {
            Variable<bool> x = Variable.Bernoulli(probability);
            x.ObservedValue = coins_data[i] == 1; // true if heads, false if tails
        }

        InferenceEngine engine = new InferenceEngine();
        Console.WriteLine("probability=" + engine.Infer(probability));
    }
}