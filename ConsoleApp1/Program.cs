namespace ConsoleApp1;

public class Program
{
    public static void Main()
    {
        double[] initialSolution = new double[] { 1, 2, 3, 4, 5 };
        double initialTemperature = 100;
        double coolingRate = 0.01;
        int numIterations = 1000;

        double result =
            SimulatedAnnealing.Exec(CostFunction, initialSolution, initialTemperature, coolingRate, numIterations);
        Console.WriteLine(result);
    }
    
    public static Func<double[], double> CostFunction = (solution) =>
    {
        double x = solution[0];
        return x * x;
    };

}