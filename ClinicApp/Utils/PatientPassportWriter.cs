namespace ClinicApp.Utils;

using System;
using System.IO;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Events;
using ClinicApp.Models;

public class PatientPassportWriter
{
    private readonly Clinic _clinic;
    private readonly string _baseDir;

    public PatientPassportWriter(Clinic clinic, string baseDir = "patients")
    {
        _clinic = clinic;
        _baseDir = baseDir;
        Directory.CreateDirectory(_baseDir);
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e)
    {
        WritePassport(e.PatientId);
    }

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e)
    {
        WritePassport(e.PatientId);
    }

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e)
    {
        WritePassport(e.PatientId);
    }

    private void WritePassport(int patientId)
    {
        Patient? patient = _clinic.Patients.FindById(patientId);
        if (patient == null)
        {
            return;
        }

        string filePath = Path.Combine(_baseDir, $"passport_{patientId}.txt");

        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            writer.WriteLine($"=== Паспорт пацієнта #{patient.Id} ===");
            writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
            writer.WriteLine();
            
            writer.WriteLine($"Ім'я: {patient.FullName}");
            writer.WriteLine($"Дата народження: {patient.DateOfBirth:dd.MM.yyyy} ({ClinicFormatter.FormatAge(patient.Age)})");
            writer.WriteLine($"Група крові: {ClinicFormatter.FormatBloodType(patient.BloodType)}   Телефон: {ClinicFormatter.FormatPhone(patient.Phone)}");
            writer.WriteLine();
            
            MedicalRecord[] records = _clinic.MedicalRecords.GetByPatient(patientId);

            writer.WriteLine("-- Діагнози --");
            bool hasDiagnoses = false;
            for (int i = 0; i < records.Length; i++)
            {
                if (records[i] is Diagnosis d)
                {
                    writer.WriteLine(d.GetSummary());
                    hasDiagnoses = true;
                }
            }
            if (!hasDiagnoses) writer.WriteLine("Немає діагнозів.");

            writer.WriteLine("-- Аналізи --");
            bool hasLabs = false;
            for (int i = 0; i < records.Length; i++)
            {
                if (records[i] is LabResult lr)
                {
                    writer.WriteLine(lr.GetSummary());
                    hasLabs = true;
                }
            }
            if (!hasLabs) writer.WriteLine("Немає аналізів.");

            writer.WriteLine("-- Рецепти --");
            bool hasPrescriptions = false;
            for (int i = 0; i < records.Length; i++)
            {
                if (records[i] is Prescription p)
                {
                    writer.WriteLine(p.GetSummary());
                    hasPrescriptions = true;
                }
            }
            if (!hasPrescriptions) writer.WriteLine("Немає рецептів.");
            writer.WriteLine();

            // Записи на прийом
            writer.WriteLine("-- Записи --");
            Appointment[] appointments = _clinic.Appointments.GetByPatient(patientId);
            if (appointments.Length == 0)
            {
                writer.WriteLine("Немає записів.");
            }
            else
            {
                for (int i = 0; i < appointments.Length; i++)
                {
                    Appointment a = appointments[i];
                    writer.WriteLine($"[{a.Id}] {a.GetDescription()} | {a.ScheduledAt:dd.MM.yyyy HH:mm} | {a.Status} | {a.GetCost():F2} грн");
                }
            }
            writer.WriteLine();
            
            writer.WriteLine("-- Плани лікування --");
            TreatmentPlan[] plans = _clinic.TreatmentPlans.GetByPatient(patientId);
            if (plans.Length == 0)
            {
                writer.WriteLine("Немає планів.");
            }
            else
            {
                for (int i = 0; i < plans.Length; i++)
                {
                    TreatmentPlan p = plans[i];
                    writer.WriteLine($"[{p.Id}] {p.Diagnosis} | {p.DurationDays} днів | {p.Status}");
                }
            }
            writer.WriteLine();

            // заборгованість
            decimal debt = _clinic.Billing.GetPatientDebt(patientId);
            writer.WriteLine($"Заборгованість: {debt:F2} грн");
        }
    }
}