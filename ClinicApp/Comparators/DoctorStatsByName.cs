namespace ClinicApp.Comparators;

using System.Collections.Generic;
using ClinicApp.Models;

public class DoctorStatsByName : IComparer<DoctorStats>
{
    public int Compare(DoctorStats? x, DoctorStats? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        //зростання: від А до Я
        return string.Compare(x.FullName, y.FullName, StringComparison.CurrentCulture);
    }
}