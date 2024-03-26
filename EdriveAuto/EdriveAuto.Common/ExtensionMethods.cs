using System.Globalization;

namespace EdriveAuto.Common;

public static class ExtensionMethods
{
    public static T ConvertTo<T>(this string input) where T : struct
    {
        return ConvertTo(input, default(T));
    }

    public static T ConvertTo<T>(this string input, params string[] replace) where T : struct
    {
        return ConvertTo(input, default(T), replace);
    }

    public static T ConvertTo<T>(this string input, T defaultValue, params string[] replace) where T : struct
    {
        try
        {
            if (string.IsNullOrWhiteSpace(input))
                return defaultValue;

            foreach (var i in replace)
            {
                input = input.Replace(i, "");
            }

            return (T)Convert.ChangeType(input.Trim(), typeof(T));
        }
        catch
        {
            return defaultValue;
        }
    }

    public static T ConvertTo<T>(this object? input, T defaultValue = default(T)) where T : struct
    {
        try
        {
            if (input == null)
                return defaultValue;

            return (T)Convert.ChangeType(input, typeof(T));
        }
        catch
        {
            return defaultValue;
        }
    }

    public static string? TrimToLower(this string? input, string? defaultValue = null)
    {
        if (input == null)
            return defaultValue;

        return input.ToLower().Trim();
    }

    public static string TrimCapitalize(this string? input, string? defaultValue = null)
    {
        if (string.IsNullOrWhiteSpace(input))
            input = defaultValue;

        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.TrimToLower(string.Empty) ?? string.Empty);
    }

    public static string ToLocalDate(this DateTime input, string defaultValue = "")
    {
        return input.Year > 1900 ? input.ToString("dd-MM-yyyy") : defaultValue;
    }

    public static string ToLocalDateTime(this DateTime input, string defaultValue = "")
    {
        return input.Year > 1900 ? input.ToString("dd-MM-yyyy HH:mm") : defaultValue;
    }

    public static string ToLocalDate(this DateTime? input, string defaultValue = "")
    {
        return input.HasValue ? input.Value.ToLocalDate(defaultValue) : defaultValue;
    }

    public static string ToLocalDateTime(this DateTime? input, string defaultValue = "")
    {
        return input.HasValue ? input.Value.ToLocalDateTime(defaultValue) : defaultValue;
    }
}