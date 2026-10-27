namespace ClinicApp.Managers;

using ClinicApp.Models;
using ClinicApp.Enums;

public class PatientManager
{
    private List<Patient> _patients = new List<Patient>();

    public int Count
    {
        get { return _patients.Count; }
    }

    public Patient? this[int index]
    {
        get
        {
            if (index < 0 || index >= _patients.Count)
            {
                return null;
            }
            return _patients[index];
        }
    }

    public void Add(Patient patient)
    {
        _patients.Add(patient);
        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }
        return null;
    }

    public bool TryFindById(int id, out Patient patient)
    {
        Patient? found = FindById(id);
        if (found == null)
        {
            patient = null!;
            return false;
        }

        patient = found;
        return true;
    }

    public Patient[] FindByName(string query)
    {
        string lower = query.ToLower();
        List<Patient> result = new List<Patient>();

        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(lower) ||
                _patients[i].LastName.ToLower().Contains(lower))
            {
                result.Add(_patients[i]);
            }
        }
        return result.ToArray();
    }

    public Patient[] FindByBloodType(BloodType bloodType)
    {
        List<Patient> result = new List<Patient>();

        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].BloodType == bloodType)
            {
                result.Add(_patients[i]);
            }
        }
        return result.ToArray();
    }

    public Patient[] GetAll()
    {
        return _patients.ToArray();
    }

    public bool Remove(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                _patients.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public void DisplayAll()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine($"=== Пацієнти ({_patients.Count}) ===");
        for (int i = 0; i < _patients.Count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
        Console.WriteLine("────────────────────────────────────────────────────────────");
    }

    public void DisplayStats()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Немає пацієнтів для статистики.");
            return;
        }

        int sum = 0;
        int minIdx = 0;
        int maxIdx = 0;
        int adults = 0;

        for (int i = 0; i < _patients.Count; i++)
        {
            sum += _patients[i].Age;
            if (_patients[i].Age < _patients[minIdx].Age) minIdx = i;
            if (_patients[i].Age > _patients[maxIdx].Age) maxIdx = i;
            if (_patients[i].IsAdult) adults++;
        }

        double average = (double)sum / _patients.Count;

        Console.WriteLine("=== Статистика пацієнтів ===");
        Console.WriteLine($"Всього:       {_patients.Count}");
        Console.WriteLine($"Середній вік: {average:F1} р.");
        Console.WriteLine($"Наймолодший:  {_patients[minIdx].FullName} ({_patients[minIdx].Age} р.)");
        Console.WriteLine($"Найстарший:   {_patients[maxIdx].FullName} ({_patients[maxIdx].Age} р.)");
        Console.WriteLine($"Дорослих:     {adults} з {_patients.Count}");
        Console.WriteLine("============================");
    }
}