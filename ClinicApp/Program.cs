namespace ClinicApp;

using System.Globalization;
using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

public static class Program
{
    public static void Main()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Clinic clinic = new Clinic("Медична Клініка");

        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 15), BloodType.APositive, "0501234567"));
        clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 20), BloodType.BNegative, "0672345678"));
        clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 3, 10), BloodType.OPositive, "0933456789"));
        clinic.Patients.Add(new Patient("Марія", "Ткач"));

        Doctor d1 = new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567");
        d1.Schedule = new WorkSchedule(8, 16);
        clinic.Doctors.Add(d1);

        Doctor d2 = new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678");
        d2.Schedule = new WorkSchedule(9, 18);
        clinic.Doctors.Add(d2);

        Doctor d3 = new Doctor("Андрій", "Власенко", Speciality.Pediatrics, "LIC-003", "0443456789");
        clinic.Doctors.Add(d3);

        clinic.Appointments.Book(1, 1, new DateTime(2027, 5, 9, 10, 0, 0));
        clinic.Appointments.BookUrgent(2, 2, new DateTime(2027, 5, 9, 11, 0, 0), "біль у грудях");
        clinic.Appointments.BookSpecialist(3, 3, new DateTime(2027, 5, 10, 9, 0, 0), "кардіологія");

        clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-30), "I10", "Гіпертонічна хвороба", isChronic: true));
        clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-5), "J06.9", "Гострий ринофарингіт"));
        clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today.AddDays(-10), "Гемоглобін", 145, "г/л", "120–160", isNormal: true));
        clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today.AddDays(-10), "Холестерин", 6.2, "ммоль/л", "< 5.2", isNormal: false));
        clinic.MedicalRecords.Add(new Prescription(1, 1, DateTime.Today.AddDays(-5), "Лізиноприл", "10 мг", 30, "1 раз на добу вранці"));
        clinic.MedicalRecords.Add(new Prescription(2, 2, DateTime.Today.AddDays(-40), "Аспірин", "500 мг", 10, "після їжі"));
        clinic.MedicalRecords.Add(new Diagnosis(3, 3, DateTime.Today.AddDays(-3), "J45", "Бронхіальна астма"));
        clinic.MedicalRecords.Add(new LabResult(2, 2, DateTime.Today.AddDays(-2), "Цукор", 5.4, "ммоль/л", "4.0–6.0", isNormal: true));

        Console.WriteLine();
        Console.WriteLine("== new vs override ==");

        UrgentAppointment urgent = new UrgentAppointment(1, 1, DateTime.Today.AddDays(1).AddHours(15), "тест");
        Appointment asBase = urgent;

        Console.WriteLine($"Через Appointment:       {asBase.GetDescription()} | пріоритет {asBase.GetPriority()}");
        Console.WriteLine($"Через UrgentAppointment: {urgent.GetDescription()} | пріоритет {urgent.GetPriority()}");

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║           МЕДИЧНА КЛІНІКА                    ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Пацієнти       — реєстрація, пошук       ║");
            Console.WriteLine("║  2. Лікарі         — персонал, розклад       ║");
            Console.WriteLine("║  3. Записи         — прийоми, скасування     ║");
            Console.WriteLine("║  4. Медична картка — діагнози, рецепти       ║");
            Console.WriteLine("║  5. Рахунки        — оплата, борги           ║");
            Console.WriteLine("║  6. Звіт           — загальна статистика     ║");
            Console.WriteLine("║  0. Вийти                                    ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.Write("Оберіть розділ: ");

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
                MedicalRecordsMenu(clinic);
            }
            else if (choice == "5")
            {
                BillingMenu(clinic);
            }
            else if (choice == "6")
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
                try
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
                    Console.Write("Група крові (0-Unknown, 1-APositive, 2-ANegative, 3-BPositive, 4-BNegative, 5-ABPositive, 6-ABNegative, 7-OPositive, 8-ONegative): ");
                    int bloodChoice = int.Parse(Console.ReadLine()!);
                    BloodType bloodType = (BloodType)bloodChoice;
                    Console.Write("Телефон: ");
                    string phone = Console.ReadLine()!;

                    Patient newPatient = new Patient(firstName, lastName, new DateTime(year, month, day), bloodType, phone);
                    clinic.Patients.Add(newPatient);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
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
                if (!TryReadInt("Id пацієнта для видалення: ", out int id)) continue;
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
            Console.WriteLine("5. Вільні години лікаря");
            Console.WriteLine("6. Статистика");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "1")
            {
                clinic.Doctors.DisplayAll();
            }
            else if (choice == "2")
            {
                try
                {
                    Console.Write("Ім'я: ");
                    string firstName = Console.ReadLine()!;
                    Console.Write("Прізвище: ");
                    string lastName = Console.ReadLine()!;
                    Console.Write("Спеціальність (General, Cardiology, Neurology, Pediatrics, Surgery, Orthopedics, Dermatology, Emergency): ");
                    string specialityText = Console.ReadLine()!;
                    Speciality speciality = (Speciality)Enum.Parse(typeof(Speciality), specialityText, true);
                    Console.Write("Номер ліцензії: ");
                    string license = Console.ReadLine()!;
                    Console.Write("Телефон: ");
                    string phone = Console.ReadLine()!;
                    Console.Write("Година початку роботи: ");
                    int start = int.Parse(Console.ReadLine()!);
                    Console.Write("Година кінця роботи: ");
                    int end = int.Parse(Console.ReadLine()!);

                    Doctor newDoctor = new Doctor(firstName, lastName, speciality, license, phone);
                    newDoctor.Schedule = new WorkSchedule(start, end);
                    clinic.Doctors.Add(newDoctor);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
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
                if (!TryReadInt("Id лікаря для видалення: ", out int id)) continue;
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
                if (!TryReadInt("ID лікаря: ", out int doctorId)) continue;

                Doctor? doctor = clinic.Doctors.FindById(doctorId);
                if (doctor == null)
                {
                    Console.WriteLine("Лікаря не знайдено.");
                    continue;
                }

                ISchedulable schedulable = doctor;

                Console.Write("Дата (dd.MM.yyyy): ");
                string dateText = Console.ReadLine()!;
                if (!DateTime.TryParseExact(dateText, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    Console.WriteLine("Некоректна дата.");
                    continue;
                }

                if (!TryReadInt("Скільки слотів: ", out int slotCount)) continue;

                try
                {
                    DateTime[] slots = schedulable.GetAvailableSlots(date, slotCount);
                    for (int i = 0; i < slots.Length; i++)
                    {
                        Console.WriteLine(slots[i].ToString("HH:mm"));
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
            }
            else if (choice == "6")
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
            Console.WriteLine("8. Скасувати всі записи пацієнта");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "1")
            {
                clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
            }
            else if (choice == "2")
            {
                try
                {
                    Console.WriteLine("Доступні пацієнти:");
                    clinic.Patients.DisplayAll();
                    Console.WriteLine("Доступні лікарі:");
                    clinic.Doctors.DisplayAll();

                    if (!TryReadInt("Id пацієнта: ", out int patientId)) continue;
                    if (!TryReadInt("Id лікаря: ", out int doctorId)) continue;
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
                    Console.Write("Тривалість (хвилин): ");
                    int duration = int.Parse(Console.ReadLine()!);

                    clinic.Appointments.Book(patientId, doctorId, new DateTime(year, month, day, hour, minute, 0), duration);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
            }
            else if (choice == "3")
            {
                if (!TryReadInt("Id запису: ", out int id)) continue;
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
                if (!TryReadInt("Id запису: ", out int id)) continue;

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
                if (!TryReadInt("Id пацієнта: ", out int patientId)) continue;
                clinic.Appointments.DisplayList(clinic.Appointments.GetByPatient(patientId));
            }
            else if (choice == "6")
            {
                if (!TryReadInt("Id лікаря: ", out int doctorId)) continue;
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

                clinic.Appointments.DisplayList(clinic.Appointments.GetByDate(year, month, day));
            }
            else if (choice == "8")
            {
                if (!TryReadInt("ID пацієнта: ", out int patientId)) continue;
                Console.Write("Причина (Enter — без причини): ");
                string reason = Console.ReadLine()!;

                Appointment[] items = clinic.Appointments.GetByPatient(patientId);
                int cancelled = AppointmentManager.CancelAll(items, reason);
                Console.WriteLine($"Скасовано записів: {cancelled}");
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

    private static void MedicalRecordsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Меню: Медична картка ===");
            Console.WriteLine("1. Картка пацієнта (зведення)");
            Console.WriteLine("2. Усі записи пацієнта");
            Console.WriteLine("3. Додати діагноз");
            Console.WriteLine("4. Додати аналіз");
            Console.WriteLine("5. Додати рецепт");
            Console.WriteLine("6. Записи лікаря");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "1")
            {
                if (!TryReadInt("ID пацієнта: ", out int patientId)) continue;
                clinic.MedicalRecords.DisplayPatientSummary(patientId);
            }
            else if (choice == "2")
            {
                if (!TryReadInt("ID пацієнта: ", out int patientId)) continue;
                MedicalRecord[] records = clinic.MedicalRecords.GetByPatient(patientId);
                if (records.Length == 0)
                {
                    Console.WriteLine("Записів не знайдено.");
                }
                else
                {
                    for (int i = 0; i < records.Length; i++)
                    {
                        Console.WriteLine($"{records[i].GetRecordType()}: {records[i].GetSummary()}");
                    }
                }
            }
            else if (choice == "3")
            {
                try
                {
                    Console.WriteLine("Доступні пацієнти:");
                    clinic.Patients.DisplayAll();
                    Console.WriteLine("Доступні лікарі:");
                    clinic.Doctors.DisplayAll();

                    if (!TryReadInt("ID пацієнта: ", out int patientId)) continue;
                    if (!TryReadInt("ID лікаря: ", out int doctorId)) continue;

                    if (clinic.Patients.FindById(patientId) == null)
                    {
                        Console.WriteLine($"Пацієнта з ID {patientId} не знайдено.");
                        continue;
                    }
                    if (clinic.Doctors.FindById(doctorId) == null)
                    {
                        Console.WriteLine($"Лікаря з ID {doctorId} не знайдено.");
                        continue;
                    }

                    Console.Write("Код діагнозу (напр. J06.9): ");
                    string code = Console.ReadLine()!;
                    Console.Write("Опис: ");
                    string description = Console.ReadLine()!;
                    Console.Write("Хронічне? (1=так, 0=ні): ");
                    int chronicChoice = int.Parse(Console.ReadLine()!);
                    bool isChronic = chronicChoice == 1;

                    Diagnosis newDiagnosis = new Diagnosis(patientId, doctorId, DateTime.Today, code, description, isChronic);
                    clinic.MedicalRecords.Add(newDiagnosis);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
            }
            else if (choice == "4")
            {
                try
                {
                    Console.WriteLine("Доступні пацієнти:");
                    clinic.Patients.DisplayAll();
                    Console.WriteLine("Доступні лікарі:");
                    clinic.Doctors.DisplayAll();

                    if (!TryReadInt("ID пацієнта: ", out int patientId)) continue;
                    if (!TryReadInt("ID лікаря: ", out int doctorId)) continue;

                    if (clinic.Patients.FindById(patientId) == null)
                    {
                        Console.WriteLine($"Пацієнта з ID {patientId} не знайдено.");
                        continue;
                    }
                    if (clinic.Doctors.FindById(doctorId) == null)
                    {
                        Console.WriteLine($"Лікаря з ID {doctorId} не знайдено.");
                        continue;
                    }

                    Console.Write("Назва аналізу: ");
                    string testName = Console.ReadLine()!;
                    Console.Write("Значення (число): ");
                    string valueText = Console.ReadLine()!;
                    if (!double.TryParse(valueText, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                    {
                        Console.WriteLine("Некоректне число.");
                        continue;
                    }
                    Console.Write("Одиниці виміру: ");
                    string unit = Console.ReadLine()!;
                    Console.Write("Норма (напр. 4.0–9.0): ");
                    string referenceRange = Console.ReadLine()!;
                    Console.Write("В нормі? (1=так, 0=ні): ");
                    int normalChoice = int.Parse(Console.ReadLine()!);
                    bool isNormal = normalChoice == 1;

                    LabResult newLabResult = new LabResult(patientId, doctorId, DateTime.Today, testName, value, unit, referenceRange, isNormal);
                    clinic.MedicalRecords.Add(newLabResult);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
            }
            else if (choice == "5")
            {
                try
                {
                    Console.WriteLine("Доступні пацієнти:");
                    clinic.Patients.DisplayAll();
                    Console.WriteLine("Доступні лікарі:");
                    clinic.Doctors.DisplayAll();

                    if (!TryReadInt("ID пацієнта: ", out int patientId)) continue;
                    if (!TryReadInt("ID лікаря: ", out int doctorId)) continue;

                    if (clinic.Patients.FindById(patientId) == null)
                    {
                        Console.WriteLine($"Пацієнта з ID {patientId} не знайдено.");
                        continue;
                    }
                    if (clinic.Doctors.FindById(doctorId) == null)
                    {
                        Console.WriteLine($"Лікаря з ID {doctorId} не знайдено.");
                        continue;
                    }

                    Console.Write("Препарат: ");
                    string medication = Console.ReadLine()!;
                    Console.Write("Дозування (10 мг): ");
                    string dosage = Console.ReadLine()!;
                    Console.Write("Кількість днів: ");
                    string daysText = Console.ReadLine()!;
                    if (!int.TryParse(daysText, out int days))
                    {
                        Console.WriteLine("Некоректне число.");
                        continue;
                    }
                    Console.Write("Інструкція (Enter — пропустити): ");
                    string instructions = Console.ReadLine()!;

                    Prescription newPrescription = new Prescription(patientId, doctorId, DateTime.Today, medication, dosage, days, instructions);
                    clinic.MedicalRecords.Add(newPrescription);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
            }
            else if (choice == "6")
            {
                if (!TryReadInt("ID лікаря: ", out int doctorId))
                {
                    continue;
                }
                MedicalRecord[] records = clinic.MedicalRecords.GetByDoctor(doctorId);
                clinic.MedicalRecords.DisplayList(records);
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

    private static void BillingMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("── Рахунки ───────────────────");
            Console.WriteLine("  1. Борги пацієнта");
            Console.WriteLine("  2. Всі неоплачені записи");
            Console.WriteLine("  3. Оплатити запис");
            Console.WriteLine("  4. Загальний борг клініки");
            Console.WriteLine("  0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine()!;

            if (choice == "1")
            {
                if (!TryReadInt("ID пацієнта: ", out int patientId))
                {
                    continue;
                }

                IPayable[] unpaid = clinic.Billing.GetUnpaidByPatient(patientId);
                Console.WriteLine($"Неоплачені записи пацієнта #{patientId}:");
                clinic.Billing.DisplayUnpaid(unpaid);
                Console.WriteLine($"Борг: {clinic.Billing.GetPatientDebt(patientId):F2} грн");
            }
            else if (choice == "2")
            {
                IPayable[] unpaid = clinic.Billing.GetAllUnpaid();
                clinic.Billing.DisplayUnpaid(unpaid);
            }
            else if (choice == "3")
            {
                if (!TryReadInt("ID запису для оплати: ", out int id))
                {
                    continue;
                }

                bool ok = clinic.Billing.PayAppointment(id);
                if (ok)
                {
                    Console.WriteLine($"Запис [{id}] оплачено.");
                }
                else
                {
                    Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                }
            }
            else if (choice == "4")
            {
                Console.WriteLine($"Загальний борг по клініці: {clinic.Billing.GetTotalDebt():F2} грн");
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

    private static bool TryReadInt(string prompt, out int value)
    {
        Console.Write(prompt);
        string input = Console.ReadLine()!;
        if (!int.TryParse(input, out value))
        {
            Console.WriteLine("Некоректне число.");
            return false;
        }
        return true;
    }
}