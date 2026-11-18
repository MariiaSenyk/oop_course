namespace ClinicApp.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

public class SessionManager
{
    private readonly string _sessionPath;

    public SessionManager(string sessionPath = "session.dat")
    {
        _sessionPath = sessionPath;
    }

    public bool Exists()
    {
        return File.Exists(_sessionPath);
    }

    public void Save(Clinic clinic)
    {
        Patient[] patients = clinic.Patients.GetAll();

        using (StreamWriter writer = new StreamWriter(_sessionPath, false, Encoding.UTF8))
        {
            writer.WriteLine("[PATIENTS]");
            for (int i = 0; i < patients.Length; i++)
            {
                Patient p = patients[i];
                writer.WriteLine($"{p.FirstName},{p.LastName},{p.DateOfBirth:dd.MM.yyyy},{p.BloodType},{p.Phone}");
            }
            writer.WriteLine("[END]");
        }
    }

    public int Load(Clinic clinic)
    {
        if (!File.Exists(_sessionPath))
        {
            return 0;
        }

        string[] lines = File.ReadAllLines(_sessionPath, Encoding.UTF8);
        string currentSection = "";
        int loaded = 0;

        foreach (string rawLine in lines)
        {
            string line = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.StartsWith("["))
            {
                currentSection = line;
                continue;
            }

            if (currentSection != "[PATIENTS]")
            {
                continue;
            }

            try
            {
                string[] parts = line.Split(',');
                if (parts.Length != 5)
                {
                    continue;
                }

                string firstName = parts[0].Trim();
                string lastName = parts[1].Trim();
                string dateStr = parts[2].Trim();
                string bloodStr = parts[3].Trim();
                string phone = parts[4].Trim();

                DateTime dateOfBirth = DateTime.ParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), bloodStr, true);

                if (PatientExists(clinic, firstName, lastName, dateOfBirth))
                {
                    continue;
                }

                Patient patient = new Patient(firstName, lastName, dateOfBirth, bloodType, phone);
                clinic.Patients.Add(patient);
                loaded++;
            }
            catch
            {
                // Пошкоджений рядок - пропускаємо
            }
        }

        return loaded;
    }

    private bool PatientExists(Clinic clinic, string firstName, string lastName, DateTime dateOfBirth)
    {
        Patient[] all = clinic.Patients.GetAll();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].FirstName == firstName &&
                all[i].LastName == lastName &&
                all[i].DateOfBirth.Date == dateOfBirth.Date)
            {
                return true;
            }
        }
        return false;
    }
}