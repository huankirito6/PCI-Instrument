using System;
using System.Collections.Generic;
using System.Linq;

namespace RfqCopilot.Domain.Matching;

/// <summary>
/// Deterministic candidate matching.
///
/// Rules:
///   - A hard-constraint field passes ONLY when the catalog value is present and matches
///     the desired value. Unknown (absent) or mismatched hard-constraint fields reject
///     the candidate. Unknown never counts as a pass.
///   - Optional preferences influence the score but never reject a candidate.
///   - Every candidate exposes matched/unmatched/unknown buckets, failed constraints,
///     its score, and its catalog source.
///   - The engine only proposes Eligible/Rejected; a human decides "verified".
///   - Ranking is deterministic: eligible candidates first, then higher score, then
///     original catalog order (stable).
/// </summary>
public static class CandidateMatchingEngine
{
    private enum FieldOutcome
    {
        Matched,
        Unmatched,
        Unknown,
    }

    public static MatchResult Match(MatchQuery query, IReadOnlyList<CatalogProduct> products)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(products);

        // Fields to evaluate for the buckets: hard-constraint keys + optional preferences (distinct, ordered).
        var fieldsToEvaluate = query.HardConstraints
            .Select(h => h.FieldKey)
            .Concat(query.OptionalPreferences)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var hardKeys = new HashSet<string>(
            query.HardConstraints.Select(h => h.FieldKey), StringComparer.OrdinalIgnoreCase);

        var candidates = new List<Candidate>();

        foreach (var product in products)
        {
            var matched = new List<string>();
            var unmatched = new List<string>();
            var unknown = new List<string>();
            var failedConstraints = new List<string>();

            foreach (var field in fieldsToEvaluate)
            {
                var outcome = Evaluate(field, query, product, out var desired, out var actual);

                switch (outcome)
                {
                    case FieldOutcome.Matched:
                        matched.Add(field);
                        break;
                    case FieldOutcome.Unmatched:
                        unmatched.Add(field);
                        if (hardKeys.Contains(field))
                        {
                            failedConstraints.Add(
                                $"{field}: required '{desired}' but catalog has '{actual}'.");
                        }
                        break;
                    case FieldOutcome.Unknown:
                        unknown.Add(field);
                        if (hardKeys.Contains(field))
                        {
                            failedConstraints.Add(
                                $"{field}: unknown (catalog has no value; unknown never passes).");
                        }
                        break;
                }
            }

            var status = failedConstraints.Count == 0 ? CandidateStatus.Eligible : CandidateStatus.Rejected;
            var score = matched.Count;

            candidates.Add(new Candidate(
                PartNumber: product.PartNumber,
                ModelSeries: product.ModelSeries,
                Source: product.Source,
                Status: status,
                Score: score,
                MatchedRequirements: matched,
                UnmatchedRequirements: unmatched,
                UnknownRequirements: unknown,
                FailedConstraints: failedConstraints));
        }

        // Eligible first, then higher score; OrderBy/ThenBy are stable so ties keep catalog order.
        var ranked = candidates
            .OrderBy(c => c.Status == CandidateStatus.Rejected ? 1 : 0)
            .ThenByDescending(c => c.Score)
            .ToList();

        return new MatchResult(ranked);
    }

    private static FieldOutcome Evaluate(
        string field,
        MatchQuery query,
        CatalogProduct product,
        out string? desired,
        out string? actual)
    {
        query.Requirements.TryGetValue(field, out desired);
        var hasActual = product.Attributes.TryGetValue(field, out actual);

        if (!hasActual || actual is null)
        {
            return FieldOutcome.Unknown;
        }

        if (desired is null)
        {
            // Product supplies a value and the query expresses no specific desire.
            return FieldOutcome.Matched;
        }

        return string.Equals(actual, desired, StringComparison.OrdinalIgnoreCase)
            ? FieldOutcome.Matched
            : FieldOutcome.Unmatched;
    }
}
