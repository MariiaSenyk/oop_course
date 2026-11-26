namespace ClinicApp.Managers;

using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Enums;
using ClinicApp.Models;

public class ReportManager
{
    private AppointmentManager _appointments;
    private DoctorManager _doctors;
    private PatientManager _patients;
    private AnalyticsManager _analytics;

    public ReportManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
        _analytics = new AnalyticsManager(appointments, doctors, patients);
    }

    public IEnumerable<SpecialityReport> GetSpecialityStats()
    {
        Appointment[] allAppointments = _appointments.GetAll();

        return _doctors.GetAll()
            .GroupBy(d => d.Speciality)
            .Select(g =>
            {
                int[] doctorIds = g.Select(d => d.Id).ToArray();
                Appointment[] docAppointments = allAppointments.Where(a => doctorIds.Contains(a.DoctorId)).ToArray();
                decimal revenue = docAppointments.Sum(a => a.GetCost());
                
                return new SpecialityReport(g.Key, g.Count(), docAppointments.Length, revenue);
            })
            .OrderByDescending(r => r.TotalRevenue);
    }

    public string? FindBusiestDoctorName()
    {
        return _analytics.ComputeDoctorStats()
            .OrderByDescending(s => s.AppointmentCount)
            .FirstOrDefault()?.FullName;
    }

    public IEnumerable<string> GetPatientsWithMultipleVisits(int minVisits)
    {
        return _appointments.GetAll()
            .GroupBy(a => a.PatientId)
            .Where(g => g.Count() >= minVisits)
            .Join(_patients.GetAll(),
                  g => g.Key,
                  p => p.Id,
                  (g, p) => p.FullName);
    }

    public IEnumerable<DoctorStats> GetTopEarners(int n)
    {
        return _analytics.ComputeDoctorStats()
            .OrderByDescending(s => s.TotalRevenue)
            .Take(n);
    }

    public bool HasAnyUrgentAppointments()
    {
        return _appointments.GetAll().Any(a => a is UrgentAppointment);
    }

    public IEnumerable<Speciality> GetActiveSpecialities()
    {
        return _doctors.GetAll()
            .Select(d => d.Speciality)
            .Distinct()
            .OrderBy(s => s);
    }

    public IEnumerable<(int Year, int Month, decimal Total)> GetMonthlyRevenue()
    {
        return _appointments.GetAll()
            .GroupBy(a => new { a.ScheduledAt.Year, a.ScheduledAt.Month })
            .Select(g => (g.Key.Year, g.Key.Month, g.Sum(a => a.GetCost())))
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month);
    }
}