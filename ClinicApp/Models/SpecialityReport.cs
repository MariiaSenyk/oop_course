namespace ClinicApp.Models;

using ClinicApp.Enums;
using ClinicApp.Utils;

public class SpecialityReport
{
    public Speciality Speciality { get; }
    public int DoctorCount { get; }
    public int AppointmentCount { get; }
    public decimal TotalRevenue { get; }

    public SpecialityReport(Speciality speciality, int doctorCount, int appointmentCount, decimal totalRevenue)
    {
        Speciality = speciality;
        DoctorCount = doctorCount;
        AppointmentCount = appointmentCount;
        TotalRevenue = totalRevenue;
    }

    public override string ToString()
    {
        string specName = ClinicFormatter.FormatSpeciality(Speciality);
        return $"{specName} | Лікарів: {DoctorCount} | Прийомів: {AppointmentCount} | Виручка: {TotalRevenue:F2} грн";
    }
}