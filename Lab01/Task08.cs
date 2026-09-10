namespace Lab01;

public static class Task08
{
    public static void Run()
    {
        double weight=double.Parse(Console.ReadLine()!);
        double height=double.Parse(Console.ReadLine()!);
        double price=double.Parse(Console.ReadLine()!);
        int quantity=int.Parse(Console.ReadLine()!);
        int discount=int.Parse(Console.ReadLine()!);
        int birthYear=int.Parse(Console.ReadLine()!);
        int systolic=int.Parse(Console.ReadLine()!);
        int diastolic=int.Parse(Console.ReadLine()!);
        
        double bmi=CalculateBMI(weight,height);
        string bmiCategory = GetBMICategory(bmi);
        Console.WriteLine($"ІМТ: {bmi:F2} -> {bmiCategory}");
        
        double cost = CalculateCost(price, quantity, discount);
        Console.WriteLine($"Сума: {cost:F2} грн");
        
        int age=2026-birthYear;
        string ageCategory = GetAgeCategory(age);
        
        Console.WriteLine($"Вік: {age} р., категорія: {ageCategory}");

        string pressureStatus =GetPressureStatus(systolic, diastolic);
        Console.WriteLine($"Тиск: {systolic}/{diastolic} - {pressureStatus}");
        
    }

    public static double CalculateBMI(double weight, double height)
    {
        return weight/(height * height);
    }

    public static string GetAgeCategory(int age)
    {
        if (age <= 17)
        {
            return "дитина";
        }

        if (age <= 59)
        {
            return "дорослий";
        }

        return "пенсіонер";
    }

    public static string GetBMICategory(double bmi)
    {
        if (bmi < 18.5)
        {
            return "недостатня вага";
        }
        if (bmi < 25)
        {
            return "норма";
        }
        if (bmi < 30)
        {
            return "надмірна вага";
        }
        return "ожиріння";
    }

    public static double CalculateCost(double price, int quantity, int discount)
    {
        return price * quantity * (1-discount/100.0);
    }

    public static string GetPressureStatus(int systolic, int diastolic)
    {
        if (systolic < 120 && diastolic < 80)
        {
            return "норма";
        }
        if (systolic < 130 && diastolic < 80)
        {
            return "підвищений";
        }
        if (systolic < 140 || diastolic < 90)
        {
            return "гіпертонія 1 ступеня";
        }
        return "гіпертонія 2 ступеня";
    }
}