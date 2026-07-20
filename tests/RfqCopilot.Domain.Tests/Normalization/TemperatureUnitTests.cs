using RfqCopilot.Domain.Normalization;
using Xunit;

namespace RfqCopilot.Domain.Tests.Normalization;

public class TemperatureUnitTests
{
    [Theory]
    [InlineData(0, 32)]      // 0 C = 32 F
    [InlineData(100, 212)]   // 100 C = 212 F
    [InlineData(-40, -40)]   // boundary: -40 is equal in both scales
    [InlineData(37, 98.6)]   // body temp
    public void CelsiusToFahrenheit(decimal celsius, decimal expectedF)
    {
        Assert.Equal(expectedF, TemperatureUnit.CelsiusToFahrenheit(celsius));
    }

    [Theory]
    [InlineData(32, 0)]
    [InlineData(212, 100)]
    [InlineData(-40, -40)]
    public void FahrenheitToCelsius(decimal fahrenheit, decimal expectedC)
    {
        Assert.Equal(expectedC, TemperatureUnit.FahrenheitToCelsius(fahrenheit));
    }

    [Fact]
    public void Convert_C_to_F()
    {
        Assert.Equal(212m, TemperatureUnit.Convert(100m, "C", "F"));
    }

    [Fact]
    public void Convert_accepts_degree_prefixed_units()
    {
        Assert.Equal(0m, TemperatureUnit.Convert(32m, "degF", "degC"));
    }

    [Fact]
    public void Convert_same_unit_returns_same_value()
    {
        Assert.Equal(80m, TemperatureUnit.Convert(80m, "C", "C"));
    }

    [Fact]
    public void Unknown_unit_throws()
    {
        Assert.Throws<NormalizationException>(() => TemperatureUnit.Convert(1m, "kelvin", "C"));
    }
}
