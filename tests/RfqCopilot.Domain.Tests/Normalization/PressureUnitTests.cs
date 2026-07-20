using RfqCopilot.Domain.Normalization;
using Xunit;

namespace RfqCopilot.Domain.Tests.Normalization;

public class PressureUnitTests
{
    [Theory]
    [InlineData(1, "MPa", 1000)]      // 1 MPa = 1000 kPa
    [InlineData(1, "bar", 100)]       // 1 bar = 100 kPa
    [InlineData(1000, "mbar", 100)]   // 1000 mbar = 100 kPa
    [InlineData(0, "MPa", 0)]         // boundary: zero
    [InlineData(2, "MPa", 2000)]      // 2 MPa = 2000 kPa
    public void ToKilopascals_converts_correctly(decimal value, string unit, decimal expectedKpa)
    {
        var result = PressureUnit.ToKilopascals(value, unit);
        Assert.Equal(expectedKpa, result);
    }

    [Fact]
    public void Convert_bar_to_MPa()
    {
        // 10 bar = 1 MPa
        var result = PressureUnit.Convert(10m, "bar", "MPa");
        Assert.Equal(1m, result);
    }

    [Fact]
    public void Convert_kPa_to_MPa()
    {
        // 100 kPa = 0.1 MPa
        var result = PressureUnit.Convert(100m, "kPa", "MPa");
        Assert.Equal(0.1m, result);
    }

    [Fact]
    public void Convert_is_case_insensitive_on_units()
    {
        var result = PressureUnit.Convert(1m, "mpa", "KPA");
        Assert.Equal(1000m, result);
    }

    [Fact]
    public void Convert_same_unit_returns_same_value()
    {
        var result = PressureUnit.Convert(42m, "kPa", "kPa");
        Assert.Equal(42m, result);
    }

    [Fact]
    public void Unknown_source_unit_throws()
    {
        Assert.Throws<NormalizationException>(() => PressureUnit.Convert(1m, "psi_not_supported", "kPa"));
    }

    [Fact]
    public void Unknown_target_unit_throws()
    {
        Assert.Throws<NormalizationException>(() => PressureUnit.Convert(1m, "kPa", "furlongs"));
    }

    [Fact]
    public void IsSupported_reports_known_and_unknown_units()
    {
        Assert.True(PressureUnit.IsSupported("MPa"));
        Assert.True(PressureUnit.IsSupported("bar"));
        Assert.True(PressureUnit.IsSupported("kPa"));
        Assert.True(PressureUnit.IsSupported("mbar"));
        Assert.False(PressureUnit.IsSupported("psi"));
    }
}
