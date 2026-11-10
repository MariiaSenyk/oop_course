namespace ClinicApp.Models;

public class DoctorStats : IComparable<DoctorStats>
{
    public int DoctorId { get; }
    public string FullName { get; }
    public int AppointmentCount { get; }
    public decimal TotalRevenue { get; }
    public DateTime LastAppointmentDate { get; }

    public DoctorStats(int doctorId, string fullName, int appointmentCount, decimal totalRevenue, DateTime lastAppointmentDate)
    {
        DoctorId = doctorId;
        FullName = fullName;
        AppointmentCount = appointmentCount;
        TotalRevenue = totalRevenue;
        LastAppointmentDate = lastAppointmentDate;
    }

    public int CompareTo(DoctorStats? other)
    {
        if (other == null)
        {
            return -1; //null вважається "меншим", тож this йде першим
        }

        //Сортування за спаданням кількості прийомів:
        // якщо в this більше прийомів, він має бути першим (повертаємо -1)
        return other.AppointmentCount.CompareTo(AppointmentCount);
    }

    public override string ToString()
    {
        return $"[{DoctorId}] {FullName} | Прийомів: {AppointmentCount} | Виручка: {TotalRevenue:F2} грн | Останній: {LastAppointmentDate:dd.MM.yyyy}";
    }
}