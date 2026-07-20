namespace RfqCopilot.Domain.Requirements;

/// <summary>
/// Lifecycle status of a single extracted/entered requirement field.
/// Missing data must stay Missing/Unknown/null; it is never fabricated.
/// </summary>
public enum RequirementStatus
{
    Provided,
    Extracted,
    Missing,
    Unknown,
    Unverified,
    Approved,
    Rejected,
    Conflicting,
}
