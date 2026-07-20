using System.Text.RegularExpressions;

namespace RfqCopilot.Domain.Normalization;

/// <summary>
/// Result of normalizing an output-signal string.
/// </summary>
/// <param name="RawValue">The original input.</param>
/// <param name="NormalizedValue">Canonical value (e.g. "4-20mA", "4-20mA_HART"), or the raw value when unknown.</param>
/// <param name="IsUnknown">True when the signal was not recognized.</param>
/// <param name="Warning">A warning when the signal is unknown; null otherwise.</param>
public sealed record SignalNormalizationResult(
    string RawValue,
    string NormalizedValue,
    bool IsUnknown,
    string? Warning);

/// <summary>
/// Deterministic normalization of analog output-signal strings.
///
/// Rules:
///   - Recognizes 4-20mA written many ways ("4-20mA", "4 to 20mA", "4...20mA", "4~20mA", ...).
///   - Adds _HART only when the text explicitly mentions HART. Never infers HART from 4-20mA alone.
///   - Unrecognized signals are preserved with a warning, never dropped.
/// </summary>
public static class SignalNormalizer
{
    // 4 <sep> 20 mA, where sep is -, en/em dash, ~, two-or-more dots, or the word "to".
    private static readonly Regex FourToTwentyMilliamp = new(
        @"4\s*(?:-|\u2013|\u2014|~|\.{2,}|to)\s*20\s*ma",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static SignalNormalizationResult Normalize(string raw)
    {
        var input = (raw ?? string.Empty).Trim();

        if (input.Length == 0)
        {
            return new SignalNormalizationResult(
                RawValue: input,
                NormalizedValue: input,
                IsUnknown: true,
                Warning: "Empty output-signal value.");
        }

        var lower = input.ToLowerInvariant();

        if (FourToTwentyMilliamp.IsMatch(lower))
        {
            var hasHart = lower.Contains("hart");
            var normalized = hasHart ? "4-20mA_HART" : "4-20mA";
            return new SignalNormalizationResult(input, normalized, IsUnknown: false, Warning: null);
        }

        return new SignalNormalizationResult(
            RawValue: input,
            NormalizedValue: input,
            IsUnknown: true,
            Warning: $"Unrecognized output signal '{input}'; preserved for human review, not normalized.");
    }
}
