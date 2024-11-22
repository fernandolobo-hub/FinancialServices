using System;
using System.Globalization;

public static class TypeConverter
{
    public static DateTime ToDateTime(object value)
    {
        if (value is DateTime dateTimeValue)
        {
            return dateTimeValue;
        }
        else if (value is string stringValue)
        {
            if (DateTime.TryParseExact(stringValue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }
            else if (DateTime.TryParseExact(stringValue, "ddMMyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                int year = 2000 + int.Parse(stringValue.Substring(4, 2)); // Gambiarra para que o ano esteja entre 2000 e 2099 - BUG DO SÉCULO!!!
                int month = int.Parse(stringValue.Substring(2, 2));
                int day = int.Parse(stringValue.Substring(0, 2));

                return new DateTime(year, month, day);
            }
            else if (DateTime.TryParse(stringValue, out parsedDate))
            {
                return parsedDate;
            }
        }

        throw new InvalidCastException($"O valor fornecido '{value}' não é uma data válida.");
    }

    public static decimal ToPercentageDecimal(object value)
    {
        if (value is double doubleValue)
        {
            return Convert.ToDecimal(doubleValue);
        }
        else if (value is string stringValue && double.TryParse(stringValue, out double parsedValue))
        {
            return Convert.ToDecimal(parsedValue);
        }

        throw new InvalidCastException("O valor fornecido não é um número válido.");
    }

    public static decimal ToDecimal(object value)
    {
        if (value is double doubleValue)
        {
            return Convert.ToDecimal(doubleValue);
        }
        else if (value is decimal decimalValue)
        {
            return decimalValue;
        }
        else if (value is string stringValue && decimal.TryParse(stringValue, out decimal parsedValue))
        {
            return parsedValue;
        }

        throw new InvalidCastException("O valor fornecido não pode ser convertido para decimal.");
    }
}
