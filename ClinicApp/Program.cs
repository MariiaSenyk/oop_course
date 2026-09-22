namespace ClinicApp;

public static class Program
{
    public static void Main()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture =
            System.Globalization.CultureInfo.InvariantCulture;

        Patient p1 = new Patient("Іван", "Петренко", new DateTime(1985, 5, 15), "A+", "0501234567");
        Patient p2 = new Patient("Олена", "Коваль", new DateTime(1993, 8, 20), "B-", "0672345678");
        Patient p3 = new Patient("Максим", "Бойко", new DateTime(2010, 3, 10), "O+", "0933456789");
        Patient p4 = new Patient();
        Patient p5 = new Patient("Марія", "Ткач");

        Console.WriteLine(p1);
        Console.WriteLine(p2);
        Console.WriteLine(p3);
        Console.WriteLine(p4);
        Console.WriteLine(p5);
    }
}