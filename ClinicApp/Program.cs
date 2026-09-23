namespace ClinicApp;

public static class Program
{
    public static void Main()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture =
            System.Globalization.CultureInfo.InvariantCulture;

        Clinic clinic = new Clinic("Медична Клініка");

        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 15), "A+", "0501234567"));
        clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 20), "B-", "0672345678"));
        clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 3, 10), "O+", "0933456789"));
        clinic.Patients.Add(new Patient("Марія", "Ткач"));

        Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
        d1.WorkEndHour = 16;
        clinic.Doctors.Add(d1);

        Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
        d2.WorkStartHour = 9;
        d2.WorkEndHour = 18;
        clinic.Doctors.Add(d2);

        Doctor d3 = new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789");
        clinic.Doctors.Add(d3);

        Console.WriteLine();
        Console.WriteLine("=== Записи ===");

        clinic.Appointments.Book(1, 1, new DateTime(2026, 5, 9, 10, 0, 0));
        clinic.Appointments.Book(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        clinic.Appointments.Book(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        Console.WriteLine();
        clinic.Appointments.Book(99, 1, new DateTime(2026, 5, 9, 12, 0, 0));

        Console.WriteLine();
        clinic.DisplaySchedule(new DateTime(2026, 5, 9));

        Console.WriteLine();
        clinic.GenerateReport();

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Головне меню ===");
            Console.WriteLine("1. Пацієнти");
            Console.WriteLine("2. Лікарі");
            Console.WriteLine("3. Записи");
            Console.WriteLine("4. Розклад на дату");
            Console.WriteLine("5. Звіт");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "1")
            {
                PatientMenu(clinic);
            }
            else if (choice == "2")
            {
                DoctorMenu(clinic);
            }
            else if (choice == "3")
            {
                AppointmentMenu(clinic);
            }
            else if (choice == "4")
            {
                Console.Write("Рік: ");
                int year = int.Parse(Console.ReadLine()!);
                Console.Write("Місяць: ");
                int month = int.Parse(Console.ReadLine()!);
                Console.Write("День: ");
                int day = int.Parse(Console.ReadLine()!);

                clinic.DisplaySchedule(new DateTime(year, month, day));
            }
            else if (choice == "5")
            {
                clinic.GenerateReport();
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

    private static void PatientMenu(Clinic clinic)
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
                clinic.Patients.DisplayAll();
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
                clinic.Patients.Add(newPatient);
            }
            else if (choice == "3")
            {
                Console.Write("Пошук: ");
                string query = Console.ReadLine()!;
                Patient[] found = clinic.Patients.FindByName(query);

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
                bool removed = clinic.Patients.Remove(id);

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
                clinic.Patients.DisplayStats();
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

    private static void DoctorMenu(Clinic clinic)
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
                clinic.Doctors.DisplayAll();
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
                clinic.Doctors.Add(newDoctor);
            }
            else if (choice == "3")
            {
                Console.Write("Спеціальність: ");
                string query = Console.ReadLine()!;
                Doctor[] found = clinic.Doctors.FindBySpeciality(query);

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
                bool removed = clinic.Doctors.Remove(id);

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
                clinic.Doctors.DisplayStats();
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

    private static void AppointmentMenu(Clinic clinic)
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
                clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
            }
            else if (choice == "2")
            {
                Console.WriteLine("Доступні пацієнти:");
                clinic.Patients.DisplayAll();
                Console.WriteLine("Доступні лікарі:");
                clinic.Doctors.DisplayAll();

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

                clinic.Appointments.Book(patientId, doctorId, new DateTime(year, month, day, hour, minute, 0));
            }
            else if (choice == "3")
            {
                Console.Write("Id запису: ");
                int id = int.Parse(Console.ReadLine()!);
                Console.Write("Причина: ");
                string reason = Console.ReadLine()!;

                bool ok = clinic.Appointments.Cancel(id, reason);
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

                bool ok = clinic.Appointments.Complete(id);
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
                clinic.Appointments.DisplayList(clinic.Appointments.GetByPatient(patientId));
            }
            else if (choice == "6")
            {
                Console.Write("Id лікаря: ");
                int doctorId = int.Parse(Console.ReadLine()!);
                clinic.Appointments.DisplayList(clinic.Appointments.GetByDoctor(doctorId));
            }
            else if (choice == "7")
            {
                Console.Write("Рік: ");
                int year = int.Parse(Console.ReadLine()!);
                Console.Write("Місяць: ");
                int month = int.Parse(Console.ReadLine()!);
                Console.Write("День: ");
                int day = int.Parse(Console.ReadLine()!);

                clinic.Appointments.DisplayList(clinic.Appointments.GetByDate(new DateTime(year, month, day)));
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