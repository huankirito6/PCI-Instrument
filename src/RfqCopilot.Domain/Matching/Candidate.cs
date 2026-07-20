using System.Collections.Generic;

namespace RfqCopilot.Domain.Matching;

/// <summary>
/// Outcome for a candidate as proposed by the engine. The engine only proposes
/// Eligible/Rejected; treating a candidate as verified is a human decision.
/// </summary>
public enum CandidateStatus
{
    /// <summary>All hard constraints matched; proposal is eligible for human review.</summary>
    Eligible,

    /// <summary>At least one hard constraint failed (unmatched or unknown).</summary>
    Rejected,
}

/// <summary>
/// An explainable match candidate. Every bucket is exposed so a reviewer can see
/// exactly why a product was proposed or rejected.
/// </summary>
public sealed record Candidate(
    string PartNumber,
    string ModelSeries,
    string Source,
    CandidateStatus Status,
    int Score,
    IReadOnlyList<string> MatchedRequirements,
    IReadOnlyList<string> UnmatchedRequirements,
    IReadOnlyList<string> UnknownRequirements,
    IReadOnlyList<string> FailedConstraints);

/// <summary>Result of a matching run: candidates ranked eligible-first, highest score first.</summary>
public sealed record MatchResult(IReadOnlyList<Candidate> Candidates);
