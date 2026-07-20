using RfqCopilot.Domain.Pricing;
using Xunit;

namespace RfqCopilot.Domain.Tests.Pricing;

public class PricingCalculatorTests
{
    [Fact]
    public void LineTotal_is_quantity_times_selling_price()
    {
        var input = new PricingInput(CostPrice: 30_000_000m, SellingPrice: 42_800_000m, Quantity: 2);

        var result = PricingCalculator.Calculate(input);

        Assert.Equal(85_600_000m, result.LineTotal);
    }

    [Fact]
    public void GrossProfit_is_selling_minus_cost()
    {
        var input = new PricingInput(CostPrice: 30_000_000m, SellingPrice: 42_800_000m, Quantity: 1);

        var result = PricingCalculator.Calculate(input);

        Assert.Equal(12_800_000m, result.GrossProfit);
    }

    [Fact]
    public void GrossProfitPercent_uses_selling_price_as_denominator()
    {
        // (100 - 60) / 100 * 100 = 40%
        var input = new PricingInput(CostPrice: 60m, SellingPrice: 100m, Quantity: 1);

        var result = PricingCalculator.Calculate(input);

        Assert.Equal(40m, result.GrossProfitPercent);
    }

    [Fact]
    public void Using_vendor_price_as_selling_price_gives_zero_gross_profit()
    {
        var input = new PricingInput(CostPrice: 25_000_000m, SellingPrice: 25_000_000m, Quantity: 1);

        var result = PricingCalculator.Calculate(input);

        Assert.Equal(0m, result.GrossProfit);
        Assert.Equal(0m, result.GrossProfitPercent);
    }

    [Fact]
    public void Zero_selling_price_throws_instead_of_dividing_by_zero()
    {
        var input = new PricingInput(CostPrice: 10m, SellingPrice: 0m, Quantity: 1);

        var ex = Assert.Throws<PricingValidationException>(() => PricingCalculator.Calculate(input));
        Assert.Contains("selling price", ex.Message, System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Negative_quantity_is_a_validation_error()
    {
        var input = new PricingInput(CostPrice: 10m, SellingPrice: 20m, Quantity: -1);

        Assert.Throws<PricingValidationException>(() => PricingCalculator.Calculate(input));
    }

    [Fact]
    public void Zero_quantity_is_a_validation_error()
    {
        var input = new PricingInput(CostPrice: 10m, SellingPrice: 20m, Quantity: 0);

        Assert.Throws<PricingValidationException>(() => PricingCalculator.Calculate(input));
    }

    [Fact]
    public void Negative_cost_price_is_a_validation_error()
    {
        var input = new PricingInput(CostPrice: -1m, SellingPrice: 20m, Quantity: 1);

        Assert.Throws<PricingValidationException>(() => PricingCalculator.Calculate(input));
    }

    [Fact]
    public void Negative_selling_price_is_a_validation_error()
    {
        var input = new PricingInput(CostPrice: 10m, SellingPrice: -5m, Quantity: 1);

        Assert.Throws<PricingValidationException>(() => PricingCalculator.Calculate(input));
    }

    [Fact]
    public void SellingPriceFromFactor_multiplies_cost_by_factor()
    {
        // Cost 30,000,000 x factor 1.4 = 42,000,000
        var selling = PricingCalculator.SellingPriceFromFactor(costPrice: 30_000_000m, factor: 1.4m);

        Assert.Equal(42_000_000m, selling);
    }

    [Fact]
    public void SellingPriceFromFactor_rejects_non_positive_factor()
    {
        Assert.Throws<PricingValidationException>(
            () => PricingCalculator.SellingPriceFromFactor(costPrice: 100m, factor: 0m));
    }
}
