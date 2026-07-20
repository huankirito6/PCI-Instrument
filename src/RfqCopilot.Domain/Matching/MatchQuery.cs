using System.Collections.Generic;

namespace RfqCopilot.Domain.Matching;

/// <summary>A hard constraint: this field must match, or the candidate is rejected.</summary>
/// <param name="FieldKey">The requirement field that must match.</param>
public sealed record HardConstraint(string FieldKey);

/// <summary>
/// The query used to match catalog products against reviewed requirements.
/// </summary>
/// <param name="HardConstraints">Fields that must match; a failure rejects the candidate.</param>
/// <param name="OptionalPreferences">Fields that improve the score when matched but never reject.</param>
/// <param name="Requirements">Desired values keyed by field. A field with no desired value here
/// is only checked for presence.</param>
public sealed record MatchQuery(
    IReadOnlyList<HardConstraint> HardConstraints,
    IReadOnlyList<string> OptionalPreferences,
    IReadOnlyDictionary<string, string> Requirements);
