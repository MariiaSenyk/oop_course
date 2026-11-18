namespace ClinicApp.Utils;

using System;
using System.IO;
using System.Text;

public class ClinicLogger
{
    private readonly string _logPath;

    public ClinicLogger(string logPath = "clinic.log")
    {
        _logPath = logPath;
    }

    public bool Exists()
    {
        return File.Exists(_logPath);
    }

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
        if (!File.Exists(_logPath) || n <= 0)
        {
            return Array.Empty<string>();
        }

        string[] lines = File.ReadAllLines(_logPath, Encoding.UTF8);
        int start = Math.Max(0, lines.Length - n);
        string[] result = new string[lines.Length - start];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = lines[start + i];
        }
        return result;
    }

    public void Clear()
    {
        if (File.Exists(_logPath))
        {
            File.Delete(_logPath);
        }
    }
}