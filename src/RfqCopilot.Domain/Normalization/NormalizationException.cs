using System;

namespace RfqCopilot.Domain.Normalization;

/// <summary>
/// Thrown when a value cannot be normalized deterministically — for example an
/// unsupported unit. Normalization must fail loudly rather than guess.
/// </summary>
public sealed class NormalizationException : Exception
{
    public NormalizationException(string message) : base(message)
    {
    }
}
