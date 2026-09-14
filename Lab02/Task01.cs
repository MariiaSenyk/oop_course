namespace Lab02;

public static class Task01
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);
        double [] weights = new double[n];
        double sum = 0;
        int aboveAverage = 0;
        double average = 0;
        for (int i=0;i<n;i++)
        {
            weights[i] = double.Parse(Console.ReadLine()!);
        }
        double min=weights[0];
        double max=weights[0];
        foreach (double weight in weights)
        {
            sum+=weight;
            if (weight<min)
                min = weight;
            if (weight>max)
                max = weight;
        }
        average = sum / n;
        foreach (double weight in weights )
        {
            if (weight > average)
                aboveAverage++;
        }
        Console.WriteLine($"Кількість: {n}");
        Console.WriteLine($"Середня вага: {average:F1}");
        Console.WriteLine($"Мін/Макс: {min:F1} / {max:F1}");
        Console.WriteLine($"Вище середнього: {aboveAverage} з {n}");
    }
}