namespace Lab02;

public static class Task08
{
    public static void Run()
    {
        int d=int.Parse(Console.ReadLine()!);
        int w=int.Parse(Console.ReadLine()!);
        int[,,] data=new int[d,w,2];
        for (int i = 0; i < d; i++)
        {
            for (int j = 0; j < w; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    data[i, j, k] = int.Parse(Console.ReadLine()!);
                }
            }
        }

        int[] depTotals = new int[d];
        for (int i = 0; i < d; i++)
        {
            Console.WriteLine($"Відділення {i + 1}:");
            for (int j = 0; j < w; j++)
            {
                int morning = data[i, j, 0];
                int evening = data[i, j, 1];
                int weekTotal=morning + evening;
                Console.WriteLine($"  Тиждень {j + 1}: ранок {morning}, вечір {evening} → разом {weekTotal}");
                depTotals[i]+=weekTotal;
            }
            Console.WriteLine($"  Разом: {depTotals[i]} пацієнтів");
        }

        int maxIdx = 0;
        for (int i = 0; i < d; i++)
        {
            if (depTotals[i] > depTotals[maxIdx])
            {
                maxIdx = i;
            }
        }
        Console.WriteLine($"Найзавантаженіше: Відділення {maxIdx + 1} ({depTotals[maxIdx]} пацієнтів)");
    }
}