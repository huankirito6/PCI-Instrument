namespace RfqCopilot.Domain.Requirements;

/// <summary>Why a field appears in the missing-information report.</summary>
public enum MissingKind
{
    /// <summary>An always-required field is absent/missing/unknown.</summary>
    RequiredMissing,

    /// <summary>A field required by an active conditional rule is absent/missing/unknown.</summary>
    ConditionalMissing,

    /// <summary>The field has a value but is unverified; it needs confirmation, not invention.</summary>
    Unverified,
}

/// <summary>One entry in the missing-information report.</summary>
/// <param name="FieldKey">The field that needs attention.</param>
/// <param name="Kind">Why it is reported.</param>
/// <param name="Reason">Human-readable explanation for the reviewer.</param>
public sealed record MissingItem(string FieldKey, MissingKind Kind, string Reason);
