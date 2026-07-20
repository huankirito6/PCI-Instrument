using System.Collections.Generic;

namespace RfqCopilot.Domain.Pricing;

/// <summary>
/// Aggregate totals for a quotation:
///   Subtotal   = sum of line totals
///   Vat        = Subtotal * (vatRatePercent / 100)
///   GrandTotal = Subtotal + Vat
/// </summary>
public readonly record struct QuotationTotals(decimal Subtotal, decimal Vat, decimal GrandTotal)
{
    public static QuotationTotals From(IEnumerable<PricingResult> lines, decimal vatRatePercent)
    {
        if (vatRatePercent < 0m)
        {
            throw new PricingValidationException("VAT rate cannot be negative.");
        }

        decimal subtotal = 0m;
        foreach (var line in lines)
        {
            subtotal += line.LineTotal;
        }

        var vat = subtotal * (vatRatePercent / 100m);
        var grandTotal = subtotal + vat;

        return new QuotationTotals(subtotal, vat, grandTotal);
    }
}
