namespace ClinicApp.Comparators;

using System.Collections.Generic;
using ClinicApp.Models;

public class PatientStatsByLastVisit : IComparer<PatientStats>
{
    public int Compare(PatientStats? x, PatientStats? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        //спадання: найновіша дата йде першою
        return y.LastVisitDate.CompareTo(x.LastVisitDate);
    }
}