namespace Lab02;

public static class Task07
{
    public static void Run()
    {
        int n=int.Parse(Console.ReadLine()!);
        string[] names=new string[n];
        double[] bmis=new double[n];
        for (int i = 0; i < n; i++)
        {
            names[i] = Console.ReadLine()!;
            bmis[i] = double.Parse(Console.ReadLine()!);
        }

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n - 1 - i; j++)
            {
                if (bmis[j] < bmis[j + 1])
                {
                    double tempBMI=bmis[j];
                    bmis[j] = bmis[j + 1];
                    bmis[j + 1] = tempBMI;
                    string tempName=names[j];
                    names[j] = names[j+1];
                    names[j+1] = tempName;
                }
            }
        }
        Console.WriteLine("=== Рейтинг ІМТ ===");
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"#{i + 1} {names[i]}: {bmis[i]:F2}");
        }
    }
}