namespace Lab01;

public static class Task07
{
    public static void Run()
    {
        int n=int.Parse(Console.ReadLine()!);
        decimal[] costs = new decimal[n];
        for (int i = 0; i < n; i++)
        {
            costs[i] = decimal.Parse(Console.ReadLine()!);
        } 
        decimal sum = 0; 
        decimal min = costs[0];
        decimal max = costs[0];
        foreach (decimal cost in costs)
        {
            sum += cost;
            if(cost<min) 
            {
                min = cost;
            }
            if (cost>max)
                {
                max = cost;
                }
        }
        decimal average = sum / n;
        int aboveAverage = 0;
        for (int i = 0; i < n; i++)
        {
            if(costs[i] > average) 
            {
                aboveAverage++;
            }
        }
        int firstExpensive = -1;
        int index = 0;
        while (index < n)
        {
            if (costs[index] > 1000)
            {
                firstExpensive = index;
                break;
            }
            index++;
        }
        Console.WriteLine("=== Звіт по прийомах ===");
        Console.WriteLine($"Кількість:        {n}");
        Console.WriteLine($"Загальна сума:    {sum:F2} грн");
        Console.WriteLine($"Середня:          {average:F2} грн");
        Console.WriteLine($"Мін / Макс:       {min:F2} / {max:F2} грн");
        Console.WriteLine($"Вище середнього:  {aboveAverage} з {n}");
        if (firstExpensive >= 0)
        {
            Console.WriteLine($"Перший > 1000:    #{firstExpensive + 1} — {costs[firstExpensive]:F2} грн");
        }
        else
        {
            Console.WriteLine("Перший > 1000:    немає");
        }
        Console.WriteLine("========================");
    }
}