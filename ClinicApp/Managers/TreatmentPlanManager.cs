namespace ClinicApp.Managers;

using System;
using System.Collections.Generic;
using ClinicApp.Events;
using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Utils;

public class TreatmentPlanManager
{
    private List<TreatmentPlan> _plans = new List<TreatmentPlan>();

    public event EventHandler<TreatmentPlanEventArgs>? PlanCompleted;

    public int Count
    {
        get { return _plans.Count; }
    }

    public bool Add(TreatmentPlan plan)
    {
        ValidationResult result = ModelValidator.Validate(plan);
        if (!result.IsValid)
        {
            Console.WriteLine("Помилки валідації:");
            result.Print();
            return false;
        }

        _plans.Add(plan);
        Console.WriteLine($"План лікування [{plan.Id}] додано.");
        return true;
    }

    public TreatmentPlan? GetById(int id)
    {
        for (int i = 0; i < _plans.Count; i++)
        {
            if (_plans[i].Id == id) return _plans[i];
        }
        return null;
    }

    public TreatmentPlan[] GetByPatient(int patientId)
    {
        List<TreatmentPlan> result = new List<TreatmentPlan>();
        for (int i = 0; i < _plans.Count; i++)
        {
            if (_plans[i].PatientId == patientId) result.Add(_plans[i]);
        }
        return result.ToArray();
    }

    public TreatmentPlan[] GetByStatus(TreatmentStatus status)
    {
        List<TreatmentPlan> result = new List<TreatmentPlan>();
        for (int i = 0; i < _plans.Count; i++)
        {
            if (_plans[i].Status == status) result.Add(_plans[i]);
        }
        return result.ToArray();
    }

    public TreatmentPlan[] GetAll()
    {
        return _plans.ToArray();
    }

    public bool Complete(int planId)
    {
        TreatmentPlan? plan = GetById(planId);
        if (plan == null) return false;
        
        if (plan.Complete())
        {
            PlanCompleted?.Invoke(this, new TreatmentPlanEventArgs(plan.Id, plan.PatientId, plan.Diagnosis));
            return true;
        }
        return false;
    }
}