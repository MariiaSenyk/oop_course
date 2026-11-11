namespace ClinicApp.Utils;

using System;
using System.Reflection;
using ClinicApp.Attributes;

public static class ModelValidator
{
    public static ValidationResult Validate(object obj)
    {
        ValidationResult result = new ValidationResult();
        PropertyInfo[] properties = obj.GetType().GetProperties();

        foreach (PropertyInfo prop in properties)
        {
            object? value = prop.GetValue(obj);

            // перевірка Required
            RequiredAttribute? required = prop.GetCustomAttribute<RequiredAttribute>();
            if (required != null)
            {
                bool isEmpty = false;
                if (value == null)
                {
                    isEmpty = true;
                }
                else if (value is string str && string.IsNullOrWhiteSpace(str))
                {
                    isEmpty = true;
                }

                if (isEmpty)
                {
                    result.AddError($"{prop.Name}: {required.ErrorMessage}");
                }
            }

            //перевірка MaxLength
            MaxLengthAttribute? maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLength != null && value is string strValue)
            {
                if (strValue.Length > maxLength.Length)
                {
                    result.AddError($"{prop.Name}: {maxLength.ErrorMessage}");
                }
            }

            //перевірка MinValue
            MinValueAttribute? minValue = prop.GetCustomAttribute<MinValueAttribute>();
            if (minValue != null && value != null)
            {
                try
                {
                    double numValue = Convert.ToDouble(value);
                    if (numValue < minValue.Min)
                    {
                        result.AddError($"{prop.Name}: {minValue.ErrorMessage}");
                    }
                }
                catch
                {
                    //якщо не вдалося конвертувати в число то ігноруємо
                }
            }
        }

        return result;
    }

    public static void PrintInfo(Type type)
    {
        Console.WriteLine($"Тип: {type.Name}");
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo prop in properties)
        {
            Console.WriteLine($"  Властивість: {prop.Name} ({prop.PropertyType.Name})");
            var attrs = prop.GetCustomAttributes();

            foreach (object attr in attrs)
            {
                Console.WriteLine($"    Атрибут: {attr.GetType().Name}");
            }
        }
    }
}