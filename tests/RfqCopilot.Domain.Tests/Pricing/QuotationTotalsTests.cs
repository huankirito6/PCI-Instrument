using System.Collections.Generic;
using RfqCopilot.Domain.Pricing;
using Xunit;

namespace RfqCopilot.Domain.Tests.Pricing;

public class QuotationTotalsTests
{
    [Fact]
    public void Subtotal_is_sum_of_line_totals()
    {
        var lines = new List<PricingResult>
        {
            PricingCalculator.Calculate(new PricingInput(30_000_000m, 42_800_000m, 1)),
            PricingCalculator.Calculate(new PricingInput(25_000_000m, 35_700_000m, 1)),
        };

        var totals = QuotationTotals.From(lines, vatRatePercent: 8m);

        Assert.Equal(78_500_000m, totals.Subtotal);
    }

    [Fact]
    public void Vat_is_subtotal_times_rate()
    {
        var lines = new List<PricingResult>
        {
            PricingCalculator.Calculate(new PricingInput(50_000_000m, 55_000_000m, 1)),
        };

        var totals = QuotationTotals.From(lines, vatRatePercent: 8m);

        Assert.Equal(55_000_000m, totals.Subtotal);
        Assert.Equal(4_400_000m, totals.Vat);
        Assert.Equal(59_400_000m, totals.GrandTotal);
    }

    [Fact]
    public void Zero_vat_rate_gives_grand_total_equal_to_subtotal()
    {
        var lines = new List<PricingResult>
        {
            PricingCalculator.Calculate(new PricingInput(10m, 20m, 3)),
        };

        var totals = QuotationTotals.From(lines, vatRatePercent: 0m);

        Assert.Equal(60m, totals.Subtotal);
        Assert.Equal(0m, totals.Vat);
        Assert.Equal(60m, totals.GrandTotal);
    }

    [Fact]
    public void Negative_vat_rate_is_a_validation_error()
    {
        var lines = new List<PricingResult>
        {
            PricingCalculator.Calculate(new PricingInput(10m, 20m, 1)),
        };

        Assert.Throws<PricingValidationException>(
            () => QuotationTotals.From(lines, vatRatePercent: -1m));
    }
}
