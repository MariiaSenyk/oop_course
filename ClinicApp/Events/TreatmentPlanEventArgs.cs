namespace ClinicApp.Events;

using System;

public class TreatmentPlanEventArgs : EventArgs
{
    public int PlanId { get; }
    public int PatientId { get; }
    public string Diagnosis { get; }

    public TreatmentPlanEventArgs(int planId, int patientId, string diagnosis)
    {
        PlanId = planId;
        PatientId = patientId;
        Diagnosis = diagnosis;
    }
}