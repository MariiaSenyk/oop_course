namespace ClinicApp.Comparators;

using System.Collections.Generic;
using ClinicApp.Models;

public class PatientStatsBySpent : IComparer<PatientStats>
{
    public int Compare(PatientStats? x, PatientStats? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        //спадання: більша сума йде першою
        return y.TotalSpent.CompareTo(x.TotalSpent);
    }
}