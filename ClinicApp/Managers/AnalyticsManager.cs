namespace ClinicApp.Managers;

using System;
using System.Collections.Generic;
using System.Linq;
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

        return allDoctors.Select(d =>
        {
            Appointment[] doctorAppointments = allAppointments.Where(a => a.DoctorId == d.Id).ToArray();
            
            int count = doctorAppointments.Count();
            decimal totalRevenue = doctorAppointments.Sum(a => a.GetCost());
            DateTime lastDate = doctorAppointments.Any() 
                ? doctorAppointments.Max(a => a.ScheduledAt) 
                : DateTime.MinValue;

            return new DoctorStats(d.Id, d.FullName, count, totalRevenue, lastDate);
        });
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Patient[] allPatients = _patients.GetAll();
        Appointment[] allAppointments = _appointments.GetAll(); // Отримуємо один раз

        return allPatients.Select(p =>
        {
            Appointment[] patientAppointments = allAppointments.Where(a => a.PatientId == p.Id).ToArray();
            
            int count = patientAppointments.Count();
            decimal totalSpent = patientAppointments.Sum(a => a.GetCost());
            DateTime lastDate = patientAppointments.Any() 
                ? patientAppointments.Max(a => a.ScheduledAt) 
                : DateTime.MinValue;

            return new PatientStats(p.Id, p.FullName, count, totalSpent, lastDate);
        });
    }
}