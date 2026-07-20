using System.Collections.Generic;
using System.Linq;
using RfqCopilot.Domain.Matching;
using Xunit;

namespace RfqCopilot.Domain.Tests.Matching;

public class CandidateMatchingEngineTests
{
    // ---- Helpers -----------------------------------------------------------

    private static CatalogProduct Product(string partNumber, params (string key, string value)[] attrs) =>
        new(
            PartNumber: partNumber,
            Manufacturer: "Yokogawa",
            ModelSeries: partNumber.Split('-')[0],
            Attributes: attrs.ToDictionary(a => a.key, a => a.value),
            Source: "SYNTHETIC-CATALOG");

    private static MatchQuery GaugePtQuery(params (string key, string value)[] extra)
    {
        var required = new Dictionary<string, string>
        {
            ["product_family"] = "pressure_transmitter",
            ["measurement_type"] = "gauge_pressure",
            ["output_signal"] = "4-20mA_HART",
        };
        foreach (var (k, v) in extra)
        {
            required[k] = v;
        }

        return new MatchQuery(
            HardConstraints:
            [
                new HardConstraint("product_family"),
                new HardConstraint("measurement_type"),
                new HardConstraint("output_signal"),
            ],
            OptionalPreferences: ["wetted_material", "display_requirement"],
            Requirements: required);
    }

    // ---- Tests -------------------------------------------------------------

    [Fact]
    public void Candidate_violating_a_hard_constraint_is_rejected()
    {
        var query = GaugePtQuery();
        var products = new List<CatalogProduct>
        {
            // measurement_type differs -> hard-constraint violation
            Product("EJA110E-A",
                ("product_family", "pressure_transmitter"),
                ("measurement_type", "differential_pressure"),
                ("output_signal", "4-20mA_HART")),
        };

        var result = CandidateMatchingEngine.Match(query, products);

        var candidate = Assert.Single(result.Candidates);
        Assert.Equal(CandidateStatus.Rejected, candidate.Status);
        Assert.Contains(candidate.FailedConstraints, f => f.Contains("measurement_type"));
    }

    [Fact]
    public void Candidate_meeting_all_hard_constraints_is_eligible()
    {
        var query = GaugePtQuery();
        var products = new List<CatalogProduct>
        {
            Product("EJA430E-A",
                ("product_family", "pressure_transmitter"),
                ("measurement_type", "gauge_pressure"),
                ("output_signal", "4-20mA_HART")),
        };

        var result = CandidateMatchingEngine.Match(query, products);

        var candidate = Assert.Single(result.Candidates);
        Assert.Equal(CandidateStatus.Eligible, candidate.Status);
        Assert.Empty(candidate.FailedConstraints);
    }

    [Fact]
    public void Unknown_attribute_does_not_count_as_a_pass_on_a_hard_constraint()
    {
        var query = GaugePtQuery();
        var products = new List<CatalogProduct>
        {
            // output_signal attribute is absent from catalog -> unknown, must NOT pass
            Product("EJA430E-B",
                ("product_family", "pressure_transmitter"),
                ("measurement_type", "gauge_pressure")),
        };

        var result = CandidateMatchingEngine.Match(query, products);

        var candidate = Assert.Single(result.Candidates);
        Assert.Equal(CandidateStatus.Rejected, candidate.Status);
        Assert.Contains("output_signal", candidate.UnknownRequirements);
        Assert.DoesNotContain("output_signal", candidate.MatchedRequirements);
    }

    [Fact]
    public void Matched_unmatched_unknown_buckets_are_populated()
    {
        var query = GaugePtQuery(("wetted_material", "316L SST"));
        var products = new List<CatalogProduct>
        {
            Product("EJA430E-C",
                ("product_family", "pressure_transmitter"),
                ("measurement_type", "gauge_pressure"),
                ("output_signal", "4-20mA_HART"),
                ("wetted_material", "316L SST")),
                // display_requirement absent -> unknown (it is an optional preference in query)
        };

        var result = CandidateMatchingEngine.Match(query, products);
        var candidate = Assert.Single(result.Candidates);

        Assert.Contains("product_family", candidate.MatchedRequirements);
        Assert.Contains("wetted_material", candidate.MatchedRequirements);
        Assert.Contains("display_requirement", candidate.UnknownRequirements);
    }

