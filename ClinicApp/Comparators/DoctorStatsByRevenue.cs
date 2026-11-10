namespace ClinicApp.Comparators;

using System.Collections.Generic;
using ClinicApp.Models;

public class DoctorStatsByRevenue : IComparer<DoctorStats>
{
    public int Compare(DoctorStats? x, DoctorStats? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        //спадання: більша виручка йде першою
        return y.TotalRevenue.CompareTo(x.TotalRevenue);
    }
}