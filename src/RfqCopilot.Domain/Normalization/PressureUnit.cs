using System;
using System.Collections.Generic;

namespace RfqCopilot.Domain.Normalization;

/// <summary>
/// Deterministic pressure-unit conversions for the units in V0 scope
/// (kPa, MPa, bar, mbar). Unknown units throw rather than being silently dropped.
/// </summary>
public static class PressureUnit
{
    // Factor to convert 1 unit into kilopascals.
    private static readonly IReadOnlyDictionary<string, decimal> ToKpaFactor =
        new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
        {
            ["kPa"] = 1m,
            ["MPa"] = 1000m,
            ["bar"] = 100m,
            ["mbar"] = 0.1m,
        };

    public static bool IsSupported(string unit)
        => unit is not null && ToKpaFactor.ContainsKey(unit.Trim());

    public static decimal ToKilopascals(decimal value, string unit)
        => value * Factor(unit);

    public static decimal Convert(decimal value, string fromUnit, string toUnit)
    {
        var kpa = ToKilopascals(value, fromUnit);
        return kpa / Factor(toUnit);
    }

    private static decimal Factor(string unit)
    {
        if (unit is null || !ToKpaFactor.TryGetValue(unit.Trim(), out var factor))
        {
            throw new NormalizationException(
                $"Unsupported pressure unit '{unit}'. Supported: kPa, MPa, bar, mbar.");
        }

        return factor;
    }
}
