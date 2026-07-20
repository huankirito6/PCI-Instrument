using System;

namespace RfqCopilot.Domain.Normalization;

/// <summary>
/// Deterministic temperature conversions between Celsius and Fahrenheit.
/// Accepts "C"/"F" or "degC"/"degF" (case-insensitive). Unknown units throw.
/// </summary>
public static class TemperatureUnit
{
    public static decimal CelsiusToFahrenheit(decimal celsius)
        => celsius * 9m / 5m + 32m;

    public static decimal FahrenheitToCelsius(decimal fahrenheit)
        => (fahrenheit - 32m) * 5m / 9m;

    public static decimal Convert(decimal value, string fromUnit, string toUnit)
    {
        var from = ParseUnit(fromUnit);
        var to = ParseUnit(toUnit);

        if (from == to)
        {
            return value;
        }

        // Convert to Celsius first, then to the target scale.
        var celsius = from == "C" ? value : FahrenheitToCelsius(value);
        return to == "C" ? celsius : CelsiusToFahrenheit(celsius);
    }

    private static string ParseUnit(string unit)
    {
        if (unit is null)
        {
            throw new NormalizationException("Temperature unit cannot be null.");
        }

        var u = unit.Trim();
        if (u.StartsWith("deg", StringComparison.OrdinalIgnoreCase))
        {
            u = u.Substring(3);
        }

        u = u.ToUpperInvariant();
        if (u is not ("C" or "F"))
        {
            throw new NormalizationException(
                $"Unsupported temperature unit '{unit}'. Supported: C, F (or degC, degF).");
        }

        return u;
    }
}
