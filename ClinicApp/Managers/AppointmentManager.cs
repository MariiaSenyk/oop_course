namespace ClinicApp.Managers;

using ClinicApp.Interfaces;
using ClinicApp.Models;

public class AppointmentManager
{
    private const int MaxAppointments = 500;
    private Appointment[] _appointments = new Appointment[MaxAppointments];
    private int _count = 0;

    private PatientManager _patients;
    private DoctorManager _doctors;

    public int Count
    {
        get { return _count; }
    }

    public Appointment? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
            {
                return null;
            }
            return _appointments[index];
        }
    }

    public AppointmentManager(PatientManager patients, DoctorManager doctors)
    {
        _patients = patients;
        _doctors = doctors;
    }

    public Appointment[] GetAll()
    {
        Appointment[] copy = new Appointment[_count];
        for (int i = 0; i < _count; i++)
        {
            copy[i] = _appointments[i];
        }
        return copy;
    }

    public static int CancelAll(ICancellable[] items, string reason = "")
    {
        int cancelled = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Cancel(reason))
            {
                cancelled++;
            }
        }
        return cancelled;
    }

    public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        RegularAppointment appointment = new RegularAppointment(patientId, doctorId, scheduledAt, durationMinutes);
        return AddAppointment(appointment);
    }

    public bool BookUrgent(int patientId, int doctorId, DateTime scheduledAt, string urgencyNote = "", int durationMinutes = 30)
    {
        UrgentAppointment appointment = new UrgentAppointment(patientId, doctorId, scheduledAt, urgencyNote, durationMinutes);
        return AddAppointment(appointment);
    }

    public bool BookSpecialist(int patientId, int doctorId, DateTime scheduledAt, string topic = "", int durationMinutes = 45)
    {
        SpecialistAppointment appointment = new SpecialistAppointment(patientId, doctorId, scheduledAt, topic, durationMinutes);
        return AddAppointment(appointment);
    }

    private bool AddAppointment(Appointment appointment)
    {
        if (_count >= MaxAppointments)
        {
            Console.WriteLine("Досягнуто ліміт записів.");
            return false;
        }

        Patient? patient = _patients.FindById(appointment.PatientId);
        if (patient == null)
        {
            Console.WriteLine($"Помилка: пацієнта з ID {appointment.PatientId} не знайдено.");
            return false;
        }

        Doctor? doctor = _doctors.FindById(appointment.DoctorId);
        if (doctor == null)
        {
            Console.WriteLine($"Помилка: лікаря з ID {appointment.DoctorId} не знайдено.");
            return false;
        }

        _appointments[_count] = appointment;
        _count++;

        Console.WriteLine($"Запис [{appointment.Id}] створено: {patient.FullName} → {doctor.FullName} о {appointment.ScheduledAt:dd.MM.yyyy HH:mm}");
        return true;
    }

    public bool Cancel(int id, string reason = "")
    {
        Appointment? found = FindById(id);
        if (found == null)
        {
            return false;
        }

        return found.Cancel(reason);
    }

    public bool Complete(int id)
    {
        Appointment? found = FindById(id);
        if (found == null)
        {
            return false;
        }

        return found.Complete();
    }

    public Appointment[] GetByPatient(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId) matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public Appointment[] GetByDoctor(int doctorId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId) matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public Appointment[] GetByDate(DateTime date)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date) matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public Appointment[] GetByDate(int year, int month, int day)
    {
        return GetByDate(new DateTime(year, month, day));
    }

    public Appointment[] GetUpcoming()
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming) matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public Appointment[] GetUrgent()
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is UrgentAppointment) matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is UrgentAppointment)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public Appointment[] GetSpecialist()
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is SpecialistAppointment) matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is SpecialistAppointment)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public Appointment[] GetRegular()
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is RegularAppointment) matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is RegularAppointment)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public void DisplayAppointment(Appointment appointment)
    {
        Patient? patient = _patients.FindById(appointment.PatientId);
        Doctor? doctor = _doctors.FindById(appointment.DoctorId);

        string patientName = patient != null ? patient.FullName : $"Пацієнт #{appointment.PatientId}";
        string doctorName = doctor != null ? doctor.FullName : $"Лікар #{appointment.DoctorId}";

        string line = $"[{appointment.Id}] {appointment.GetDescription()} | " +
                      $"{patientName} → {doctorName} | " +
                      $"{appointment.ScheduledAt:dd.MM.yyyy HH:mm}–{appointment.EndsAt:HH:mm} | {appointment.Status} | " +
                      $"{appointment.GetCost():F2} грн";

        if (appointment.Notes.Length > 0)
        {
            line += $" | {appointment.Notes}";
        }

        Console.WriteLine(line);
    }

    public void DisplayList(Appointment[] appointments)
    {
        if (appointments.Length == 0)
        {
            Console.WriteLine("Не знайдено.");
            return;
        }

        for (int i = 0; i < appointments.Length; i++)
        {
            DisplayAppointment(appointments[i]);
        }
    }

    private Appointment? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].Id == id)
            {
                return _appointments[i];
            }
        }
        return null;
    }
}