namespace ClinicApp.Managers;

using System.Collections.Generic;
using ClinicApp.Models;

public class AnalyticsManager
{
    private AppointmentManager _appointments;
    private DoctorManager _doctors;
    private PatientManager _patients;

    public AnalyticsManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
    }

    public IEnumerable<DoctorStats> ComputeDoctorStats()
    {
        Doctor[] allDoctors = _doctors.GetAll();
        Appointment[] allAppointments = _appointments.GetAll();

        for (int i = 0; i < allDoctors.Length; i++)
        {
            Doctor doctor = allDoctors[i];
            int count = 0;
            decimal totalRevenue = 0m;
            DateTime lastDate = DateTime.MinValue;

            for (int j = 0; j < allAppointments.Length; j++)
            {
                if (allAppointments[j].DoctorId == doctor.Id)
                {
                    count++;
                    totalRevenue += allAppointments[j].GetCost();
                    if (allAppointments[j].ScheduledAt > lastDate)
                    {
                        lastDate = allAppointments[j].ScheduledAt;
                    }
                }
            }

            // Ліниве обчислення: віддаємо результат по одному
            yield return new DoctorStats(doctor.Id, doctor.FullName, count, totalRevenue, lastDate);
        }
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Patient[] allPatients = _patients.GetAll();
        Appointment[] allAppointments = _appointments.GetAll();

        for (int i = 0; i < allPatients.Length; i++)
        {
            Patient patient = allPatients[i];
            int count = 0;
            decimal totalSpent = 0m;
            DateTime lastDate = DateTime.MinValue;

            for (int j = 0; j < allAppointments.Length; j++)
            {
                if (allAppointments[j].PatientId == patient.Id)
                {
                    count++;
                    totalSpent += allAppointments[j].GetCost();
                    if (allAppointments[j].ScheduledAt > lastDate)
                    {
                        lastDate = allAppointments[j].ScheduledAt;
                    }
                }
            }

            yield return new PatientStats(patient.Id, patient.FullName, count, totalSpent, lastDate);
        }
    }
}