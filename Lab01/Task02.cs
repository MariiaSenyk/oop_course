namespace Lab01;

public static class Task02
{
    public static void Run()
    {
        double price = double.Parse(Console.ReadLine()!);
        int quantity = int.Parse(Console.ReadLine()!);
        int discount = int.Parse(Console.ReadLine()!);
        
        double totalPrice = price * quantity * (1 - discount/100.0);
        
        Console.WriteLine($"Сума: {totalPrice:F2} грн");
    }
}