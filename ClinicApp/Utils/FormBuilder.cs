namespace ClinicApp.Utils;

using System;
using System.Collections.Generic;
using System.Reflection;
using ClinicApp.Attributes;

public static class FormBuilder
{
    public static T Build<T>() where T : new()
    {
        T obj = new T();
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo prop in properties)
        {
            // Пропускаємо властивості без публічного сеттера (Id, Status)
            if (prop.GetSetMethod() == null)
            {
                continue;
            }

            string hint = BuildHint(prop);
            string prompt = hint.Length > 0 
                ? $"  {prop.Name} ({hint}): " 
                : $"  {prop.Name}: ";

            bool success = false;
            while (!success)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()!;

                try
                {
                    object converted = Convert.ChangeType(input, prop.PropertyType);
                    prop.SetValue(obj, converted);
                    success = true;
                }
                catch
                {
                    Console.WriteLine("Некоректне значення, спробуйте ще раз.");
                }
            }
        }

        return obj;
    }

    private static string BuildHint(PropertyInfo prop)
    {
        List<string> parts = new List<string>();

        RequiredAttribute? required = prop.GetCustomAttribute<RequiredAttribute>();
        if (required != null)
        {
            parts.Add("обов'язкове");
        }

        MaxLengthAttribute? maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
        if (maxLength != null)
        {
            parts.Add($"макс. {maxLength.Length}");
        }

        MinValueAttribute? minValue = prop.GetCustomAttribute<MinValueAttribute>();
        if (minValue != null)
        {
            parts.Add($"мін. {minValue.Min}");
        }

        return string.Join(", ", parts);
    }
}