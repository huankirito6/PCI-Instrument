using System.Linq;
using RfqCopilot.Domain.Vendor;
using Xunit;

namespace RfqCopilot.Domain.Tests.Vendor;

public class VendorQuoteMismatchDetectorTests
{
    private static ProposedLine Proposed(
        string model = "EJA430E-JAS4J-712ED/HE",
        string range = "0-1 MPa",
        int qty = 2) => new(model, range, qty);

    private static VendorConfirmedLine Vendor(
        string model = "EJA430E-JAS4J-712ED/HE",
        string range = "0-1 MPa",
        int qty = 2) => new(model, range, qty);

    [Fact]
    public void No_mismatch_when_model_range_and_quantity_all_agree()
    {
        var result = VendorQuoteMismatchDetector.Compare(Proposed(), Vendor());

        Assert.False(result.HasMismatch);
        Assert.Empty(result.Warnings);
    }

    [Fact]
    public void Model_mismatch_is_flagged()
    {
        var result = VendorQuoteMismatchDetector.Compare(
            Proposed(model: "EJA430E-JAS4J-712ED/HE"),
            Vendor(model: "EJA430E-DAS4J-712ED/HE"));

        Assert.True(result.HasMismatch);
        Assert.Contains(result.Warnings, w => w.Field == MismatchField.Model);
    }

    [Fact]
    public void Model_comparison_ignores_case_and_surrounding_whitespace()
    {
        var result = VendorQuoteMismatchDetector.Compare(
            Proposed(model: "  eja430e-jas4j-712ed/he "),
            Vendor(model: "EJA430E-JAS4J-712ED/HE"));

        Assert.False(result.HasMismatch);
    }

    [Fact]
    public void Range_mismatch_is_flagged()
    {
        var result = VendorQuoteMismatchDetector.Compare(
            Proposed(range: "0-1 MPa"),
            Vendor(range: "0-2 MPa"));

        Assert.True(result.HasMismatch);
        Assert.Contains(result.Warnings, w => w.Field == MismatchField.Range);
    }

    [Fact]
    public void Quantity_mismatch_is_flagged()
    {
        var result = VendorQuoteMismatchDetector.Compare(
            Proposed(qty: 2),
            Vendor(qty: 3));

        Assert.True(result.HasMismatch);
        Assert.Contains(result.Warnings, w => w.Field == MismatchField.Quantity);
    }

    [Fact]
    public void Multiple_mismatches_are_all_reported()
    {
        var result = VendorQuoteMismatchDetector.Compare(
            Proposed(model: "A", range: "0-1 MPa", qty: 2),
            Vendor(model: "B", range: "0-2 MPa", qty: 5));

        Assert.Equal(3, result.Warnings.Count);
        Assert.Contains(result.Warnings, w => w.Field == MismatchField.Model);
        Assert.Contains(result.Warnings, w => w.Field == MismatchField.Range);
        Assert.Contains(result.Warnings, w => w.Field == MismatchField.Quantity);
    }

    [Fact]
    public void Every_warning_carries_both_proposed_and_vendor_values()
    {
        var result = VendorQuoteMismatchDetector.Compare(
            Proposed(qty: 2),
            Vendor(qty: 3));

        var warning = result.Warnings.Single(w => w.Field == MismatchField.Quantity);
        Assert.Equal("2", warning.ProposedValue);
        Assert.Equal("3", warning.VendorValue);
    }

    [Fact]
    public void Missing_vendor_model_is_flagged_not_silently_accepted()
    {
        var result = VendorQuoteMismatchDetector.Compare(
            Proposed(model: "EJA430E-JAS4J-712ED/HE"),
            Vendor(model: ""));

        Assert.True(result.HasMismatch);
        Assert.Contains(result.Warnings, w => w.Field == MismatchField.Model);
    }

    [Fact]
    public void Range_comparison_ignores_case_and_spacing_but_not_values()
    {
        // Same meaning, different spacing/case -> no mismatch.
        var same = VendorQuoteMismatchDetector.Compare(
            Proposed(range: "0-1 MPa"),
            Vendor(range: "0-1 mpa"));
        Assert.False(same.HasMismatch);

        // Different numeric value -> mismatch.
        var diff = VendorQuoteMismatchDetector.Compare(
            Proposed(range: "0-1 MPa"),
            Vendor(range: "0-10 MPa"));
        Assert.True(diff.HasMismatch);
    }
}