    [Fact]
    public void Attribute_value_mismatch_on_optional_is_unmatched_not_unknown()
    {
        var query = GaugePtQuery(("wetted_material", "Hastelloy C-276"));
        var products = new List<CatalogProduct>
        {
            Product("EJA430E-D",
                ("product_family", "pressure_transmitter"),
                ("measurement_type", "gauge_pressure"),
                ("output_signal", "4-20mA_HART"),
                ("wetted_material", "316L SST")), // present but different -> unmatched
        };

        var result = CandidateMatchingEngine.Match(query, products);
        var candidate = Assert.Single(result.Candidates);

        Assert.Contains("wetted_material", candidate.UnmatchedRequirements);
        Assert.DoesNotContain("wetted_material", candidate.UnknownRequirements);
    }

    [Fact]
    public void Eligible_candidate_ranks_above_one_missing_optional_fields()
    {
        var query = GaugePtQuery(("wetted_material", "316L SST"), ("display_requirement", "LCD"));
        var products = new List<CatalogProduct>
        {
            // Covers both optional preferences
            Product("EJA430E-FULL",
                ("product_family", "pressure_transmitter"),
                ("measurement_type", "gauge_pressure"),
                ("output_signal", "4-20mA_HART"),
                ("wetted_material", "316L SST"),
                ("display_requirement", "LCD")),
            // Missing both optional preferences (unknown)
            Product("EJA430E-BARE",
                ("product_family", "pressure_transmitter"),
                ("measurement_type", "gauge_pressure"),
                ("output_signal", "4-20mA_HART")),
        };

        var result = CandidateMatchingEngine.Match(query, products);

        Assert.Equal("EJA430E-FULL", result.Candidates[0].PartNumber);
        Assert.True(result.Candidates[0].Score >= result.Candidates[1].Score);
    }

    [Fact]
    public void Rejected_candidates_rank_below_eligible_ones()
    {
        var query = GaugePtQuery();
        var products = new List<CatalogProduct>
        {
            Product("BAD",
                ("product_family", "differential_pressure_transmitter"),
                ("measurement_type", "differential_pressure"),
                ("output_signal", "4-20mA_HART")),
            Product("GOOD",
                ("product_family", "pressure_transmitter"),
                ("measurement_type", "gauge_pressure"),
                ("output_signal", "4-20mA_HART")),
        };

        var result = CandidateMatchingEngine.Match(query, products);

        Assert.Equal("GOOD", result.Candidates[0].PartNumber);
        Assert.Equal(CandidateStatus.Eligible, result.Candidates[0].Status);
        Assert.Equal(CandidateStatus.Rejected, result.Candidates[1].Status);
    }

    [Fact]
    public void Result_is_stable_for_the_same_inputs()
    {
        var query = GaugePtQuery(("wetted_material", "316L SST"));
        var products = new List<CatalogProduct>
        {
            Product("EJA430E-1",
                ("product_family", "pressure_transmitter"),
                ("measurement_type", "gauge_pressure"),
                ("output_signal", "4-20mA_HART"),
                ("wetted_material", "316L SST")),
            Product("EJA430E-2",
                ("product_family", "pressure_transmitter"),
                ("measurement_type", "gauge_pressure"),
                ("output_signal", "4-20mA_HART")),
        };

        var first = CandidateMatchingEngine.Match(query, products);
        var second = CandidateMatchingEngine.Match(query, products);

        Assert.Equal(
            first.Candidates.Select(c => c.PartNumber),
            second.Candidates.Select(c => c.PartNumber));
    }

    [Fact]
    public void Every_candidate_records_its_catalog_source()
    {
        var query = GaugePtQuery();
        var products = new List<CatalogProduct>
        {
            Product("EJA430E-S",
                ("product_family", "pressure_transmitter"),
                ("measurement_type", "gauge_pressure"),
                ("output_signal", "4-20mA_HART")),
        };

        var result = CandidateMatchingEngine.Match(query, products);

        Assert.Equal("SYNTHETIC-CATALOG", result.Candidates[0].Source);
    }

    [Fact]
    public void No_candidate_is_ever_marked_verified_by_the_engine()
    {
        var query = GaugePtQuery();
        var products = new List<CatalogProduct>
        {
            Product("EJA430E-V",
                ("product_family", "pressure_transmitter"),
                ("measurement_type", "gauge_pressure"),
                ("output_signal", "4-20mA_HART")),
        };

        var result = CandidateMatchingEngine.Match(query, products);

        // Engine only proposes Eligible/Rejected; "verified" is a human decision, not a status here.
        Assert.All(result.Candidates,
            c => Assert.True(c.Status is CandidateStatus.Eligible or CandidateStatus.Rejected));
    }

    [Fact]
    public void Empty_catalog_yields_no_candidates()
    {
        var query = GaugePtQuery();
        var result = CandidateMatchingEngine.Match(query, new List<CatalogProduct>());

        Assert.Empty(result.Candidates);
    }
}
