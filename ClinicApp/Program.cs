namespace ClinicApp;

public static class Program
{
    public static void Main()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture =
            System.Globalization.CultureInfo.InvariantCulture;

        PatientManager manager = new PatientManager();

        manager.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 15), "A+", "0501234567"));
        manager.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 20), "B-", "0672345678"));
        manager.Add(new Patient("Максим", "Бойко", new DateTime(2010, 3, 10), "O+", "0933456789"));
        manager.Add(new Patient("Марія", "Ткач"));

        PatientMenu(manager);
    }

    private static void PatientMenu(PatientManager manager)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Меню: Пацієнти ===");
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Додати пацієнта");
            Console.WriteLine("3. Знайти за ім'ям");
            Console.WriteLine("4. Видалити за Id");
            Console.WriteLine("5. Статистика");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "1")
            {
                manager.DisplayAll();
            }
            else if (choice == "2")
            {
                Console.Write("Ім'я: ");
                string firstName = Console.ReadLine()!;
                Console.Write("Прізвище: ");
                string lastName = Console.ReadLine()!;
                Console.Write("Рік народження: ");
                int year = int.Parse(Console.ReadLine()!);
                Console.Write("Місяць народження: ");
                int month = int.Parse(Console.ReadLine()!);
                Console.Write("День народження: ");
                int day = int.Parse(Console.ReadLine()!);
                Console.Write("Група крові: ");
                string bloodType = Console.ReadLine()!;
                Console.Write("Телефон: ");
                string phone = Console.ReadLine()!;

                Patient newPatient = new Patient(firstName, lastName, new DateTime(year, month, day), bloodType, phone);
                manager.Add(newPatient);
            }
            else if (choice == "3")
            {
                Console.Write("Пошук: ");
                string query = Console.ReadLine()!;
                Patient[] found = manager.FindByName(query);

                if (found.Length == 0)
                {
                    Console.WriteLine("Нічого не знайдено.");
                }
                else
                {
                    Console.WriteLine($"Знайдено: {found.Length}");
                    for (int i = 0; i < found.Length; i++)
                    {
                        Console.WriteLine(found[i]);
                    }
                }
            }
            else if (choice == "4")
            {
                Console.Write("Id пацієнта для видалення: ");
                int id = int.Parse(Console.ReadLine()!);
                bool removed = manager.Remove(id);

                if (removed)
                {
                    Console.WriteLine($"Пацієнта [{id}] видалено.");
                }
                else
                {
                    Console.WriteLine($"Пацієнта з Id={id} не знайдено.");
                }
            }
            else if (choice == "5")
            {
                manager.DisplayStats();
            }
            else if (choice == "0")
            {
                Console.WriteLine("Вихід з меню пацієнтів.");
                break;
            }
            else
            {
                Console.WriteLine("Невідома команда.");
            }
        }
    }
}