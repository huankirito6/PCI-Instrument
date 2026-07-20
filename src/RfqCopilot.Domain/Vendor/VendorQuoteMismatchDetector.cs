using System;
using System.Collections.Generic;

namespace RfqCopilot.Domain.Vendor;

/// <summary>Which field a mismatch warning refers to.</summary>
public enum MismatchField
{
    Model,
    Range,
    Quantity,
}

/// <summary>A single mismatch between the proposed line and the vendor-confirmed line.</summary>
/// <param name="Field">The field that differs.</param>
/// <param name="ProposedValue">The proposed value (as text).</param>
/// <param name="VendorValue">The vendor-confirmed value (as text).</param>
public sealed record MismatchWarning(MismatchField Field, string ProposedValue, string VendorValue);

/// <summary>Result of comparing a proposed line to a vendor-confirmed line.</summary>
/// <param name="HasMismatch">True when at least one field differs.</param>
/// <param name="Warnings">One warning per differing field.</param>
public sealed record MismatchResult(bool HasMismatch, IReadOnlyList<MismatchWarning> Warnings);

/// <summary>
/// Deterministic comparison of a proposed line against the vendor's confirmed line.
///
/// Rules:
///   - Model and range compare case-insensitively and ignore surrounding/internal
///     whitespace, but never treat different values as equal.
///   - An empty vendor model is a mismatch, not silently accepted.
///   - Every differing field produces a warning carrying both values so a human
///     can see exactly what changed.
/// </summary>
public static class VendorQuoteMismatchDetector
{
    public static MismatchResult Compare(ProposedLine proposed, VendorConfirmedLine vendor)
    {
        ArgumentNullException.ThrowIfNull(proposed);
        ArgumentNullException.ThrowIfNull(vendor);

        var warnings = new List<MismatchWarning>();

        if (!TextEquivalent(proposed.Model, vendor.Model))
        {
            warnings.Add(new MismatchWarning(MismatchField.Model, proposed.Model ?? "", vendor.Model ?? ""));
        }

        if (!TextEquivalent(proposed.Range, vendor.Range))
        {
            warnings.Add(new MismatchWarning(MismatchField.Range, proposed.Range ?? "", vendor.Range ?? ""));
        }

        if (proposed.Quantity != vendor.Quantity)
        {
            warnings.Add(new MismatchWarning(
                MismatchField.Quantity,
                proposed.Quantity.ToString(),
                vendor.Quantity.ToString()));
        }

        return new MismatchResult(warnings.Count > 0, warnings);
    }

    private static bool TextEquivalent(string? a, string? b)
        => string.Equals(Canonical(a), Canonical(b), StringComparison.OrdinalIgnoreCase);

    private static string Canonical(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        // Remove all whitespace so spacing differences don't count, but keep the characters
        // that carry the value (digits, units, separators).
        var chars = new List<char>(value.Length);
        foreach (var ch in value)
        {
            if (!char.IsWhiteSpace(ch))
            {
                chars.Add(ch);
            }
        }

        return new string(chars.ToArray());
    }
}
