using System.Reflection;
using System.Diagnostics;
class Program
{
    static Random rnd = new Random();
    static void Main(string[] args)
    {
        int cities = 100;
        long proby = 0;
        long zle_proby = 0;
        //tworzymy nową tablicę dla 100 miast i wypełniamy ją losowymi kosztami przejazdu
        int[,] travel_cost = GenerateTravelCost(cities);

        //tworzymy tablicę kolejności przebycia miast 1 do 100
        int[] numbers = new int[100];
        for (int i = 0; i < 100; i++)
        {
            numbers[i] = i + 1;
        }
        int[] suboptimal_solution = new int[100];
        int suboptimal_cost = 6000;

        // Odliczanie czasu działania pętli
        Stopwatch time = new Stopwatch();
        time.Start(); // rozpoczyna odliczanie
        TimeSpan time_limit = TimeSpan.FromMinutes(2);

    ShuffleParent:
        // Tworzymy rodzica - losową kolejność przebycia 100 miast - 500 inwersji na indeksach wypełnionych od 1 do 100
        int[] parent = Inversion(numbers, 500);

    InverseChild:
        if (time.Elapsed > time_limit)
        {
            Console.WriteLine("Czas upłynął. Najlepsze znalezione rozwiązanie to:");
            foreach (int city in suboptimal_solution)
            {
                Console.Write($"{city} ->");
            }
            Console.WriteLine("");
            Console.WriteLine($"Koszty podróży wyniosą: {suboptimal_cost}");
            return;
        }

        proby++;
        Console.WriteLine($"Próba: {proby}");

        int parent_travel_cost = CountTravelCosts(parent, travel_cost);
        Console.WriteLine($"Aktualny koszt podróży: {parent_travel_cost}");
        int[] child = CloneArray(parent);
        child = Inversion(child, 1);
        int child_travel_cost = CountTravelCosts(child, travel_cost);

        //sprawdzamy wymagania i zapisujemy najlepsze dotychczas rozwiązanie
        if (child_travel_cost < parent_travel_cost)
        {
            if (child_travel_cost < suboptimal_cost)
            {
                suboptimal_solution = CloneArray(child);
                suboptimal_cost = child_travel_cost;
            }
            parent = CloneArray(child);
            goto InverseChild;
        }
        else
        {
            if (parent_travel_cost < suboptimal_cost)
            {
                suboptimal_solution = CloneArray(parent);
                suboptimal_cost = parent_travel_cost;
            }
            zle_proby++;
            if (zle_proby % 2000 == 0) // Co 2000 prób
            {
                Console.WriteLine("Generowanie nowego rodzica.");
                goto ShuffleParent;
            }
            goto InverseChild;
        }
    }
    static int[,] GenerateTravelCost(int cities)
    {
        int[,] travel_cost = new int[cities, cities];
        for (int i = 0; i < cities; i++)
        {
            for (int j = 0; j < cities; j++)
            {
                if (i == j)
                {
                    travel_cost[i, j] = 0;
                }
                else
                {
                    travel_cost[i, j] = rnd.Next(10, 91);
                }
            }
        }
        return travel_cost;
    }
    static int[] Inversion(int[] numbers, int inverse_count)
    {
        for (int i = 0; i < inverse_count; i++)
        {
            int index1 = rnd.Next(0, 100);
            int index2 = rnd.Next(0, 100);

            int temp = numbers[index1];
            numbers[index1] = numbers[index2];
            numbers[index2] = temp;
        }
        return numbers;
    }

    static int CountTravelCosts(int[] travel_order, int[,] travel_costs)
    {
        int cost = 0;
        for (int i = 0; i < travel_order.Length - 1; i++)
        {
            cost += travel_costs[travel_order[i] - 1, travel_order[i + 1] - 1];
        }
        return cost;
    }
    static int[] CloneArray(int[] base_array)
    {
        int[] newArray = new int[100];
        for (int i = 0; i < newArray.Length; i++)
        {
            newArray[i] = base_array[i];
        }
        return newArray;
    }
}