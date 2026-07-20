using RfqCopilot.Domain.Normalization;
using Xunit;

namespace RfqCopilot.Domain.Tests.Normalization;

public class SignalNormalizerTests
{
    [Theory]
    [InlineData("4-20mA", "4-20mA")]
    [InlineData("4-20 mA", "4-20mA")]
    [InlineData("4 to 20mA", "4-20mA")]
    [InlineData("4...20mA", "4-20mA")]
    [InlineData("4 - 20 mA", "4-20mA")]
    [InlineData("4~20mA", "4-20mA")]
    [InlineData("4-20MA", "4-20mA")]
    public void Normalizes_4_20mA_variants(string raw, string expected)
    {
        var result = SignalNormalizer.Normalize(raw);
        Assert.Equal(expected, result.NormalizedValue);
        Assert.False(result.IsUnknown);
    }

    [Theory]
    [InlineData("4-20mA HART", "4-20mA_HART")]
    [InlineData("4-20 mA with HART", "4-20mA_HART")]
    [InlineData("4 to 20mA HART", "4-20mA_HART")]
    public void Detects_HART_on_top_of_4_20mA(string raw, string expected)
    {
        var result = SignalNormalizer.Normalize(raw);
        Assert.Equal(expected, result.NormalizedValue);
    }

    [Fact]
    public void Plain_4_20mA_is_not_upgraded_to_HART()
    {
        // Must NOT infer HART from 4-20mA alone.
        var result = SignalNormalizer.Normalize("4-20mA");
        Assert.Equal("4-20mA", result.NormalizedValue);
        Assert.DoesNotContain("HART", result.NormalizedValue);
    }

    [Fact]
    public void Unrecognized_signal_is_preserved_with_warning_not_dropped()
    {
        var result = SignalNormalizer.Normalize("PROFIBUS PA");
        Assert.True(result.IsUnknown);
        Assert.Equal("PROFIBUS PA", result.RawValue);
        Assert.False(string.IsNullOrWhiteSpace(result.Warning));
    }

    [Fact]
    public void Empty_input_is_unknown()
    {
        var result = SignalNormalizer.Normalize("");
        Assert.True(result.IsUnknown);
    }
}
