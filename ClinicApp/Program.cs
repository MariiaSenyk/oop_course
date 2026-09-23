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

        AppointmentManager appointmentManager = new AppointmentManager(patientManager, doctorManager);

        Console.WriteLine();
        Console.WriteLine("=== Записи ===");

        appointmentManager.Book(1, 1, new DateTime(2026, 5, 9, 10, 0, 0));
        appointmentManager.Book(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        appointmentManager.Book(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        Console.WriteLine();
        appointmentManager.Book(99, 1, new DateTime(2026, 5, 9, 12, 0, 0));

        Console.WriteLine();
        Console.WriteLine("Майбутні записи:");
        appointmentManager.DisplayList(appointmentManager.GetUpcoming());

        Console.WriteLine();
        appointmentManager.Cancel(1, "Пацієнт не зміг прийти");

        Console.WriteLine();
        Console.WriteLine("Записи пацієнта #2:");
        appointmentManager.DisplayList(appointmentManager.GetByPatient(2));

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Головне меню ===");
            Console.WriteLine("1. Пацієнти");
            Console.WriteLine("2. Лікарі");
            Console.WriteLine("3. Записи");
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
            else if (choice == "3")
            {
                AppointmentMenu(appointmentManager, patientManager, doctorManager);
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

    private static void AppointmentMenu(AppointmentManager manager, PatientManager patients, DoctorManager doctors)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Меню: Записи ===");
            Console.WriteLine("1. Показати всі майбутні");
            Console.WriteLine("2. Створити запис");
            Console.WriteLine("3. Скасувати запис");
            Console.WriteLine("4. Завершити запис");
            Console.WriteLine("5. Записи пацієнта");
            Console.WriteLine("6. Записи лікаря");
            Console.WriteLine("7. Записи на дату");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "1")
            {
                manager.DisplayList(manager.GetUpcoming());
            }
            else if (choice == "2")
            {
                Console.WriteLine("Доступні пацієнти:");
                patients.DisplayAll();
                Console.WriteLine("Доступні лікарі:");
                doctors.DisplayAll();

                Console.Write("Id пацієнта: ");
                int patientId = int.Parse(Console.ReadLine()!);
                Console.Write("Id лікаря: ");
                int doctorId = int.Parse(Console.ReadLine()!);
                Console.Write("Рік: ");
                int year = int.Parse(Console.ReadLine()!);
                Console.Write("Місяць: ");
                int month = int.Parse(Console.ReadLine()!);
                Console.Write("День: ");
                int day = int.Parse(Console.ReadLine()!);
                Console.Write("Година: ");
                int hour = int.Parse(Console.ReadLine()!);
                Console.Write("Хвилини: ");
                int minute = int.Parse(Console.ReadLine()!);

                manager.Book(patientId, doctorId, new DateTime(year, month, day, hour, minute, 0));
            }
            else if (choice == "3")
            {
                Console.Write("Id запису: ");
                int id = int.Parse(Console.ReadLine()!);
                Console.Write("Причина: ");
                string reason = Console.ReadLine()!;

                bool ok = manager.Cancel(id, reason);
                if (ok)
                {
                    Console.WriteLine($"Запис [{id}] скасовано.");
                }
                else
                {
                    Console.WriteLine($"Не вдалося скасувати запис [{id}].");
                }
            }
            else if (choice == "4")
            {
                Console.Write("Id запису: ");
                int id = int.Parse(Console.ReadLine()!);

                bool ok = manager.Complete(id);
                if (ok)
                {
                    Console.WriteLine($"Запис [{id}] завершено.");
                }
                else
                {
                    Console.WriteLine($"Не вдалося завершити запис [{id}].");
                }
            }
            else if (choice == "5")
            {
                Console.Write("Id пацієнта: ");
                int patientId = int.Parse(Console.ReadLine()!);
                manager.DisplayList(manager.GetByPatient(patientId));
            }
            else if (choice == "6")
            {
                Console.Write("Id лікаря: ");
                int doctorId = int.Parse(Console.ReadLine()!);
                manager.DisplayList(manager.GetByDoctor(doctorId));
            }
            else if (choice == "7")
            {
                Console.Write("Рік: ");
                int year = int.Parse(Console.ReadLine()!);
                Console.Write("Місяць: ");
                int month = int.Parse(Console.ReadLine()!);
                Console.Write("День: ");
                int day = int.Parse(Console.ReadLine()!);

                manager.DisplayList(manager.GetByDate(new DateTime(year, month, day)));
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