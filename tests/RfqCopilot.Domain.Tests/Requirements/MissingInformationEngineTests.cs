using System.Collections.Generic;
using System.Linq;
using RfqCopilot.Domain.Requirements;
using Xunit;

namespace RfqCopilot.Domain.Tests.Requirements;

public class MissingInformationEngineTests
{
    private static RuleSet PressureDpRules() => new(
        RequiredFields:
        [
            new RequiredFieldRule("product_family", "Needed to route to the right selection rules."),
            new RequiredFieldRule("measurement_type", "Needed to pick gauge/absolute/differential."),
            new RequiredFieldRule("measurement_range_max", "Needed to select sensor range."),
            new RequiredFieldRule("output_signal", "Needed to select electronics/output option."),
            new RequiredFieldRule("wetted_material", "Needed to select wetted parts option."),
            new RequiredFieldRule("quantity", "Needed for the quotation line."),
        ],
        ConditionalRules:
        [
            new ConditionalRule(
                Id: "hazardous-area-certification",
                When: new FieldCondition("hazardous_area_requirement", ConditionOperator.IsPresent, null),
                Requires: ["certification"],
                Reason: "Hazardous-area selection must be backed by an explicit certification."),
            new ConditionalRule(
                Id: "differential-pressure-static-pressure",
                When: new FieldCondition("measurement_type", ConditionOperator.Equals, "differential_pressure"),
                Requires: ["static_pressure_or_overpressure"],
                Reason: "DP selection requires static/overpressure information."),
            new ConditionalRule(
                Id: "remote-seal-details",
                When: new FieldCondition("remote_seal", ConditionOperator.Equals, "true"),
                Requires: ["diaphragm_or_seal_type", "process_temperature_max", "manifold_and_accessories"],
                Reason: "Remote-seal configurations require seal/process/accessory details."),
        ]);

    private static RequirementValue Present(string key, object? value = null) =>
        new(key, RequirementStatus.Extracted, value ?? "x");

    private static RequirementValue Missing(string key) =>
        new(key, RequirementStatus.Missing, null);

    [Fact]
    public void Missing_required_field_is_reported()
    {
        var reqs = new List<RequirementValue>
        {
            Present("product_family", "pressure_transmitter"),
            Present("measurement_type", "gauge_pressure"),
            Present("measurement_range_max", 1m),
            Present("output_signal", "4-20mA_HART"),
            Present("quantity", 2),
            // wetted_material intentionally absent
        };

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        Assert.Contains(report, r => r.FieldKey == "wetted_material");
    }

    [Fact]
    public void Provided_required_field_is_not_reported()
    {
        var reqs = new List<RequirementValue>
        {
            Present("product_family", "pressure_transmitter"),
            Present("measurement_type", "gauge_pressure"),
            Present("measurement_range_max", 1m),
            Present("output_signal", "4-20mA_HART"),
            Present("wetted_material", "316L SST"),
            Present("quantity", 2),
        };

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        Assert.DoesNotContain(report, r => r.FieldKey == "wetted_material");
    }

    [Fact]
    public void Required_field_present_but_null_value_is_reported()
    {
        var reqs = new List<RequirementValue>
        {
            new("wetted_material", RequirementStatus.Missing, null),
        };

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        Assert.Contains(report, r => r.FieldKey == "wetted_material");
    }

    [Fact]
    public void Field_not_listed_at_all_counts_as_missing()
    {
        var reqs = new List<RequirementValue>(); // nothing provided

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        // All six required fields should be reported.
        Assert.Equal(6, report.Count(r => r.Kind == MissingKind.RequiredMissing));
    }

    [Fact]
    public void Unknown_status_counts_as_missing()
    {
        var reqs = new List<RequirementValue>
        {
            new("wetted_material", RequirementStatus.Unknown, null),
        };

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        Assert.Contains(report, r => r.FieldKey == "wetted_material");
    }

    [Fact]
    public void Unverified_field_is_flagged_as_warning_not_satisfied()
    {
        var reqs = new List<RequirementValue>
        {
            new("wetted_material", RequirementStatus.Unverified, "maybe 316L"),
        };

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        Assert.Contains(report, r => r.FieldKey == "wetted_material" && r.Kind == MissingKind.Unverified);
    }

    [Fact]
    public void Hazardous_area_present_triggers_certification_requirement()
    {
        var reqs = new List<RequirementValue>
        {
            Present("hazardous_area_requirement", "Ex"),
            // certification absent
        };

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        Assert.Contains(report, r => r.FieldKey == "certification" && r.Kind == MissingKind.ConditionalMissing);
    }

    [Fact]
    public void Hazardous_area_absent_does_not_trigger_certification()
    {
        var reqs = new List<RequirementValue>
        {
            Present("product_family", "pressure_transmitter"),
            // no hazardous_area_requirement at all
        };

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        Assert.DoesNotContain(report, r => r.FieldKey == "certification");
    }

    [Fact]
    public void Differential_pressure_triggers_static_pressure_requirement()
    {
        var reqs = new List<RequirementValue>
        {
            Present("measurement_type", "differential_pressure"),
            // static_pressure_or_overpressure absent
        };

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        Assert.Contains(report, r => r.FieldKey == "static_pressure_or_overpressure"
                                     && r.Kind == MissingKind.ConditionalMissing);
    }

    [Fact]
    public void Gauge_pressure_does_not_trigger_static_pressure_requirement()
    {
        var reqs = new List<RequirementValue>
        {
            Present("measurement_type", "gauge_pressure"),
        };

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        Assert.DoesNotContain(report, r => r.FieldKey == "static_pressure_or_overpressure");
    }

    [Fact]
    public void Remote_seal_true_triggers_all_three_seal_requirements()
    {
        var reqs = new List<RequirementValue>
        {
            new("remote_seal", RequirementStatus.Extracted, "true"),
            // seal type, temp, accessories all absent
        };

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        Assert.Contains(report, r => r.FieldKey == "diaphragm_or_seal_type");
        Assert.Contains(report, r => r.FieldKey == "process_temperature_max");
        Assert.Contains(report, r => r.FieldKey == "manifold_and_accessories");
    }

    [Fact]
    public void Remote_seal_false_does_not_trigger_seal_requirements()
    {
        var reqs = new List<RequirementValue>
        {
            new("remote_seal", RequirementStatus.Extracted, "false"),
        };

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        Assert.DoesNotContain(report, r => r.FieldKey == "diaphragm_or_seal_type");
    }

    [Fact]
    public void Conditional_requirement_already_satisfied_is_not_reported()
    {
        var reqs = new List<RequirementValue>
        {
            Present("measurement_type", "differential_pressure"),
            Present("static_pressure_or_overpressure", "10 MPa"),
        };

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        Assert.DoesNotContain(report, r => r.FieldKey == "static_pressure_or_overpressure");
    }

    [Fact]
    public void Each_missing_field_carries_a_reason()
    {
        var reqs = new List<RequirementValue>();

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        Assert.All(report, r => Assert.False(string.IsNullOrWhiteSpace(r.Reason)));
    }

    [Fact]
    public void Report_does_not_duplicate_a_field_across_required_and_conditional()
    {
        // wetted_material is required; ensure it appears at most once.
        var reqs = new List<RequirementValue>
        {
            Present("measurement_type", "differential_pressure"),
        };

        var report = MissingInformationEngine.Detect(reqs, PressureDpRules());

        Assert.True(report.Count(r => r.FieldKey == "static_pressure_or_overpressure") <= 1);
        Assert.True(report.Count(r => r.FieldKey == "wetted_material") <= 1);
    }
}
