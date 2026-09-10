namespace  Lab01;

public static class Program
{
    public static void Main() {
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        
        Task02.Run();
    }

}

