namespace ConsoleApp1;

public class SimulatedAnnealing
{
    public static double Exec(Func<double[], double> costFunction, double[] initialSolution, double initialTemperature, double coolingRate, int numIterations)
    {
        double[] currentSolution = initialSolution;
        double currentCost = costFunction(currentSolution);
        double temperature = initialTemperature;
        Random random = new Random();

        for (int i = 0; i < numIterations; i++)
        {
            double[] newSolution = GenerateNeighbor(currentSolution, temperature, random);
            double newCost = costFunction(newSolution);
            double acceptanceProbability = AcceptanceProbability(currentCost, newCost, temperature);

            if (acceptanceProbability > random.NextDouble())
            {
                currentSolution = newSolution;
                currentCost = newCost;
            }

            temperature *= 1 - coolingRate;
        }

        return currentCost;
    }

    private static double[] GenerateNeighbor(double[] solution, double temperature, Random random)
    {
        double[] newSolution = new double[solution.Length];

        for (int i = 0; i < solution.Length; i++)
        {
            double randomNumber = (random.NextDouble() * 2) - 1;
            newSolution[i] = solution[i] + (randomNumber * temperature);
        }

        return newSolution;
    }

    private static double AcceptanceProbability(double currentCost, double newCost, double temperature)
    {
        if (newCost < currentCost)
        {
            return 1;
        }

        return Math.Exp((currentCost - newCost) / temperature);
    }

}