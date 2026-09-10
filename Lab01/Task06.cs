namespace Lab01;

public static class Task06
{
    public static void Run()
    {
        int number= int.Parse(Console.ReadLine()!);
        int lastDigit = number % 10;
        string department = lastDigit switch
        {
            0 or 1 => "загальна терапія",
            2 or 3 => "хірургія",
            4 or 5 => "кардіологія",
            6 or 7 => "неврологія",
            8 or 9 => "офтальмологія",
            _ => "невідомо",
        };
        string pension = number % 2 == 0 ? "так" : "ні";
        string checkup = number % 3 == 0 ? "так" : "ні";
        
        Console.WriteLine($"Відділення: {department}");
        Console.WriteLine($"Пільгова: {pension}");
        Console.WriteLine($"Огляд: {checkup}");
    }
}