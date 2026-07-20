namespace RfqCopilot.Domain.Requirements;

/// <summary>
/// A single requirement field value with its status and (optional) raw value.
/// </summary>
/// <param name="FieldKey">Schema field key, e.g. "measurement_range_max".</param>
/// <param name="Status">Lifecycle status of this value.</param>
/// <param name="Value">Normalized value, or null when missing/unknown.</param>
public sealed record RequirementValue(string FieldKey, RequirementStatus Status, object? Value);
