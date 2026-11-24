namespace ClinicApp.Utils;

using System;
using System.IO;
using System.Text;
using ClinicApp.Events;

public class ClinicLogger
{
    private readonly string _logPath;

    public ClinicLogger(string logPath = "clinic.log")
    {
        _logPath = logPath;
    }

    public bool Exists() => File.Exists(_logPath);

    public void LogInfo(string message) => Log("INFO ", message);
    public void LogWarning(string message) => Log("WARN ", message);
    public void LogError(string message) => Log("ERROR", message);

    private void Log(string level, string message)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string line = $"[{timestamp}] [{level}] {message}" + Environment.NewLine;
        File.AppendAllText(_logPath, line, Encoding.UTF8);
    }

    public string[] GetLastLines(int n)
    {
        if (!File.Exists(_logPath) || n <= 0) return Array.Empty<string>();
        string[] lines = File.ReadAllLines(_logPath, Encoding.UTF8);
        int start = Math.Max(0, lines.Length - n);
        string[] result = new string[lines.Length - start];
        for (int i = 0; i < result.Length; i++) result[i] = lines[start + i];
        return result;
    }

    public void Clear()
    {
        if (File.Exists(_logPath)) File.Delete(_logPath);
    }

    // === Обробники подій ===

    public void OnAppointmentBooked(object? sender, AppointmentEventArgs e)
    {
        LogInfo($"Запис #{e.AppointmentId} створено: пацієнт #{e.PatientId} → лікар #{e.DoctorId}");
    }

    public void OnUrgentAppointmentBooked(object? sender, AppointmentEventArgs e)
    {
        LogWarning($"ТЕРМІНОВИЙ запис #{e.AppointmentId}: {e.Notes}");
        WriteAlert($"ТЕРМІНОВИЙ запис #{e.AppointmentId}: {e.Notes}");
    }

    public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e)
    {
        LogInfo($"Запис #{e.AppointmentId} скасовано: {e.Notes}");
    }

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e)
    {
        LogInfo($"Запис #{e.AppointmentId} завершено");
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e)
    {
        LogInfo($"Новий пацієнт #{e.PatientId}: {e.FullName}");
    }

    public void OnPaymentReceived(object? sender, PaymentEventArgs e)
    {
        LogInfo($"Оплата запису #{e.AppointmentId}: {e.Amount:F2} грн");
    }

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e)
    {
        LogInfo($"План лікування #{e.PlanId} завершено (пацієнт #{e.PatientId}, діагноз: {e.Diagnosis})");
    }

    private void WriteAlert(string message)
    {
        string dir = "alerts";
        Directory.CreateDirectory(dir);
        string fileName = $"urgent_{DateTime.Now:yyyy-MM-dd}.txt";
        string path = Path.Combine(dir, fileName);
        string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}" + Environment.NewLine;
        File.AppendAllText(path, line, Encoding.UTF8);
    }
}