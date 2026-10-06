namespace ClinicApp.Utils;

using System.Text.RegularExpressions;

public static class ClinicValidator
{
    private static readonly Regex PhonePattern = new Regex(@"^(?:[0-9]{10}|\+38[0-9]{10})\z");
    private static readonly Regex EmailPattern = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+\z");

    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
        {
            throw new ArgumentException("Ім'я не може бути порожнім або довшим за 50 символів.", fieldName);
        }
    }

    public static void ValidatePhone(string phone)
    {
        if (!PhonePattern.IsMatch(phone))
        {
            throw new ArgumentException("Телефон має містити 10 цифр (або +38 і 10 цифр).", nameof(phone));
        }
    }

    public static void ValidateEmail(string email)
    {
        if (email.Length == 0)
        {
            return;
        }
        if (!EmailPattern.IsMatch(email))
        {
            throw new ArgumentException("Email має некоректний формат.", nameof(email));
        }
    }

    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value > DateTime.Today)
        {
            throw new ArgumentOutOfRangeException(fieldName, "Дата не може бути в майбутньому.");
        }
        if (value.Year < 1900)
        {
            throw new ArgumentOutOfRangeException(fieldName, "Дата не може бути раніше 1900 року.");
        }
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(fieldName, "Значення має бути більшим за нуль.");
        }
    }
}