namespace RfqCopilot.Domain.Pricing;

/// <summary>
/// Deterministic pricing math for quotation lines.
///
/// Formulas (as approved in the survey / roadmap):
///   LineTotal          = Quantity * SellingPrice
///   GrossProfit        = SellingPrice - CostPrice
///   GrossProfitPercent = (SellingPrice - CostPrice) / SellingPrice * 100
///
/// The tool only calculates; it never decides the selling price or factor —
/// those are entered and approved by a human.
/// </summary>
public static class PricingCalculator
{
    public static PricingResult Calculate(PricingInput input)
    {
        if (input.CostPrice < 0m)
        {
            throw new PricingValidationException("Cost price cannot be negative.");
        }

        if (input.SellingPrice < 0m)
        {
            throw new PricingValidationException("Selling price cannot be negative.");
        }

        if (input.Quantity <= 0)
        {
            throw new PricingValidationException("Quantity must be a positive integer.");
        }

        if (input.SellingPrice == 0m)
        {
            throw new PricingValidationException(
                "Selling price must be greater than zero to compute gross profit percent.");
        }

        var grossProfit = input.SellingPrice - input.CostPrice;
        var grossProfitPercent = grossProfit / input.SellingPrice * 100m;
        var lineTotal = input.Quantity * input.SellingPrice;

        return new PricingResult(lineTotal, grossProfit, grossProfitPercent);
    }

    /// <summary>
    /// Helper for the "enter a factor" path: SellingPrice = CostPrice * factor.
    /// The result is still subject to human review before export.
    /// </summary>
    public static decimal SellingPriceFromFactor(decimal costPrice, decimal factor)
    {
        if (costPrice < 0m)
        {
            throw new PricingValidationException("Cost price cannot be negative.");
        }

        if (factor <= 0m)
        {
            throw new PricingValidationException("Pricing factor must be greater than zero.");
        }

        return costPrice * factor;
    }
}
