using System;

namespace RfqCopilot.Domain.Pricing;

/// <summary>
/// Thrown when pricing inputs are invalid. Calculations must fail loudly
/// rather than return a fabricated number (e.g. dividing by a zero selling price).
/// </summary>
public sealed class PricingValidationException : Exception
{
    public PricingValidationException(string message) : base(message)
    {
    }
}
