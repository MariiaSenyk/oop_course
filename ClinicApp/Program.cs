namespace ClinicApp;

public static class Program
{
    public static void Main()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture =
            System.Globalization.CultureInfo.InvariantCulture;

        PatientManager patientManager = new PatientManager();
        patientManager.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 15), "A+", "0501234567"));
        patientManager.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 20), "B-", "0672345678"));
        patientManager.Add(new Patient("Максим", "Бойко", new DateTime(2010, 3, 10), "O+", "0933456789"));
        patientManager.Add(new Patient("Марія", "Ткач"));

        DoctorManager doctorManager = new DoctorManager();

        Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
        d1.WorkEndHour = 16;
        doctorManager.Add(d1);

        Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
        d2.WorkStartHour = 9;
        d2.WorkEndHour = 18;
        doctorManager.Add(d2);

        Doctor d3 = new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789");
        doctorManager.Add(d3);
        Console.WriteLine();
        Console.WriteLine("=== Прийоми ===");

        Appointment a1 = new Appointment(1, 1, new DateTime(2026, 5, 9, 10, 0, 0));
        Appointment a2 = new Appointment(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        Appointment a3 = new Appointment(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        a1.Cancel("Пацієнт не зміг прийти");
        a2.Complete();

        Console.WriteLine(a1);
        Console.WriteLine(a2);
        Console.WriteLine(a3);
        
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Головне меню ===");
            Console.WriteLine("1. Пацієнти");
            Console.WriteLine("2. Лікарі");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "1")
            {
                PatientMenu(patientManager);
            }
            else if (choice == "2")
            {
                DoctorMenu(doctorManager);
            }
            else if (choice == "0")
            {
                Console.WriteLine("До побачення.");
                break;
            }
            else
            {
                Console.WriteLine("Невідома команда.");
            }
        }
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
            Console.WriteLine("0. Назад");
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
                break;
            }
            else
            {
                Console.WriteLine("Невідома команда.");
            }
        }
    }

    private static void DoctorMenu(DoctorManager manager)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Меню: Лікарі ===");
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Додати лікаря");
            Console.WriteLine("3. Знайти за спеціальністю");
            Console.WriteLine("4. Видалити за Id");
            Console.WriteLine("5. Статистика");
            Console.WriteLine("0. Назад");
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
                Console.Write("Спеціальність: ");
                string speciality = Console.ReadLine()!;
                Console.Write("Номер ліцензії: ");
                string license = Console.ReadLine()!;
                Console.Write("Телефон: ");
                string phone = Console.ReadLine()!;

                Doctor newDoctor = new Doctor(firstName, lastName, speciality, license, phone);
                manager.Add(newDoctor);
            }
            else if (choice == "3")
            {
                Console.Write("Спеціальність: ");
                string query = Console.ReadLine()!;
                Doctor[] found = manager.FindBySpeciality(query);

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
                Console.Write("Id лікаря для видалення: ");
                int id = int.Parse(Console.ReadLine()!);
                bool removed = manager.Remove(id);

                if (removed)
                {
                    Console.WriteLine($"Лікаря [{id}] видалено.");
                }
                else
                {
                    Console.WriteLine($"Лікаря з Id={id} не знайдено.");
                }
            }
            else if (choice == "5")
            {
                manager.DisplayStats();
            }
            else if (choice == "0")
            {
                break;
            }
            else
            {
                Console.WriteLine("Невідома команда.");
            }
        }
    }
}