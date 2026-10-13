namespace ClinicApp.Managers;

using ClinicApp.Models;

public class MedicalRecordManager
{
    private const int MaxRecords = 1000;
    private MedicalRecord[] _records = new MedicalRecord[MaxRecords];
    private int _count = 0;

    public int Count
    {
        get { return _count; }
    }

    public MedicalRecord? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
            {
                return null;
            }
            return _records[index];
        }
    }

    public void Add(MedicalRecord record)
    {
        if (_count >= MaxRecords)
        {
            Console.WriteLine("Досягнуто ліміт записів.");
            return;
        }

        _records[_count] = record;
        _count++;
        Console.WriteLine($"Запис [{record.Id}] {record.GetRecordType()} додано.");
    }

    public MedicalRecord? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].Id == id)
            {
                return _records[i];
            }
        }
        return null;
    }

    public MedicalRecord[] GetByPatient(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId) matches++;
        }

        MedicalRecord[] result = new MedicalRecord[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
            {
                result[index] = _records[i];
                index++;
            }
        }
        return result;
    }

    public MedicalRecord[] GetByDoctor(int doctorId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId) matches++;
        }

        MedicalRecord[] result = new MedicalRecord[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
            {
                result[index] = _records[i];
                index++;
            }
        }
        return result;
    }

    public Diagnosis[] GetDiagnoses(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis)
            {
                matches++;
            }
        }

        Diagnosis[] result = new Diagnosis[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis diagnosis)
            {
                result[index] = diagnosis;
                index++;
            }
        }
        return result;
    }

    public LabResult[] GetLabResults(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is LabResult)
            {
                matches++;
            }
        }

        LabResult[] result = new LabResult[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is LabResult labResult)
            {
                result[index] = labResult;
                index++;
            }
        }
        return result;
    }

    public Prescription[] GetPrescriptions(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription)
            {
                matches++;
            }
        }

        Prescription[] result = new Prescription[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription prescription)
            {
                result[index] = prescription;
                index++;
            }
        }
        return result;
    }

    public Diagnosis[] GetChronicDiagnoses(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis d && d.IsChronic)
            {
                matches++;
            }
        }

        Diagnosis[] result = new Diagnosis[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis d && d.IsChronic)
            {
                result[index] = d;
                index++;
            }
        }
        return result;
    }

    public Prescription[] GetActivePrescriptions(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription p && p.IsActive())
            {
                matches++;
            }
        }

        Prescription[] result = new Prescription[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription p && p.IsActive())
            {
                result[index] = p;
                index++;
            }
        }
        return result;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список медичних записів порожній.");
            return;
        }

        Console.WriteLine($"=== Медичні записи ({_count} / {MaxRecords}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_records[i]);
        }
        Console.WriteLine("────────────────────────────────────────────────────────────");
    }

    public void DisplayList(MedicalRecord[] records)
    {
        if (records.Length == 0)
        {
            Console.WriteLine("Не знайдено.");
            return;
        }

        for (int i = 0; i < records.Length; i++)
        {
            Console.WriteLine(records[i]);
        }
    }

    public void DisplayPatientSummary(int patientId)
    {
        MedicalRecord[] patientRecords = GetByPatient(patientId);

        if (patientRecords.Length == 0)
        {
            Console.WriteLine($"У пацієнта #{patientId} немає медичних записів.");
            return;
        }

        int diagnoses = 0;
        int labResults = 0;
        int prescriptions = 0;

        for (int i = 0; i < patientRecords.Length; i++)
        {
            if (patientRecords[i] is Diagnosis) diagnoses++;
            else if (patientRecords[i] is LabResult) labResults++;
            else if (patientRecords[i] is Prescription) prescriptions++;
        }

        Console.WriteLine($"=== Медична картка пацієнта #{patientId} ===");
        Console.WriteLine($"Всього записів: {patientRecords.Length} (діагнозів: {diagnoses}, аналізів: {labResults}, рецептів: {prescriptions})");

        Diagnosis[] chronic = GetChronicDiagnoses(patientId);
        if (chronic.Length > 0)
        {
            Console.WriteLine($"Хронічні діагнози ({chronic.Length}):");
            for (int i = 0; i < chronic.Length; i++)
            {
                Console.WriteLine("  " + chronic[i]);
            }
        }

        Prescription[] active = GetActivePrescriptions(patientId);
        if (active.Length > 0)
        {
            Console.WriteLine($"Активні рецепти ({active.Length}):");
            for (int i = 0; i < active.Length; i++)
            {
                Console.WriteLine($"  {active[i]} | до {active[i].ExpiresAt:dd.MM.yyyy}");
            }
        }
    }
}