namespace ClinicApp.Utils;

using System;
using System.IO;
using System.Globalization;
using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Managers;

public static class CsvImporter
{
    public static ImportResult ImportPatients(Clinic clinic, string filePath)
    {
        ImportResult result = new ImportResult();

        if (!File.Exists(filePath))
        {
            result.AddError(0, "Файл не знайдено.");
            return result;
        }

        string[] lines = File.ReadAllLines(filePath);

        // Пропускаємо заголовок (індекс 0)
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                string[] parts = line.Split(',');
                if (parts.Length != 5)
                {
                    throw new Exception($"Рядок має {parts.Length} поле(ів) замість 5.");
                }

                string firstName = parts[0].Trim();
                string lastName = parts[1].Trim();
                string dateStr = parts[2].Trim();
                string bloodStr = parts[3].Trim();
                string phone = parts[4].Trim();

                DateTime dateOfBirth = DateTime.ParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), bloodStr, true);

                Patient patient = new Patient(firstName, lastName, dateOfBirth, bloodType, phone);
                clinic.Patients.Add(patient);
                result.AddSuccess();
            }
            catch (Exception ex)
            {
                result.AddError(i + 1, ex.Message);
            }
        }

        return result;
    }
}